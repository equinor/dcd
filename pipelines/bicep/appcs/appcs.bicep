//
// az bicep publish --file common.bicep --target "br:crdcd.azurecr.io/bicep/common:v1"
//
param location string = resourceGroup().location
param baseAppName string = 'dcd'

var appConfigName = 'appcs-${baseAppName}'

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2021-03-01-preview' = {
  location: location
  name: appConfigName
  properties: {
    disableLocalAuth: false
    encryption: {}
  }
  sku: {
    name: 'standard'
  }
  identity: {
    type: 'SystemAssigned'
  }
}
