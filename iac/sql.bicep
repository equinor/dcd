param location string
@allowed(['ci', 'qa', 'prod'])
param environmentName string
param preprod bool
param keyVaultName string

@secure()
param databasePasswordSeed string = newGuid()

var sqlAdminPassword = base64(uniqueString(resourceGroup().id, databasePasswordSeed))

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

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: preprod ? 'sql-fapp-dcd-preprod' : 'sql-fapp-dcd-fprd'
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/sqlAdminPassword)'
  }
}

/*
resource sqlAdminPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'sqlAdminPassword'
  properties: {
    value: sqlAdminPassword
  }
}

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: preprod ? 'dcdsql-preprod' : 'dcdsql-prod'
  location: location
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: sqlAdminPassword
  }
}
*/

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  name: 'dcdsql-db-${environmentName}'
  location: location
  parent: sqlServer
  sku: sqlServerSkuMap[environmentName]
}
