import { useAppConfig, useCurrentUser, useFusionEnvironment } from "@equinor/fusion"
import { FC } from "react"
import { ModuleRegistry } from "@ag-grid-community/core"
import { ClientSideRowModelModule } from "@ag-grid-community/client-side-row-model"
import { ColumnsToolPanelModule } from "@ag-grid-enterprise/column-tool-panel"
import { FiltersToolPanelModule } from "@ag-grid-enterprise/filter-tool-panel"
import { RangeSelectionModule } from "@ag-grid-enterprise/range-selection"
import { ClipboardModule } from "@ag-grid-enterprise/clipboard"
import { MultiFilterModule } from "@ag-grid-enterprise/multi-filter"
import { SetFilterModule } from "@ag-grid-enterprise/set-filter"
import { MenuModule } from "@ag-grid-enterprise/menu"
import { GridChartsModule } from "@ag-grid-enterprise/charts"
import { AppRouter } from "./app/AppRouter"
import { StoreAppId, StoreAppScope } from "./Utils/common"
import { FusionRouterBootstrap } from "./app/FusionRouterBootstrap"
import ConceptAppAuthProvider from "./auth/ConceptAppAuthProvider"
import { APP_VERSION } from "./version"
import { buildConfig } from "./Services/config"
import { ResolveConfiguration } from "./Utils/config"

ModuleRegistry.registerModules([
    ClientSideRowModelModule,
    ColumnsToolPanelModule,
    FiltersToolPanelModule,
    RangeSelectionModule,
    ClipboardModule,
    MultiFilterModule,
    SetFilterModule,
    MenuModule,
    GridChartsModule,
])

const setEnvironment = (): void => {
    const fusionEnv = useFusionEnvironment()
    localStorage.setItem("FUSION_ENV_LOCAL_CACHE_KEY", fusionEnv.env)
}

const AppComponent: FC = () => {
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

export default AppComponent
