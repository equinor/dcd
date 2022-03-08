var tenantId = subscription().tenantId
var keyVaultAccessPolicies = [
  {
    tenantId: tenantId
    // appcs-dcd system-assigned managed identity
    objectId: appConfigDeploy.outputs.resource.identity.principalId
    permissions: {
      keys: [
        'get'
        'list'
      ]
      secrets: [
        'get'
        'list'
      ]
      certificates: [
        'get'
        'list'
      ]
    }
  }
  {
    tenantId: tenantId
    // no way to access with code as app registrations are AD objects!?
    objectId: '79a59be8-c151-48f2-8efe-cf82beb54d15' // App DCD
    permissions: {
      keys: [
        'get'
        'list'
      ]
      secrets: [
        'get'
        'list'
      ]
      certificates: []
    }
  }
  {
    tenantId: tenantId
    objectId: '0eff847e-abd4-4451-99e0-2cab565618f8' // App Shellvis-DCD-...
    permissions: {
      keys: [
        'get'
        'list'
      ]
      secrets: [
        'get'
        'list'
      ]
      certificates: [
        'get'
        'list'
      ]
    }
  }
]

module appConfigDeploy 'appcs/appcs.bicep' = {
  name: 'appConfig'
  params: {
    baseAppName: 'dcd-test'
  }
}

module keyVault 'kv/kv.bicep' = {
  name: 'keyVault'
  params: {
    tenantId: tenantId
    accessPolicies: keyVaultAccessPolicies
    baseAppName: 'dcd-test-2'
    appConfigConnectionString: listKeys(resourceId('Microsoft.AppConfiguration/configurationStores', appConfigDeploy.outputs.name), appConfigDeploy.outputs.apiVersion).value[0].connectionString
  }
}
