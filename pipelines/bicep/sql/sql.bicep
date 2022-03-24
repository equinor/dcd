param location string = resourceGroup().location
param baseAppName string = 'dcd'
param sqlAdminPw string = ''
var appConfigName = 'sql-${baseAppName}'
var deployments = [
  {
    env: 'dev'
  }
  {
    env: 'pr'
  }
  {
    env: 'qa'
  }
  {
    env: 'prod'
  }
]


resource sqlServer 'Microsoft.Sql/servers@2021-05-01-preview' = {
  name: 'sql-${baseAppName}'
  location: location
  properties: {
    administratorLogin: 'conceptappadmin'
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
    administratorLoginPassword: sqlAdminPw
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-05-01-preview'  = [for deployment in deployments: {
  parent: sqlServer
  name: 'sqldb-${baseAppName}-${deployment.env}'
  location: location
  sku: {
    name: 'GP_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 2
  }
  kind: 'v12.0,user,vcore'
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    licenseType: 'BasePrice'
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Local'
    maintenanceConfigurationId: '/subscriptions/9e4deb6c-b5ed-443f-a940-c68f3087907f/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
    isLedgerOn: false
  }
}]
