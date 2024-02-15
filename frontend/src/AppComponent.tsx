import { FC } from "react"
import { APP_VERSION } from "./version"
import AppRouter from "./Router"
import { AppContextProvider } from "./context/AppContext"

const AppComponent: FC = () => {
    // const runtimeConfig = useAppConfig()

    const suppressConsoleError = (shouldBeHidden: ((message: string) => boolean)[]) => {
        const err = console.error
        console.error = (message?: any, ...optionalParams: any[]) => {
            if (typeof message === "string" && shouldBeHidden.some((func) => func(message))) {
                return
            }
            err(message, ...optionalParams)
        }
    }

    suppressConsoleError([
        (m) => m.startsWith("Warning: Invalid aria prop"),
        (m) => m.startsWith("*"),
    ])

    // const config = resolveConfiguration(EnvironmentVariables.ENVIRONMENT)

    // if (runtimeConfig.value !== undefined && runtimeConfig.value !== null) {
    //     if (runtimeConfig.value?.endpoints.REACT_APP_API_BASE_URL) {
    //         buildConfig(runtimeConfig.value!.endpoints.REACT_APP_API_BASE_URL)
    //     }

    //     if (runtimeConfig.value?.environment) {
    //         const values: any = { ...runtimeConfig.value.environment }
    //         storeAppId(values.APP_ID)
    //         storeAppScope(values.BACKEND_APP_SCOPE)
    //     }
    // } else {
    // buildConfig(config.REACT_APP_API_BASE_URL)
    // storeAppId(config.APP_ID)
    // storeAppScope(config.BACKEND_APP_SCOPE[0])
    // }

    console.log("Concept App version: ", APP_VERSION)

    return (
        <>
            <h1>Concept App</h1>
            <AppContextProvider>
                <AppRouter />
            </AppContextProvider>

        </>
    )
}

export default AppComponent
