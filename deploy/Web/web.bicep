@description('Web app name.')
@minLength(2)
param webAppName string

@description('Service plan name.')
@minLength(2)
param servicePlanName string

@description('The SKU of App Service Plan.')
param servicePlanSku string = 'B1'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|7.0'

resource servicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: servicePlanName
  location: location
  sku: {
    name: servicePlanSku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppName
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

resource webAppSlot 'Microsoft.Web/sites/slots@2022-03-01' = {
  parent: webApp
  name: 'staging'
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
