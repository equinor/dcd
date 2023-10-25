import {
    useAppConfig, useCurrentUser, useFusionEnvironment,
} from "@equinor/fusion"
import ConceptAppAuthProvider from "../auth/ConceptAppAuthProvider"
import { buildConfig } from "../Services/config"
import { StoreAppId, StoreAppScope } from "../Utils/common"
import { ResolveConfiguration } from "../Utils/config"
import { APP_VERSION } from "../version"
import { AppRouter } from "./AppRouter"
import { FusionRouterBootstrap } from "./FusionRouterBootstrap"

const setEnvironment = (): void => {
    const fusionEnv = useFusionEnvironment()
    localStorage.setItem("FUSION_ENV_LOCAL_CACHE_KEY", fusionEnv.env)
}

/**
 * Renders the appropriate view based on user authentication and matching application routes.
 * @returns {*} {JSX.Element}
 */
function App(): JSX.Element {
    setEnvironment()
    const user = useCurrentUser()
    const runtimeConfig = useAppConfig()
    const fusionEnvironment = useFusionEnvironment()

    const config = ResolveConfiguration(fusionEnvironment.env)

    if (runtimeConfig.value !== undefined && runtimeConfig.value !== null) {
        if (runtimeConfig.value?.endpoints.REACT_APP_API_BASE_URL) {
            buildConfig(runtimeConfig.value!.endpoints.REACT_APP_API_BASE_URL)
        }

        if (runtimeConfig.value?.environment) {
            const values: any = { ...runtimeConfig.value.environment }
            StoreAppId(values.APP_ID)
            StoreAppScope(values.BACKEND_APP_SCOPE)
        }
    } else {
        buildConfig(config.REACT_APP_API_BASE_URL)
        StoreAppId(config.APP_ID)
        StoreAppScope(config.BACKEND_APP_SCOPE[0])
    }

    console.log("Concept App version: ", APP_VERSION)

    return (
        <ConceptAppAuthProvider>
            <FusionRouterBootstrap>
                <AppRouter />
            </FusionRouterBootstrap>
        </ConceptAppAuthProvider>
    )
}

export default App
