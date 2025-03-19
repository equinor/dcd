import { ClientSideRowModelModule } from "@ag-grid-community/client-side-row-model"
import { ModuleRegistry } from "@ag-grid-community/core"
import { GridChartsModule } from "@ag-grid-enterprise/charts"
import { ClipboardModule } from "@ag-grid-enterprise/clipboard"
import { ColumnsToolPanelModule } from "@ag-grid-enterprise/column-tool-panel"
import { ExcelExportModule } from "@ag-grid-enterprise/excel-export"
import { FiltersToolPanelModule } from "@ag-grid-enterprise/filter-tool-panel"
import { MenuModule } from "@ag-grid-enterprise/menu"
import { MultiFilterModule } from "@ag-grid-enterprise/multi-filter"
import { RangeSelectionModule } from "@ag-grid-enterprise/range-selection"
import { SetFilterModule } from "@ag-grid-enterprise/set-filter"
import { AppModuleInitiator } from "@equinor/fusion-framework-app"
import { enableAgGrid } from "@equinor/fusion-framework-module-ag-grid"
import { enableContext } from "@equinor/fusion-framework-module-context"
import { enableNavigation } from "@equinor/fusion-framework-module-navigation"
// import { agGridLicenseKey } from "./agGridLicense"

export const configure: AppModuleInitiator = (configurator, args) => {
    const { basename } = args.env

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

    // if (agGridLicenseKey && agGridLicenseKey.length > 0) {
    //     enableAgGrid(configurator, {
    //         licenseKey: agGridLicenseKey || "",
    //     })
    // } else {
    //     enableAgGrid(configurator)
    // }

    enableAgGrid(configurator)

    configurator.useFrameworkServiceClient("portal")

    enableNavigation(configurator, basename)

    enableContext(configurator, (builder) => {
        builder.setContextType(["ProjectMaster"])
    })
}

export default configure
