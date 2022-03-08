//
// az bicep publish --file webapp.bicep --target "br:crdcd.azurecr.io/bicep/webapp:v1"
//
param environment string = 'dev'
param azureLocation string = resourceGroup().location
param baseAppName string = 'dcd'
param serverfarmId string
// frontend or backend ?
param end string

var webAppName = 'ase-${baseAppName}-${end}-${environment}'
param containerRegistry string = 'crdcd.azurecr.io'
param dockerImageName string = end
param dockerImageVersion string = 'latest'

var dockerImageTag = '${containerRegistry}/${dockerImageName}:${dockerImageVersion}'

resource webApplication 'Microsoft.Web/sites@2021-03-01' = {
  kind: 'app,linux,container'
  name: webAppName
  location: azureLocation

  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }

  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${webAppName}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${webAppName}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarmId
    reserved: true
    isXenon: false
    hyperV: false
    siteConfig: {
      acrUseManagedIdentityCreds: false
      alwaysOn: true
      http20Enabled: true
      linuxFxVersion: 'DOCKER|${dockerImageTag}'
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 1
      numberOfWorkers: 1
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: '2A5AA6E28D65CDCD28744FFEF2616D1331D7D02DCC28B2C1687B58581CAD56FC'
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource pubCredsPolFTP 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-03-01' = {
  parent: webApplication
  name: 'ftp'
  location: 'Norway East'
  properties: {
    allow: true
  }
}

resource pubCredsPolSCM 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-03-01' = {
  parent: webApplication
  name: 'scm'
  location: 'Norway East'
  properties: {
    allow: true
  }
}

var allowedOrigins = {
  backend: [
        'https://${webAppName}.azurewebsites.net'
        'http://localhost:3000'
      ]
  frontend: [
        'http://localhost:3000'
      ]
}

resource config 'Microsoft.Web/sites/config@2021-03-01' = {
  parent: webApplication
  name: 'web'
  location: 'Norway East'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
      'hostingstart.html'
    ]
    netFrameworkVersion: 'v4.0'
    linuxFxVersion: 'DOCKER|${dockerImageTag}'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: true
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: webAppName
    scmType: 'VSTSRM'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: true
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: true
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    cors: {
      allowedOrigins: allowedOrigins[end]
      supportCredentials: end == 'frontend'
    }
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: true
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.0'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 0
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 1
    azureStorageAccounts: {}
  }
}

resource hostnameBinding 'Microsoft.Web/sites/hostNameBindings@2021-03-01' = {
  parent: webApplication
  name: '${webAppName}.azurewebsites.net'
  location: 'Norway East'
  properties: {
    siteName: webAppName
    hostNameType: 'Verified'
  }
}
