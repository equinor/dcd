import { AppModuleInitiator } from "@equinor/fusion-framework-app"
import { enableAgGrid } from "@equinor/fusion-framework-module-ag-grid"
import { enableNavigation } from "@equinor/fusion-framework-module-navigation"
import { enableContext } from "@equinor/fusion-framework-module-context"

export const configurator: AppModuleInitiator = (config, args) => {
    const { basename } = args.env
    console.log("Configuring app with basename", basename)
    enableAgGrid(config)
    config.useFrameworkServiceClient("portal")
    enableNavigation(config, basename)
    enableContext(config, (builder) => {
        builder.setContextType(["ProjectMaster"])
    })
}
