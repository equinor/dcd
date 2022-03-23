//
// az bicep publish --file common.bicep --target "br:crdcd.azurecr.io/bicep/common:v1"
//
param location string = resourceGroup().location
param baseAppName string = 'dcd'

var appConfigName = 'appcs-${baseAppName}'

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2021-10-01-preview' = {
  location: location
  name: appConfigName
  properties: {
    disableLocalAuth: false
    encryption: {}
    softDeleteRetentionInDays: 7
    enablePurgeProtection: false
  }
  sku: {
    name: 'standard'
  }
  identity: {
    type: 'SystemAssigned'
  }
}



output id string = appConfig.id
output apiVersion string = appConfig.apiVersion
output name string = appConfig.name
output resource object = appConfig
