import { FC } from "react"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { ThemeProvider, createTheme } from "@mui/material"
import { APP_VERSION } from "./version"
import AppRouter from "./Router"
import { resolveConfiguration } from "./Utils/config"
import { EnvironmentVariables } from "./environmentVariables"
import { storeAppId, storeAppScope } from "./Utils/common"
import { buildConfig } from "./Services/config"
import { ModalContextProvider } from "./Store/ModalContext"
import { ProjectContextProvider } from "./Store/ProjectContext"
import { FeatureContextProvider } from "./Store/FeatureContext"
import { EditQueueProvider } from "./Store/EditQueueContext"
import Styles from "./styles"

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
