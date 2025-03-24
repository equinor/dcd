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

resource generatePassword 'Microsoft.Resources/deploymentScripts@2020-10-01' = {
    name: 'generateSqlPassword'
    location: location
    kind: 'AzurePowerShell'
    properties: {
      azPowerShellVersion: '7.2'
      scriptContent: '''
        Write-Host "Key Vault Name: ${keyVaultName}"

        # Generate a secure password
        $Password = (-join ((33..126) | Get-Random -Count 16 | ForEach-Object { [char]$_ }))

        # Authenticate with Azure and set the secret in Key Vault
        Set-AzKeyVaultSecret -VaultName "${keyVaultName}" -Name "sqlAdminPassword" -SecretValue (ConvertTo-SecureString -String $Password -AsPlainText -Force)
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
