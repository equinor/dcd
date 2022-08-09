import { useAppConfig, useCurrentUser, useFusionEnvironment } from "@equinor/fusion"
import { ErrorBoundary } from "@equinor/fusion-components"
import { FUSION_ENV_LOCAL_CACHE_KEY } from "../api/apiConfig"
import ConceptAppAuthProvider from "../auth/ConceptAppAuthProvider"
import { buildConfig } from "../Services/config"
import { AppRouter } from "./AppRouter"
import { FusionRouterBootstrap } from "./FusionRouterBootstrap"

const setEnvironment = (): void => {
    const fusionEnv = useFusionEnvironment()
    localStorage.setItem(FUSION_ENV_LOCAL_CACHE_KEY, fusionEnv.env)
}

/**
 * Renders the appropriate view based on user authentication and matching application routes.
 * @returns {*} {JSX.Element}
 */
function App(): JSX.Element {
    setEnvironment()
    const user = useCurrentUser()
    const runtimeConfig = useAppConfig()
    if (runtimeConfig.value?.endpoints.REACT_APP_API_BASE_URL) {
        buildConfig(runtimeConfig.value!.endpoints.REACT_APP_API_BASE_URL)
    }
    return (
        <ErrorBoundary>
            <ConceptAppAuthProvider>
                {(() => {
                    if (!user) {
                        return <p>Please login</p>
                    }
                    // eslint-disable-next-line max-len
                    if (runtimeConfig.value?.endpoints.REACT_APP_API_BASE_URL === null || runtimeConfig.value?.endpoints.REACT_APP_API_BASE_URL === undefined) {
                        return <p>Fetching Fusion app config</p>
                    }

                    buildConfig(runtimeConfig.value!.endpoints.REACT_APP_API_BASE_URL)

                    return (
                        <FusionRouterBootstrap>
                            <AppRouter />
                        </FusionRouterBootstrap>
                    )
                })()}
            </ConceptAppAuthProvider>
        </ErrorBoundary>
    )
}

export default App
