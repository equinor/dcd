param baseAppName string = 'dcd'

// NB: ae8e was taken from the deployed example, unsure how it came about
var serverfarmName = 'plan-${baseAppName}'

resource serverfarms 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: serverfarmName
  location: 'Norway East'
  sku: {
    name: 'P3v2'
    tier: 'PremiumV2'
    size: 'P3v2'
    family: 'Pv2'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: true
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

output serverfarmId string = serverfarms.id
