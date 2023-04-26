param name string
@description('Location for all resources.')
param location string = resourceGroup().location
param tags object = {}
param keyValutName string
param databaseName string
param connectionStringKey string
@secure()
param sqlAdminPassword string
@secure()
param appUserPassword string

module sql '../core/database/sqlserver/sqlserver.bicep' = {
  name: '${name}-sql-module'
  params: {
    name: name
    location: location
    databaseName: databaseName
    keyVaultName: keyValutName
    connectionStringKey: connectionStringKey
    sqlAdminPassword: sqlAdminPassword
    appUserPassword: appUserPassword
    tags: tags
  }
}

output dbConnectionStringKey string = sql.outputs.connectionStringKey
