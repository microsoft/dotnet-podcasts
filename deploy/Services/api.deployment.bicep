@description('Location for all resources.')
param location string = resourceGroup().location
param imageTag string

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

@secure()
param administratorLoginPassword string

var kubernetesEnvLocation = 'canadacentral'
var podcastDbConnectionString = 'Server=tcp:${serverName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDBName};Persist Security Info=False;User ID=${administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
var podcastApiImage = '${acrLoginServer}/podcastapi:${imageTag}'
var podcastUpdaterImage = '${acrLoginServer}/podcastupdaterworker:${imageTag}'
var podcastIngestionWorkerImage = '${acrLoginServer}/podcastingestionworker:${imageTag}'
var imagesStorage = 'https://${storageAccountName}.blob.${environment().suffixes.storage}/covers/'
var storageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', storageAccountName), '2019-06-01').keys[0].value}'
output storageConnectionString string = storageConnectionString
var deployIngestion = false

resource serverName_resource 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource sqlDB 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {
  parent: serverName_resource
  name: sqlDBName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

resource serverName_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2014-04-01-preview' = if (true) {
  parent: serverName_resource
  name: 'AllowAllWindowsAzureIps'
  location: location
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

resource kubernetesEnv 'Microsoft.App/managedEnvironments@2022-01-01-preview' = {
  name: kubernetesEnvName
  location: kubernetesEnvLocation
  tags: {}
  properties: {
    type: 'Managed'
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: workspace.properties.customerId
        sharedKey: workspace.listKeys().primarySharedKey
      }
    }
  }
}

resource podcastapica 'Microsoft.App/containerApps@2022-01-01-preview' = {
  name: 'podcastapica'
  location: kubernetesEnvLocation
  kind: 'containerapp'
  properties: {
    managedEnvironmentId: kubernetesEnv.id
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
          value: storageConnectionString
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
            {
              name: 'Features__FeedIngestion'
              value: '${deployIngestion}'
            }
          ]
        }
      ]
      dapr: {
        enabled: false
      }
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
  tags: {}
  dependsOn: [
    // kubernetesEnv
    sqlDB
    storageAccount
  ]
}

resource podcastingestorca 'Microsoft.App/containerApps@2022-06-01-preview' = if (deployIngestion) {
  name: 'podcastingestionca'
  location: kubernetesEnvLocation
  kind: 'containerapp'
  properties: {
    managedEnvironmentId: kubernetesEnv.id
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
          value: storageConnectionString
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
      dapr: {
        enabled: false
      }
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
  tags: {}
  dependsOn: [
    // kubernetesEnv
    sqlDB
    storageAccount
    podcastapica
  ]
}

resource podcastupdaterca 'Microsoft.App/containerApps@2022-01-01-preview' = {
  name: 'podcastupdaterca'
  location: kubernetesEnvLocation
  kind: 'containerapp'
  properties: {
    // managedEnvironmentId: kubernetesEnvId
    managedEnvironmentId: kubernetesEnv.id
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
      dapr: {
        enabled: false
      }
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
  tags: {}
  dependsOn: [
    // kubernetesEnv
    storageAccount
    podcastapica
  ]
}
