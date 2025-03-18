param environmentName string = 'ci'
param preprod bool = true
param location string = 'norwayeast'

var keyVaultName = preprod ? 'dcdkeyvault-preprod' : 'dcdkeyvault-prod'
var storageAccountName = 'dcdstorage${environmentName}'
var appConfigName = preprod ? 'dcdappconfig-preprod' : 'dcdappconfig-prod'
var roughtechsObjectId = 'a64069dd-12fd-422b-8c1e-2093fa32819d'
var commonTags = {
  'environment': environmentName
}

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

var connectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccountKeys.keys[0].value};EndpointSuffix=core.windows.net'

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'storageAccountConnectionString'
  properties: {
    value: connectionString
  }
}

module appconfig './appconfig.bicep' = {
  name: 'appconfig'
  params: {
    location: location
    appConfigName: appConfigName
    preprod: preprod
  }
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' = if (!preprod) {
  name: 'dcdcr'
  location: location
  sku: {
    name: 'Standard'
  }
}

module sql './sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    preprod: preprod
    keyVaultName: keyVaultName
    environmentName: environmentName
  }
}




/*
// TODO: Fix this reference
resource appConfigKeyVaultReference 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
    parent: appConfig
    name: 'AppSettings:StorageAccountConnectionString'
    properties: {
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVault.properties.vaultUri}secrets/storageAccountConnectionString)'
    }
  }

resource appConfigItem3 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
    parent: appConfig
    name: 'AppSettings:Setting3'
    properties: {
      value: 'Value3'
    }
  }

*/
