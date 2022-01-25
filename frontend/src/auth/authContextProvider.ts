import { LogLevel } from '@azure/msal-browser'
import { AzureAd } from '../config'

export const msalConfig = {
    auth: {
        clientId: AzureAd.clientId,
        authority: AzureAd.authority,
        redirectUri: AzureAd.redirectUri,
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
}

export const loginRequest = {
    scopes: ['api://9b125a0c-4907-43b9-8db2-ff405d6b0524/user_impersonation'],
}

export const graphConfig = {
    graphMeEndpoint: 'https://graph.microsoft.com/v1.0/me',
}
