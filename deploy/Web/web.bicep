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

@description('The name of the API container app.')
param apiName string

@description('The name of the Hub Web App.')
param hubWebAppName string = ''

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

// Reference existing API Container App to set App Settings
resource apiContainerApp 'Microsoft.App/containerApps@2022-03-01' existing = {
  name: apiName
  scope: resourceGroup()
}

// Reference existing Hub Container App to set App Settings
resource hubWebApp 'Microsoft.Web/sites@2022-03-01' existing = {
  name: hubWebAppName
  scope: resourceGroup()
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
      appSettings: [
        {
          name: 'PodcastApi__BaseAddress'
          value: 'https://${apiContainerApp.properties.configuration.ingress.fqdn}'
        }
        {
          name: 'ListenTogetherHub'
          value: 'https://${hubWebApp.properties.hostNames[0]}/listentogether'
        }
      ]
    }
    httpsOnly: true
    clientAffinityEnabled: false
    
  }
}
