//var tenantId = subscription().tenantId
var location = resourceGroup().location
var baseAppName = 'dcdapp'

var sqlAdminPw = 'QX8dVpaJpwaz6hPK'

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

module appcs 'appcs/appcs.bicep' = {
  name: 'appcs'
  params: {
    baseAppName: baseAppName
    location: location
  }
}


module acr 'acr/acr.bicep' = {
  name: 'cr'
  params: {
    baseAppName: baseAppName
    location: location
  }
}

// module kv 'kv/kv.bicep' = {
//   name: 'kv'
//   params: {
//     baseAppName: baseAppName
//     location: location

//   }
// }
