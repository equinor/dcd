import { AppConfigurationClient } from '@azure/app-configuration'
import { LogLevel } from '@azure/msal-browser'

export const AzureAd = {
    clientId: '9b125a0c-4907-43b9-8db2-ff405d6b0524',
    authority: 'https://login.microsoftonline.com/3aa4a235-b6e2-48d5-9195-7fcf05b459b0',
    redirectUri: process.env.REACT_APP_REDIRECT_URI,
}

export const fusionApiScope = ['97978493-9777-4d48-b38a-67b0b9cd88d2/.default']

export const appInsightsInstrumentationKey = 'f218caa9-c1a4-4c31-973b-c787a90af4ce'

export async function RetrieveConfigFromAzure() {
    var authority = appConfigClient.getConfigurationSetting({
        key: 'AzureAd:Authority',
        label: environmentName,
    })

    var clientId = appConfigClient.getConfigurationSetting({
        key: 'AzureAd:ClientId',
        label: environmentName,
    })

    var redirecturl = appConfigClient.getConfigurationSetting({
        key: 'AzureAd:RedirectUrl',
        label: environmentName,
    })

    var appinnsightkey = appConfigClient.getConfigurationSetting({
        key: 'ApplicationInsightInstrumentationKey',
    })

    var backendurl = appConfigClient.getConfigurationSetting({
        key: 'BackendUrl',
        label: environmentName,
    })

    return Promise.all([clientId, authority, redirecturl, backendurl, appinnsightkey]).then(res => {
        const clientId = res[0]?.value ?? ''
        const authority = res[1]?.value ?? ''
        const redirectUri = res[2]?.value ?? ''

        return {
            azureAd: {
                clientId,
                authority,
                redirectUri,
            },
            backendUrl: res[3]?.value ?? '',
            applicationInsightInstrumentationKey: res[4]?.value ?? '',
            msal: {
                auth: {
                    clientId,
                    authority,
                    redirectUri,
                },
                cache: {
                    cacheLocation: 'sessionStorage',
                    storeAuthStateInCookie: false,
                },
                system: {
                    loggerOptions: {
                        loggerCallback: (level: any, message: any, containsPii: any) => {
                            if (containsPii) {
                                return
                            }
                            switch (level) {
                                case LogLevel.Error:
                                    console.error(message)
                                    return
                                case LogLevel.Info:
                                    console.info(message)
                                    return
                                case LogLevel.Verbose:
                                    console.debug(message)
                                    return
                                case LogLevel.Warning:
                                    console.warn(message)
                                    return
                            }
                        },
                    },
                },
            },
        }
    })
}

var appconfigconnstring = process.env.REACT_APP_AZURE_APP_CONFIG_CONNECTION_STRING || ''
var environmentName = process.env.REACT_APP_ENVIRONMENT || 'localdev'
const appConfigClient = new AppConfigurationClient(appconfigconnstring!)
console.log('Loading config for ENV: ' + environmentName)
