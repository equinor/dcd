param location string
param appConfigName string
param preprod bool

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2024-05-01' = {
    location: location
    name: appConfigName
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

resource appConfigTentantId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'AzureAd:TenantId'
  properties: {
    value: '3aa4a235-b6e2-48d5-9195-7fcf05b459b0'
  }
}

resource appConfigInstanceId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
    parent: appConfig
    name: 'AzureAd:Instance'
    properties: {
      value: 'https://login.microsoftonline.com'
    }
  }

  resource appConfigClientId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
    parent: appConfig
    name: 'AzureAd:ClientId'
    properties: {
      value: preprod ? '151950a5-f886-47cd-b361-afb81e75c345' : '81bd7b7f-4096-4c4f-b0c2-ebef7d05c0e6'
    }
  }
