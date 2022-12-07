@description('Web app name.')
@minLength(2)
param webAppName string

@description('Service plan name.')
@minLength(2)
param servicePlanName string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('The SKU of App Service Plan.')
param sku string = 'S1'

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|7.0'
param serverName string
param sqlDBName string = 'ListenTogether'
param administratorLogin string

@description('The name of the API container app.')
param apiName string = 'podcastapica'

@secure()
param administratorLoginPassword string

param storageAccountName string

resource servicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
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

// Reference storage account to set keys in app settings
resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
  scope:  resourceGroup()
}

// Reference existing API Container App to set App Settings
resource apiContainerApp 'Microsoft.App/containerApps@2022-03-01' existing = {
  name: apiName
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
          name: 'NetPodcastApi__BaseAddress'
          value: 'https://${apiContainerApp.properties.configuration.ingress.fqdn}'
        }
      ]
    }
    httpsOnly: true
    clientAffinityEnabled: false
  }
  dependsOn: [
    sqlDB
  ]
}

resource webAppConnectionString 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: webApp
  name: 'connectionstrings'
  properties: {
    ListenTogetherDb: {
      value: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDB.name};Persist Security Info=False;User ID=${administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
      type: 'SQLAzure'
    }
    OrleansStorage: {
      value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
      type: 'Custom'
    }
  }
}

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: serverName
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

resource sqlServerFirewallRule 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
