/* eslint-disable import/no-import-module-exports */
import { Context, ContextTypes, registerApp } from "@equinor/fusion"
import createApp from "@equinor/fusion-framework-react-app"
import App from "./app/App"

export const render = createApp(App)

registerApp("conceptapp", {
    render,
    AppComponent: App,
    context: {
        types: [ContextTypes.ProjectMaster],
        buildUrl: (context: Context | null) => (context ? `/${context.id}` : ""),
        getContextFromUrl: (url: string) => url.split("/")[1],
    },
    name: "DCD Concept App",
})

if (module.hot) module.hot.accept()
