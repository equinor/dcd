import { FC } from "react"
import { APP_VERSION } from "./version"
import AppRouter from "./Router"
import { AppContextProvider } from "./Context/AppContext"
import { resolveConfiguration } from "./Utils/config"
import { EnvironmentVariables } from "./environmentVariables"
import { storeAppId, storeAppScope } from "./Utils/common"
import { buildConfig } from "./Services/config"
import { ModalContextProvider } from "./Context/ModalContext"

const AppComponent: FC = () => {
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

    const config = resolveConfiguration(EnvironmentVariables.ENVIRONMENT)

    buildConfig(config.REACT_APP_API_BASE_URL)
    storeAppId(config.APP_ID)
    storeAppScope(config.BACKEND_APP_SCOPE[0])

    console.log("Concept App version: ", APP_VERSION)

    return (
        <AppContextProvider>
            <ModalContextProvider>
                <AppRouter />
            </ModalContextProvider>
        </AppContextProvider>
    )
}

export default AppComponent
