import { createElement } from "react"
import { createRoot } from "react-dom/client"
import { registerApp } from "@equinor/fusion"
import { ComponentRenderArgs, createComponent, makeComponent } from "@equinor/fusion-framework-react-app"

import AppComponent from "./AppComponent"
import { configurator } from "./config"

const appComponent = createElement(AppComponent)

const createApp = (args: ComponentRenderArgs) => makeComponent(appComponent, args, configurator)

export default function (el: HTMLElement, args: ComponentRenderArgs) {
    const app = createApp(args)
    const root = createRoot(el)
    root.render(createElement(app))
    return () => root.unmount()
}

// export const render = createComponent(AppComponent, configurator)

// registerApp("conceptapp", {
//     AppComponent,
//     render,
// })

// if (module.hot) {
//     module.hot.accept()
// }

// export default render
