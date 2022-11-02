@description('Name of the azure container registry (must be globally unique)')
@minLength(5)
@maxLength(50)
param name string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Enable an admin user that has push/pull permission to the registry.')
param acrAdminUserEnabled bool = true

@description('Tier of your Azure Container Registry.')
@allowed([ 'Basic', 'Standard', 'Premium' ])
param acrSku string = 'Basic'

// trim whitespace, remove spaces, hyphens and underscores
var nameClean = replace(replace(replace(toLower(trim(name)), ' ', ''), '-', ''), '_', '')

resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: nameClean
  location: location
  tags: {
    displayName: 'Container Registry'
    'container.registry': nameClean
  }
  sku: {
    name: acrSku
  }
  properties: {
    adminUserEnabled: acrAdminUserEnabled
  }
}

output acrLoginServer string = acr.properties.loginServer
