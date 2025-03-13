param environmentName string
param preprod bool
param location string = 'norwayeast'

var keyVaultName = preprod ? 'dcdkeyvault-preprod' : 'dcdkeyvault-prod'
var storageAccountName = 'dcdstorage${environmentName}'
var databaseName = 'dcddb${environmentName}'
var roughtechsObjectId = 'a64069dd-12fd-422b-8c1e-2093fa32819d'


resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    accessPolicies: [
        {
            tenantId: subscription().tenantId
            objectId: roughtechsObjectId
            permissions: {
                keys: []
                secrets: ['all']
                certificates: []
            }
        }
    ]
    sku: {
      family: 'A'
      name: 'standard'
    }
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

var storageAccountKeys = storageAccount.listKeys()

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'storageAccountKey'
  properties: {
    value: storageAccountKeys.keys[0].value
  }
}
