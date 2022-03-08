//
// az bicep publish --file common.bicep --target "br:crdcd.azurecr.io/bicep/common:v1"
//
param location string = resourceGroup().location
param baseAppName string = 'dcd'
param tenantId string = subscription().tenantId
param accessPolicies array
param appConfigConnectionString string

var keyvaultName = 'kv-${baseAppName}'

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyvaultName
  location: location
  properties: {
    enableRbacAuthorization: false
    //enableSoftDelete: true
    //enablePurgeProtection: true
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    // what-if in my opinion incorrectly claims that uncommenting the
    // publicNetworkAccess line will change that property on the keyVault
    publicNetworkAccess: 'Enabled'
    tenantId: tenantId
    softDeleteRetentionInDays: 90
    accessPolicies: accessPolicies
    sku: {
      family: 'A'
      name: 'standard'
    }
  }
}

resource app_config_connection_string 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'app-config-connection-string'
  location: 'norwayeast'
  properties: {
    attributes: {
      enabled: true
      value: appConfigConnectionString
    }
  }
}
/*
resource DbConnectionStringDev 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'DbConnectionStringDev'
  location: 'norwayeast'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource DbConnectionStringProd 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'DbConnectionStringProd'
  location: location
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource DbConnectionStringQa 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'DbConnectionStringQa'
  location: location
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource DCDAppRegistrationClientSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'DCDAppRegistrationClientSecret'
  location: location
  properties: {
    attributes: {
      enabled: true
      exp: 1660978800
    }
  }
}

resource dynatrace_preprod_token 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'dynatrace-preprod-token'
  location: location
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource dynatrace_prod_token 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'dynatrace-prod-token'
  location: location
  properties: {
    attributes: {
      enabled: true
    }
  }
}
*/

output kv object = keyVault
