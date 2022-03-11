//var tenantId = subscription().tenantId
var location = resourceGroup().location
var baseAppName = 'dcdtest'
module vnetModule 'vnet/vnet.bicep' = {
  name: 'vnet'
  params:{
    baseAppName: baseAppName
    location: location
  }
}

module WebApps 'webapp/main.bicep' = {
  name: 'webApps'
  params: {
    baseAppName: baseAppName
    appInsightInstrumatationKey: appInsights.outputs.appInsightKey
  }
}

module appInsights 'appi/appi.bicep' = {
  name: 'appInsight'
  params: {
    baseAppName: baseAppName
    location: location
    
  }
}

module sql 'sql/sql.bicep' = {
  name: 'SqlEnv'
  params: {
    baseAppName: baseAppName
    location: location
    sqlAdminPw: sqlAdminPw
  }
}
