@description('Web app name.')
@minLength(2)
@maxLength(60)
param name string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('The SKU of App Service Plan.')
param sku string = 'S1'

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|6.0'

// param podcastApiUrl string

@secure()
param administratorLoginPassword string
param administratorLogin string

// @secure()
// param storageAccountKey string
param storageAccountName string

var sqlDBName = 'ListenTogether'

// trim whitespace, replace spaces and underscores with hyphens
var nameClean = replace(replace(toLower(trim(name)), ' ', '-'), '_', '-')

// req: (3-64) Lowercase letters, hyphens and numbers. Globally unique.
// trim whitespace, replace spaces and underscores with hyphens
// var serverName = replace(nameClean, '_', '-')

// req: (2-40)
var servicePlanName = length(nameClean) <= 40 ? nameClean : take(nameClean, 40)

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' existing = {
  name: storageAccountName
}

resource apiContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' existing = {
  name: 'podcastapica'
}

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  // req: (3-64) Lowercase letters, hyphens and numbers. Globally unique.
  name: nameClean
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

resource sqlFirewallRule 'Microsoft.Sql/servers/firewallRules@2021-11-01' = if (true) {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

resource servicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: servicePlanName
  location: location
  sku: { name: sku }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2020-06-01' = {
  name: name
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
          // value: podcastApiUrl
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

resource webAppConnectionString 'Microsoft.Web/sites/config@2020-12-01' = {
  parent: webApp
  name: 'connectionstrings'
  properties: {
    ListenTogetherDb: {
      value: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDB.name};Persist Security Info=False;User ID=${sqlServer.properties.administratorLogin};Password=${administratorLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
      type: 'SQLAzure'
    }
    OrleansStorage: {
      // value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccountKey}'
      value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
      type: 'Custom'
    }
  }
}
