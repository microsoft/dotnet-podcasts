param name string
param location string = resourceGroup().location
param tags object = {}
param containerAppsEnvironmentName string
param containerRegistryName string
param imageName string = ''
param dbConnectionStringKey string
param keyVaultEndpoint  string
param applicationInsightsConnectionString string
param keyVaultName string

var serviceName = 'updater'

module app '../core/host/container-app.bicep' = {
  name: '${serviceName}-container-app-module'
  params: {
    name: name
    location: location
    tags: union(tags, { 'azd-service-name': serviceName })
    containerAppsEnvironmentName: containerAppsEnvironmentName
    containerRegistryName: containerRegistryName
    containerCpuCoreCount: '1.0'
    containerMemory: '2.0Gi'
    env: [
      {
        name: 'AZURE_API_SQL_CONNECTION_STRING_KEY'
        value: dbConnectionStringKey
      }
      {
        name: 'AZURE_KEY_VAULT_ENDPOINT'
        value: keyVaultEndpoint
      }
      {
        name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
        value: applicationInsightsConnectionString
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
    ]
    imageName: !empty(imageName) ? imageName : 'nginx:latest'
    keyVaultName: keyVault.name
  }
}

module keyVaultAccess '../core/security/keyvault-access.bicep' = {
  name: '${serviceName}-keyvault-access'
  params: {
    keyVaultName: keyVaultName
    principalId: app.outputs.identityPrincipalId
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

output SERVICE_UPDATER_IDENTITY_PRINCIPAL_ID string = app.outputs.identityPrincipalId
output SERVICE_UPDATER_NAME string = app.outputs.name
output SERVICE_UPDATER_URI string = app.outputs.uri
output SERVICE_UPDATER_IMAGE_NAME string = app.outputs.imageName
