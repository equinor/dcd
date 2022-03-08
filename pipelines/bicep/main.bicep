//var tenantId = subscription().tenantId
var location = 'norwayeast'

module vnetModule 'vnet/vnet.bicep' = {
  name: 'vnet'
  params:{
    baseAppName: 'dcd'
    location: location
  }
}
