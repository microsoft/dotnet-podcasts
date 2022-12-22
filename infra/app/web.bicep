@description('Web app name.')
@minLength(2)
param name string
@description('Location for all resources.')
param location string = resourceGroup().location
param tags object = {}
param containerAppsEnvironmentName string
param containerRegistryName string
param imageName string
param apiBaseUrl string
param listenTogetherHubUrl string
param keyVaultName string


var serviceName = 'web'

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
        name: 'REACT_APP_API_BASE_URL'
        value: apiBaseUrl
      }
      {
        name: 'REACT_LISTEN_TOGETHER_HUB'
        value: listenTogetherHubUrl
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

output SERVICE_WEB_IDENTITY_PRINCIPAL_ID string = app.outputs.identityPrincipalId
output SERVICE_WEB_NAME string = app.outputs.name
output SERVICE_WEB_URI string = app.outputs.uri
output SERVICE_WEB_IMAGE_NAME string = app.outputs.imageName
