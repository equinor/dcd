param location string
@allowed(['ci', 'qa', 'prod'])
param environmentName string
param preprod bool
param keyVaultName string
@secure()
param sqlAdminPassword string

var sqlServerSkuMap = {
  ci: {
    name: 'S2'
    tier: 'Standard'
    capacity: 50
  }
  qa: {
    name: 'S3'
    tier: 'Standard'
    capacity: 100
  }
  prod: {
    name: 'S4'
    tier: 'Standard'
    capacity: 200
  }
}

var sqlDatabaseName = 'sqldb-fapp-dcd-${environmentName}'
var sqlServerName = preprod ? 'sql-fapp-dcd-preprod' : 'sql-fapp-dcd-fprd'

var sqlConnectionString = 'Server=tcp:${sqlServerName}.database.windows.net,1433;Initial Catalog=${sqlDatabaseName};Persist Security Info=False;User ID=sqladmin;Password=${sqlAdminPassword};MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'sql-connection-string-${environmentName}'
  properties: {
    value: sqlConnectionString
  }
}

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: sqlAdminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  name: sqlDatabaseName
  location: location
  parent: sqlServer
  sku: sqlServerSkuMap[environmentName]
}
