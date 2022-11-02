@description('Web app name.')
@minLength(2)
@maxLength(60)
param name string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('The SKU of App Service Plan.')
param sku string = 'B1'

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|6.0'

// trim whitespace, replace spaces and underscores with hyphens
var nameClean = replace(replace(toLower(trim(name)), ' ', '-'), '_', '-')

// req: (2-40)
var servicePlanName = length(nameClean) <= 40 ? nameClean : take(nameClean, 40)

resource servicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: servicePlanName
  location: location
  sku: {
    name: sku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2020-06-01' = {
  name: nameClean
  location: location
  properties: {
    serverFarmId: servicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      alwaysOn: true
      http20Enabled: true
    }
    httpsOnly: true
    clientAffinityEnabled: false
  }
}
