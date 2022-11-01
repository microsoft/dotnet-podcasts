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
param linuxFxVersion string = 'DOTNETCORE|6.0'
param podcastApiUrl string
param serverName string
param sqlDBName string = 'ListenTogether'
param administratorLogin string

@secure()
param administratorLoginPassword string

@secure()
param storageAccountKey string
param storageAccountName string

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
          value: podcastApiUrl
        }
      ]
    }
    httpsOnly: true
    clientAffinityEnabled: false
  }
  dependsOn: [
    serverName_sqlDB
  ]
}

resource webAppName_connectionstrings 'Microsoft.Web/sites/config@2020-12-01' = {
  parent: webApp
  name: 'connectionstrings'
  properties: {
    ListenTogetherDb: {
      value: 'Server=tcp:${serverName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlDBName};Persist Security Info=False;User ID=${administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
      type: 'SQLAzure'
    }
    OrleansStorage: {
      value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccountKey}'
      type: 'Custom'
    }
  }
  dependsOn: [
    server
  ]
}

resource server 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
}

resource serverName_sqlDB 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {
  parent: server
  name: sqlDBName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

resource serverName_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2014-04-01-preview' = {
  parent: server
  name: 'AllowAllWindowsAzureIps'
  location: location
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
