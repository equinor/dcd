param location string
param preprod bool
param keyVaultName string
param environmentName string
@secure()
param databasePasswordSeed string = newGuid()

var sqlAdminPassword = base64(uniqueString(resourceGroup().id, databasePasswordSeed))

var ciSku = {
  name: 'S2'
  tier: 'Standard'
  capacity: 50
}

var qaSku = {
  name: 'S3'
  tier: 'Standard'
  capacity: 100
}

var prodSku = {
  name: 'S4'
  tier: 'Standard'
  capacity: 200
}

// Determine the SKU based on the environment name
var selectedSku = environmentName == 'ci' ? ciSku : (environmentName == 'qa' ? qaSku : prodSku)

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
    name: keyVaultName
  }

  resource generatePassword 'Microsoft.Resources/deploymentScripts@2020-10-01' = {
    name: 'generateSqlPassword'
    location: location
    kind: 'AzurePowerShell'
    properties: {
      azPowerShellVersion: '7.2'
      scriptContent: '''
        $Password = [Guid]::NewGuid().ToString("N").Substring(0, 16)
        az keyvault secret set --vault-name "${keyVaultName}" --name "sqlAdminPassword" --value $Password
      '''
      timeout: 'PT5M'
      retentionInterval: 'P1D'
    }
    dependsOn: [keyVault]
  }

  resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
    name: preprod ? 'dcdsql-preprod' : 'dcdsql-prod'
    location: location
    properties: {
        administratorLogin: 'sqladmin'
        administratorLoginPassword: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/sqlAdminPassword)'
    }
    dependsOn: [generatePassword]
  }

/*
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

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
  sku: selectedSku
}
