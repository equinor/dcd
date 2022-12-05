/* eslint-disable import/no-import-module-exports */
import { Context, ContextTypes, registerApp } from "@equinor/fusion"
import createApp, { createLegacyApp } from "@equinor/fusion-framework-react-app"
import { enableAgGrid } from "@equinor/fusion-framework-module-ag-grid"
import { LicenseManager } from "ag-grid-enterprise"
import App from "./app/App"

// @ts-ignore
if (window.Fusion?.modules?.agGrid?.licenseKey) {
    // @ts-ignore
    LicenseManager.setLicenseKey(window.Fusion?.modules?.agGrid?.licenseKey)
}

registerApp("conceptapp", {
    AppComponent: createLegacyApp(App, (config) => enableAgGrid(config)),
    context: {
        types: [ContextTypes.ProjectMaster],
        buildUrl: (context: Context | null) => (context ? `/${context.id}` : ""),
        getContextFromUrl: (url: string) => url.split("/")[1],
    },
    name: "DCD Concept App",
})

if (module.hot) module.hot.accept()
