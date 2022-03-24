param location string = resourceGroup().location
param baseAppName string = 'dcd'

var logWorkspaceName = 'log-${baseAppName}'

resource workspaces 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: logWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    workspaceCapping: {
      dailyQuotaGb: -1
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource appInsightInstance 'microsoft.insights/components@2020-02-02' = {
  name: 'appi-${baseAppName}'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 90
    WorkspaceResourceId: workspaces.id
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

output appInsightKey string = appInsightInstance.properties.InstrumentationKey
