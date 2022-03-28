//
// az bicep publish --file common.bicep --target "br:crdcd.azurecr.io/bicep/common:v1"
//
param location string = resourceGroup().location
param baseAppName string = 'dcd'
param tenantId string = subscription().tenantId
param accessPolicies array

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

output kv object = keyVault
