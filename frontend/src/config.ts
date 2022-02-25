import { AppConfigurationClient } from "@azure/app-configuration"
import { LogLevel } from "@azure/msal-browser"

export const AzureAd = {
    clientId: "9b125a0c-4907-43b9-8db2-ff405d6b0524",
    authority: "https://login.microsoftonline.com/3aa4a235-b6e2-48d5-9195-7fcf05b459b0",
    redirectUri: process.env.REACT_APP_REDIRECT_URI,
}

export const fusionApiScope = ["97978493-9777-4d48-b38a-67b0b9cd88d2/.default"]

export const appInsightsInstrumentationKey = "f218caa9-c1a4-4c31-973b-c787a90af4ce"

const APP_CONFIG_CONN_STRING = process.env.REACT_APP_AZURE_APP_CONFIG_CONNECTION_STRING ?? ""
const ENV_NAME = process.env.REACT_APP_ENVIRONMENT ?? "localdev"

export async function RetrieveConfigFromAzure() {
    console.log(`[RetrieveConfigFromAzure] Loading App Config for ${ENV_NAME}`)

    const appConfigClient = new AppConfigurationClient(APP_CONFIG_CONN_STRING)

    const authorityPromise = appConfigClient.getConfigurationSetting({
        key: "AzureAd:Authority",
        label: ENV_NAME,
    })

    const clientIdPromise = appConfigClient.getConfigurationSetting({
        key: "AzureAd:ClientId",
        label: ENV_NAME,
    })

    const redirectURLPromise = appConfigClient.getConfigurationSetting({
        key: "AzureAd:RedirectUrl",
        label: ENV_NAME,
    })

    const appInsightKeyPromise = appConfigClient.getConfigurationSetting({
        key: "ApplicationInsightInstrumentationKey",
    })

    const backendURLPromise = appConfigClient.getConfigurationSetting({
        key: "BackendUrl",
        label: ENV_NAME,
    })

    return Promise.all([
        clientIdPromise,
        authorityPromise,
        redirectURLPromise,
        backendURLPromise,
        appInsightKeyPromise,
    ]).then((res) => {
        const clientId = res[0]?.value ?? ""
        const authority = res[1]?.value ?? ""
        const redirectUri = res[2]?.value ?? ""

        return {
            azureAd: {
                clientId,
                authority,
                redirectUri,
            },
            backendUrl: res[3]?.value ?? "",
            applicationInsightInstrumentationKey: res[4]?.value ?? "",
            msal: {
                auth: {
                    clientId,
                    authority,
                    redirectUri,
                },
                cache: {
                    cacheLocation: "sessionStorage",
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
                                break
                            case LogLevel.Info:
                                console.info(message)
                                break
                            case LogLevel.Verbose:
                                console.debug(message)
                                break
                            case LogLevel.Warning:
                                console.warn(message)
                                break
                            default:
                                console.log(message)
                                break
                            }
                        },
                    },
                },
            },
        }
    })
}
