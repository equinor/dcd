targetScope = 'subscription'

param environmentName string
param location string = 'norwayeast'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'dcd-rg-${environmentName}'
  location: location
}
