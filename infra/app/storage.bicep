param name string
param location string = resourceGroup().location
param allowBlobPublicAccess bool = true
param keyVaultName string
param tags object = {}

var serviceName = 'storage'

module storage '../core/storage/storage-account.bicep' = {
  name: '${name}-storage-module'
  params: {
    name: name
    location: location
    tags: union(tags, { 'azd-service-name': serviceName })
    allowBlobPublicAccess: allowBlobPublicAccess
    keyVaultName: keyVaultName
  }
}

output name string = storage.outputs.name
output id string = storage.outputs.id
output primaryEndpoints object = storage.outputs.primaryEndpoints
output storageImages string = 'https://${storage.outputs.name}.blob.${environment().suffixes.storage}/covers/'
output feedQueueConnectionStringKey string = storage.outputs.feedQueueConnectionStringKey
output orleansStorageConnectionStringKey string = storage.outputs.orleansStorageConnectionStringKey
output storagePrimaryKeyConnectionStringKey string = storage.outputs.storagePrimaryKeyConnectionStringKey
