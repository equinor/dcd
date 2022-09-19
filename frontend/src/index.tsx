/* eslint-disable import/no-import-module-exports */
import { Context, ContextTypes, registerApp } from "@equinor/fusion"
import createApp, { createLegacyApp } from "@equinor/fusion-framework-react-app"
import { enableAgGrid } from "@equinor/fusion-framework-module-ag-grid"
import App from "./app/App"

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
