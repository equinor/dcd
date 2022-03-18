//
// az bicep publish --file common.bicep --target "br:crdcd.azurecr.io/bicep/common:v1"
//
param location string = resourceGroup().location
param baseAppName string = 'dcd'

var appServicePlan = 'plan-${baseAppName}'

resource serverfarms_dcd 'Microsoft.Web/serverfarms@2021-02-01' = {
  kind: 'linux'
  location: location
  name: appServicePlan
  properties: {
    elasticScaleEnabled: false
    hyperV: false
    isSpot: false
    isXenon: false
    maximumElasticWorkerCount: 1
    perSiteScaling: false
    reserved: true
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
  sku: {
    capacity: 1
    family: 'Pv2'
    name: 'P1v2'
    size: 'P1v2'
    tier: 'PremiumV2'
  }
}

output serverFarmsId string = serverfarms_dcd.id
