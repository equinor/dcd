import { FunctionComponent } from "react"
import ReactDOM from "react-dom"

const portalElement: HTMLElement = document.getElementById("portal")!

type Props = {}

export const Portal: FunctionComponent<Props> = ({ children }) => (
    ReactDOM.createPortal(children, portalElement)
)
