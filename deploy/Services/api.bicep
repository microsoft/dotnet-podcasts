@description('Location for all resources.')
param location string = resourceGroup().location
param imageTag string

@secure()
param administratorLoginPassword string
@secure()
param acrPassword string
param acrLogin string
param acrLoginServer string
param serverName string
param sqlDBName string = 'Podcast'
param administratorLogin string
param storageAccountName string
param kubernetesEnvName string
param workspaceName string

var workspaceId = workspace.id
var kubernetesEnvId = kubernetesEnv.id
var sqlServerHostname = environment().suffixes.sqlServerHostname
var podcastDbConnectionString = 'Server=tcp:${serverName}.${sqlServerHostname},1433;Initial Catalog=${sqlDBName};Persist Security Info=False;User ID=${administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
var podcastApiImage = '${acrLoginServer}/podcastapi:${imageTag}'
var podcastUpdaterImage = '${acrLoginServer}/podcastupdaterworker:${imageTag}'
var podcastIngestionWorkerImage = '${acrLoginServer}/podcastingestionworker:${imageTag}'
var storageEnv = environment().suffixes.storage
var imagesStorage = 'https://${storageAccountName}.blob.${storageEnv}/covers/'
var deployIngestion = false

resource sqlServer 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource sqlDB 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {
  parent: sqlServer
  name: sqlDBName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

resource sqlFirewallRule 'Microsoft.Sql/servers/firewallRules@2021-11-01' = if (true) {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
  }
}

resource feedQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2021-06-01' = {
  name: '${storageAccountName}/default/feed-queue'
  dependsOn: [
    storageAccount
  ]
}

resource workspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: workspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    features: {
      searchVersion: 1
      legacy: 0
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

resource kubernetesEnv 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: kubernetesEnvName
  location: location
  
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: reference(workspaceId, '2015-03-20').customerId
        sharedKey: listKeys(workspaceId, '2015-11-01-preview').primarySharedKey
      }
    }
  }
}

resource apiContainerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: 'podcastapica'
  location: location
  properties: {
    managedEnvironmentId: kubernetesEnvId
    configuration: {
      activeRevisionsMode: 'single'
      ingress: {
        external: true
        targetPort: 80
      }
      registries: [
        {
          server: acrLoginServer
          username: acrLogin
          passwordSecretRef: 'acr-password'
        }
      ]
      secrets: [
        {
          name: 'feedqueue'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=core.windows.net;AccountKey=${listKeys(storageAccount.id, '2019-06-01').keys[0].value}'
        }
        {
          name: 'podcastdb'
          value: podcastDbConnectionString
        }
        {
          name: 'acr-password'
          value: acrPassword
        }
      ]
    }
    template: {
      containers: [
        {
          image: podcastApiImage
          name: 'podcastapi'
          resources: {
            cpu: 1
            memory: '2Gi'
          }
          env: [
            {
              name: 'ConnectionStrings__FeedQueue'
              secretRef: 'feedqueue'
            }
            {
              name: 'ConnectionStrings__PodcastDb'
              secretRef: 'podcastdb'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 5
        rules: [
          {
            name: 'httpscalingrule'
            http: {
              metadata: {
                concurrentRequests: '20'
              }
            }
          }
        ]
      }
    }
  }
  tags: {
  }
  dependsOn: [
    sqlDB
  ]
}

resource ingestionContainerApp 'Microsoft.App/containerApps@2022-03-01' = if (deployIngestion) {
  name: 'podcastingestionca'
  location: location
  properties: {
    managedEnvironmentId: kubernetesEnvId
    configuration: {
      activeRevisionsMode: 'single'
      registries: [
        {
          server: acrLoginServer
          username: acrLogin
          passwordSecretRef: 'acr-password'
        }
      ]
      secrets: [
        {
          name: 'feedqueue'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=core.windows.net;AccountKey=${listKeys(storageAccount.id, '2019-06-01').keys[0].value}'
        }
        {
          name: 'podcastdb'
          value: podcastDbConnectionString
        }
        {
          name: 'acr-password'
          value: acrPassword
        }
      ]
    }
    template: {
      containers: [
        {
          image: podcastIngestionWorkerImage
          name: 'podcastingestion'
          resources: {
            cpu: 1
            memory: '2Gi'
          }
          env: [
            {
              name: 'ConnectionStrings__FeedQueue'
              secretRef: 'feedqueue'
            }
            {
              name: 'ConnectionStrings__PodcastDb'
              secretRef: 'podcastdb'
            }
          ]
        }
      ]
      scale: {
        maxReplicas: 5
        minReplicas: 0
        rules: [
          {
            name: 'queue-scaling-rule'
            azureQueue: {
              queueName: 'feed-queue'
              queueLength: 20
              auth: [
                {
                  secretRef: 'feedqueue'
                  triggerParameter: 'connection'
                }
              ]
            }
          }
        ]
      }
    }
  }
  dependsOn: [
    apiContainerApp
    sqlDB
  ]
}

resource updaterContainerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: 'podcastupdaterca'
  location: location
  properties: {
    managedEnvironmentId: kubernetesEnvId
    configuration: {
      activeRevisionsMode: 'single'
      registries: [
        {
          server: acrLoginServer
          username: acrLogin
          passwordSecretRef: 'acr-password'
        }
      ]
      secrets: [
        {
          name: 'podcastdb'
          value: podcastDbConnectionString
        }
        {
          name: 'acr-password'
          value: acrPassword
        }
      ]
    }
    template: {
      containers: [
        {
          image: podcastUpdaterImage
          name: 'podcastupdater'
          resources: {
            cpu: 1
            memory: '2Gi'
          }
          env: [
            {
              name: 'ConnectionStrings__PodcastDb'
              secretRef: 'podcastdb'
            }
            {
              name: 'Storage__Images'
              value: imagesStorage
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
  tags: {
  }
  dependsOn: [
    apiContainerApp
    storageAccount
  ]
}

output storageId string = storageAccount.id
