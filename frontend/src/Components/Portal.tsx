import { FunctionComponent } from "react"
import ReactDOM from "react-dom"

const portalElement = document.getElementById("portal")!

export const Portal: FunctionComponent = ({ children }) => (
    ReactDOM.createPortal(children, portalElement)
)
