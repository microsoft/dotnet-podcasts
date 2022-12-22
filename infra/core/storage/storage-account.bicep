param name string
param location string = resourceGroup().location
param tags object = {}

param allowBlobPublicAccess bool = false
param kind string = 'StorageV2'
param minimumTlsVersion string = 'TLS1_2'
param sku object = { name: 'Standard_LRS' }

param keyVaultName string

resource storage 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: name
  location: location
  tags: tags
  kind: kind
  sku: sku
  properties: {
    minimumTlsVersion: minimumTlsVersion
    allowBlobPublicAccess: allowBlobPublicAccess
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
  }
}

resource feedQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  name: '${storage.name}/default/feed-queue'
}

resource feedQueueSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault
  name: 'FEED-QUEUE'
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageName};EndpointSuffix=core.windows.net;AccountKey=${storagePrimaryKey}'
  }
}

resource orleansStorageSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault
  name: 'ORLEANS-STORAGE'
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageName};AccountKey=${storagePrimaryKey}'
  }
}

resource storagePrimaryKeySecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault
  name: 'STORAGE-PRIMARY-KEY'
  properties: {
    value: storagePrimaryKey
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

var storagePrimaryKey = storage.listKeys().keys[0].value
var storageName = storage.name

output name string = storage.name
output id string = storage.id
output primaryEndpoints object = storage.properties.primaryEndpoints
output feedQueueConnectionStringKey string = 'FEED-QUEUE'
output orleansStorageConnectionStringKey string = 'ORLEANS-STORAGE'
output storagePrimaryKeyConnectionStringKey string = 'STORAGE-PRIMARY-KEY'
