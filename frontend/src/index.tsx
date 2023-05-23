import { registerApp as registerLegacy } from "@equinor/fusion"
import { createComponent } from "@equinor/fusion-framework-react-app"

import App from "./app/App"
import { configurator } from "./config"

registerLegacy("conceptapp", {
    render: createComponent(App, configurator),
    AppComponent: App,
})

if (module.hot) module.hot.accept()