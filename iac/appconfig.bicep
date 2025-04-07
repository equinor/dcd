param location string
param environmentName string
param preprod bool
param keyVaultName string
param appConfigName string

var keyVaultReferenceContentType = 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'

var pipelineEnvNameMap = {
  ci: 'radix-dev'
  qa: 'radix-qa'
  prod: 'radix-prod'
}

var configEnvMap = {
  ci: {
    FusionPeopleBaseUrl: 'https://people.ci.api.fusion-dev.net/'
    FusionPeopleScopes: '5a842df8-3238-415d-b168-9f16a6a6031b/.default'
  }
  qa: {
    FusionPeopleBaseUrl: 'https://people.fqa.api.fusion-dev.net'
    FusionPeopleScopes: '5a842df8-3238-415d-b168-9f16a6a6031b/.default'
  }
  prod: {
    FusionPeopleBaseUrl: 'https://people.api.fusion.equinor.com/'
    FusionPeopleScopes: '97978493-9777-4d48-b38a-67b0b9cd88d2/.default'
  }
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2024-05-01' existing = {
  name: appConfigName
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource appConfigStorageAccountKeyVaultReference 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'BlobStorageConnectionString'
  properties: {
    value: '{"uri":"${keyVault.properties.vaultUri}secrets/storage-account-connection-string"}'
    contentType: keyVaultReferenceContentType
  }
}

resource appConfigSqlDatabaseKeyVaultReference 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'Db:ConnectionString$${pipelineEnvNameMap[environmentName]}'
  properties: {
    value: '{"uri":"${keyVault.properties.vaultUri}secrets/sql-connection-string-${environmentName}"}'
    contentType: keyVaultReferenceContentType
  }
}

resource appConfigClientSecretKeyVaultReference 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'AzureAd:ClientSecret$${pipelineEnvNameMap[environmentName]}'
  properties: {
    value: preprod
      ? '{"uri":"${keyVault.properties.vaultUri}secrets/dcd-backend-client-secret-fusion-preprod"}'
      : '{"uri":"${keyVault.properties.vaultUri}secrets/dcd-backend-client-secret-fusion-prod"}'
    contentType: keyVaultReferenceContentType
  }
}

resource appConfigTentantId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'AzureAd:TenantId'
  properties: {
    value: '3aa4a235-b6e2-48d5-9195-7fcf05b459b0'
  }
}

resource appConfigInstanceId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'AzureAd:Instance'
  properties: {
    value: 'https://login.microsoftonline.com'
  }
}

resource appConfigClientId 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'AzureAd:ClientId'
  properties: {
    value: preprod ? '151950a5-f886-47cd-b361-afb81e75c345' : '81bd7b7f-4096-4c4f-b0c2-ebef7d05c0e6'
  }
}

resource appConfigFusionPeopleBaseUrl 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'FusionPeople:BaseUrl$${pipelineEnvNameMap[environmentName]}'
  properties: {
    value: configEnvMap[environmentName].FusionPeopleBaseUrl
  }
}

resource appConfigFusionPeopleScopes 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
  parent: appConfig
  name: 'FusionPeople:Scopes:0$${pipelineEnvNameMap[environmentName]}'
  properties: {
    value: configEnvMap[environmentName].FusionPeopleScopes
  }
}

resource appConfigGraphScopes 'Microsoft.AppConfiguration/configurationStores/keyValues@2024-05-01' = {
    parent: appConfig
    name: 'Graph:Scopes:0'
    properties: {
      value: 'Sites.Read.All'
    }
  }
