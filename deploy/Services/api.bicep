@minLength(2)
@maxLength(60)
param name string

@description('Location for all resources.')
param location string = resourceGroup().location

param imageTag string

param acrName string

// @secure()
// param acrPassword string
// param acrLogin string
// param acrLoginServer string

@secure()
param administratorLoginPassword string
param administratorLogin string

var podcastApiImage = '${acr.properties.loginServer}/podcastapi:${imageTag}'
var podcastUpdaterImage = '${acr.properties.loginServer}/podcastupdaterworker:${imageTag}'
var podcastIngestionWorkerImage = '${acr.properties.loginServer}/podcastingestionworker:${imageTag}'

var sqlDBName = 'Podcast'
var imagesContainer = 'covers'
var storageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
var podcastDbConnectionString = 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDB.name};Persist Security Info=False;User ID=${sqlServer.properties.administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

var deployIngestion = false

// trim whitespace, replace spaces and underscores with hyphens
var nameClean = replace(replace(toLower(trim(name)), ' ', '-'), '_', '-')
var nameCleaner = replace(nameClean, '-', '')
var storageAccountName = length(nameCleaner) <= 24 ? nameCleaner : take(nameCleaner, 24)

// Common properties for all containers
var resources = { cpu: 1, memory: '2Gi' }
var registries = [
  { server: acr.properties.loginServer, username: acr.listCredentials().username, passwordSecretRef: 'acr-password' }
]

resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' existing = {
  name: acrName
  // name: nameCleaner
}

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  // (3-64) Lowercase letters, hyphens and numbers. Globally unique.
  name: nameClean
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource sqlDB 'Microsoft.Sql/servers/databases@2021-11-01' = {
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

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
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

resource feedQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  name: '${storageAccount.name}/default/feed-queue'
}

resource workspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  // (3-63) Alphanumerics and hyphens.
  name: nameCleaner
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

resource kubernetesEnv 'Microsoft.App/managedEnvironments@2022-06-01-preview' = {
  name: nameClean
  location: location
  sku: { name: 'Consumption' }
  properties: {
    // type: 'Managed'
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: workspace.properties.customerId
        sharedKey: workspace.listKeys().primarySharedKey
      }
    }
  }
  tags: {}
}

resource apiContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  // (2-32) Lowercase letters, numbers, and hyphens
  name: 'podcastapica'
  location: location
  // kind: 'containerapp'
  properties: {
    environmentId: kubernetesEnv.id
    // managedEnvironmentId: kubernetesEnv.id
    configuration: {
      activeRevisionsMode: 'single'
      registries: registries
      secrets: [
        { name: 'acr-password', value: acr.listCredentials().passwords[0].value }
        { name: 'podcastdb', value: podcastDbConnectionString }
        { name: 'feedqueue', value: storageConnectionString }
      ]
      dapr: { enabled: false }
      ingress: {
        external: true
        targetPort: 80
      }
    }
    template: {
      containers: [
        {
          name: 'podcastapi'
          image: podcastApiImage
          resources: resources
          env: [
            { name: 'ConnectionStrings__PodcastDb', secretRef: 'podcastdb' }
            { name: 'ConnectionStrings__FeedQueue', secretRef: 'feedqueue' }
            { name: 'Features__FeedIngestion', value: '${deployIngestion}' }
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
  tags: {}
}

resource ingestionContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = if (deployIngestion) {
  // (2-32) Lowercase letters, numbers, and hyphens
  name: 'podcastingestionca'
  location: location
  // kind: 'containerapp'
  properties: {
    environmentId: kubernetesEnv.id
    // managedEnvironmentId: kubernetesEnv.id
    configuration: {
      activeRevisionsMode: 'single'
      registries: registries
      secrets: [
        { name: 'acr-password', value: acr.listCredentials().passwords[0].value }
        { name: 'podcastdb', value: podcastDbConnectionString }
        { name: 'feedqueue', value: storageConnectionString }
      ]
      dapr: { enabled: false }
    }
    template: {
      containers: [
        {
          name: 'podcastingestion'
          image: podcastIngestionWorkerImage
          resources: resources
          env: [
            { name: 'ConnectionStrings__PodcastDb', secretRef: 'podcastdb' }
            { name: 'ConnectionStrings__FeedQueue', secretRef: 'feedqueue' }
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
  ]
  tags: {}
}

resource updaterContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  // (2-32) Lowercase letters, numbers, and hyphens
  name: 'podcastupdaterca'
  location: location
  // kind: 'containerapp'
  properties: {
    environmentId: kubernetesEnv.id
    // managedEnvironmentId: kubernetesEnv.id
    configuration: {
      activeRevisionsMode: 'single'
      registries: registries
      secrets: [
        { name: 'acr-password', value: acr.listCredentials().passwords[0].value }
        { name: 'podcastdb', value: podcastDbConnectionString }
      ]
      dapr: { enabled: false }
    }
    template: {
      containers: [
        {
          name: 'podcastupdater'
          image: podcastUpdaterImage
          resources: resources
          env: [
            { name: 'ConnectionStrings__PodcastDb', secretRef: 'podcastdb' }
            { name: 'Storage__Images', value: '${storageAccount.properties.primaryEndpoints.blob}${imagesContainer}/' }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
  dependsOn: [
    apiContainerApp
  ]
  tags: {}
}

// output storageAccountName string = storageAccount.name

// output storageConnectionString string = storageConnectionString
