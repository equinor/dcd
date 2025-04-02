@allowed(['ci', 'qa', 'prod'])
param environmentName string
param location string = 'norwayeast'
@secure()
param sqlAdminPassword string
@secure()
param clientSecret string


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

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2024-05-01' = {
  location: location
  name: preprod ? 'appcs-fapp-dcd-preprod2' : 'appcs-fapp-dcd-fprd'
  sku: {
    name: 'standard'
  }
  properties: {
    disableLocalAuth: false
    encryption: {}
    softDeleteRetentionInDays: 7
    enablePurgeProtection: false
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: preprod ? 'kvfappdcdpreprod' : 'kvfappdcdfprd'
  location: location
  properties: {
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: appConfig.identity.principalId
        permissions: {
          keys: []
          secrets: ['get', 'list']
          certificates: []
        }
      }
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
    enabledForTemplateDeployment: true
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: 'stdcd${environmentName}'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
    parent: storageAccount
    name: 'default'
  }

resource storageContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2024-01-01' = {
    parent: blobService
    name: 'image-storage'
    properties: {
      publicAccess: 'Container'
    }
  }

var storageAccountKey = storageAccount.listKeys().keys[0].value

var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccountKey};EndpointSuffix=core.windows.net'

resource keyVaultSecretStorageAccount 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'storage-account-connection-string'
  properties: {
    value: storageAccountConnectionString
  }
}

resource keyVaultSecretClientSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
    parent: keyVault
    name: preprod ? 'dcd-backend-client-secret-fusion-preprod' : 'dcd-backend-client-secret-fusion-prod'
    properties: {
      value: clientSecret
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
    sqlAdminPassword: sqlAdminPassword
  }
}

module appconfig './appconfig.bicep' = {
  name: 'appconfig'
  params: {
    location: location
    environmentName: environmentName
    preprod: preprod
    keyVaultName: keyVault.name
    appConfigName: appConfig.name
  }
}
