import { AppModuleInitiator } from "@equinor/fusion-framework-app"
import { enableAgGrid } from "@equinor/fusion-framework-module-ag-grid"
import { enableNavigation } from "@equinor/fusion-framework-module-navigation"
import { enableContext } from "@equinor/fusion-framework-module-context"
import { ModuleRegistry } from "@ag-grid-community/core"
import { ClientSideRowModelModule } from "@ag-grid-community/client-side-row-model"
import { GridChartsModule } from "@ag-grid-enterprise/charts"
import { ClipboardModule } from "@ag-grid-enterprise/clipboard"
import { ColumnsToolPanelModule } from "@ag-grid-enterprise/column-tool-panel"
import { FiltersToolPanelModule } from "@ag-grid-enterprise/filter-tool-panel"
import { MenuModule } from "@ag-grid-enterprise/menu"
import { MultiFilterModule } from "@ag-grid-enterprise/multi-filter"
import { RangeSelectionModule } from "@ag-grid-enterprise/range-selection"
import { SetFilterModule } from "@ag-grid-enterprise/set-filter"
import { ExcelExportModule } from "@ag-grid-enterprise/excel-export"

export const configure: AppModuleInitiator = (configurator, args) => {
    const { agGridLicense } = (args.env.config?.environment as { agGridLicense?: string })
    const { basename } = args.env
    console.log("Configuring app with basename", basename)

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
        ExcelExportModule,
    ])

    enableAgGrid(configurator, {
        licenseKey: agGridLicense || "",
    })

    configurator.useFrameworkServiceClient("portal")

    enableNavigation(configurator, basename)

    enableContext(configurator, (builder) => {
        builder.setContextType(["ProjectMaster"])
    })
}

export default configure
