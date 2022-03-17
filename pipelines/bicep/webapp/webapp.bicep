//
// az bicep publish --file webapp.bicep --target "br:crdcd.azurecr.io/bicep/webapp:v1"
//
param environment string = 'dev'
param azureLocation string = resourceGroup().location
param baseAppName string = 'dcd'
param serverFarmId string = ''

var webAppName = 'ase-${baseAppName}-${environment}'
//var containerName = 
resource webApplication 'Microsoft.Web/sites@2021-02-01' = {
  kind: 'app,linux,container'
  name: webAppName
  location: azureLocation
  
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    serverFarmId: serverFarmId
    siteConfig: {
      acrUseManagedIdentityCreds: true
      alwaysOn: true
      functionAppScaleLimit: 0
      http20Enabled: false
      linuxFxVersion: 'DOCKER|crdcd.azurecr.io/dcd_backend:latest'
      minimumElasticInstanceCount: 1
      numberOfWorkers: 1
    }
  }
}

