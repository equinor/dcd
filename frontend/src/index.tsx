import { registerApp } from "@equinor/fusion"
import { createComponent } from "@equinor/fusion-framework-react-app"

import AppComponent from "./AppComponent"
import { configurator } from "./config"

declare let module: NodeModule
interface NodeModule {
  hot: any
}

export const render = createComponent(AppComponent, configurator)

registerApp("conceptapp", {
    AppComponent,
    render,
})

if (module.hot) {
    module.hot.accept()
}

export default render
