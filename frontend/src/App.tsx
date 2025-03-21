import { ThemeProvider, createTheme } from "@mui/material"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { FC } from "react"

import AppRouter from "./Router"
import { buildConfig } from "./Services/config"
import { EditQueueProvider } from "./Store/EditQueueContext"
import { FeatureContextProvider } from "./Store/FeatureContext"
import { ModalContextProvider } from "./Store/ModalContext"
import { ProjectContextProvider } from "./Store/ProjectContext"
import { resolveConfiguration } from "./Utils/Config/EnvConfig"
import { EnvironmentVariables } from "./Utils/Config/environmentVariables"
import Styles from "./styles"
import { APP_VERSION } from "./version"

const storeAppId = (appId: string): void => {
    window.sessionStorage.setItem("appId", appId)
}

const storeAppScope = (appScope: string): void => {
    window.sessionStorage.setItem("appScope", appScope)
}

const App: FC = () => {
    const queryClient = new QueryClient({
        defaultOptions: {
            queries: {
                refetchOnWindowFocus: true,
                refetchOnReconnect: true,
            },
        },
    })
    const theme = createTheme({
        palette: {
            primary: {
                main: "#007079",
            },
            secondary: {
                main: "#FF9100",
            },
        },
    })

    const config = resolveConfiguration(EnvironmentVariables.ENVIRONMENT)

    buildConfig(config.REACT_APP_API_BASE_URL)
    storeAppId(config.APP_ID)
    storeAppScope(config.BACKEND_APP_SCOPE[0])

    console.log("Concept App version: ", APP_VERSION)

    return (
        <QueryClientProvider client={queryClient}>
            <FeatureContextProvider>
                <ThemeProvider theme={theme}>
                    <ReactQueryDevtools />
                    <ProjectContextProvider>
                        <EditQueueProvider>
                            <ModalContextProvider>
                                <Styles />
                                <AppRouter />
                            </ModalContextProvider>
                        </EditQueueProvider>
                    </ProjectContextProvider>
                </ThemeProvider>
            </FeatureContextProvider>
        </QueryClientProvider>
    )
}

export default App
