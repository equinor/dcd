import { createElement } from "react"
import { createRoot } from "react-dom/client"
import { ComponentRenderArgs, makeComponent } from "@equinor/fusion-framework-react-app"

import App from "./App"
import { configure } from "./Utils/AgGrid/AgGridConfig"

const appComponent = createElement(App)

const createApp = (args: ComponentRenderArgs) => makeComponent(appComponent, args, configure)

export const renderApp = (el: HTMLElement, args: ComponentRenderArgs) => {
    const app = createApp(args)
    const root = createRoot(el)
    root.render(createElement(app))
    return () => root.unmount()
}

export default renderApp
