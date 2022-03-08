param baseAppName string = 'dcd'

var deployments = [
  {
    end: 'frontend'
    env: 'dev'
  }
  {
    end: 'frontend'
    env: 'pr'
  }
  {
    end: 'frontend'
    env: 'qa'
  }
  {
    end: 'frontend'
    env: 'prod'
  }
  {
    end: 'backend'
    env: 'dev'
  }
  {
    end: 'backend'
    env: 'pr'
  }
  {
    end: 'backend'
    env: 'qa'
  }
  {
    end: 'backend'
    env: 'prod'
  }
]

module serverFarmDeploy 'serverfarm.bicep' = {
  name: 'serverfarm'
  params: {
    baseAppName: baseAppName
  }
}


module webApplicationDeploy 'webapp.bicep' = [for deployment in deployments: {
  name: 'webapp-${deployment.end}-${deployment.env}'
  params: {
    end: deployment.end
    environment: deployment.env
    baseAppName: baseAppName
    serverfarmId: serverFarmDeploy.outputs.serverfarmId
  }
}]
