@allowed(['ci', 'qa', 'prod'])
param environmentName string = 'ci'
param location string = 'norwayeast'

var preprod = environmentName == 'ci' || environmentName == 'qa'

var roughtechsEntraGroupObjectId = 'a64069dd-12fd-422b-8c1e-2093fa32819d'

var commonTags = {
  'iac': 'bicep'
  'pipeline': 'azurePipelines'
  'projectName': 'dcd'
  'environment': environmentName
  'responsible': 'roughtechs'
  'area': 'fapp'
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: preprod ? 'kvfappdcdpreprod' : 'kvfappdcdfprd'
  location: location
  properties: {
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: roughtechsEntraGroupObjectId
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
  name: preprod ? 'stdcdpreprod' : 'stdcdfprd'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

var storageAccountKey = storageAccount.listKeys().keys[0].value

var connectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccountKey};EndpointSuffix=core.windows.net'

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
    preprod: preprod
  }
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' = if (!preprod) {
  name: 'crfappdcd'
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
    keyVaultName: keyVault.name
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
