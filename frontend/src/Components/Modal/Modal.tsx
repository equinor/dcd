import { FunctionComponent, ReactNode, Children } from "react"
import { Typography } from "@equinor/eds-core-react"
import Dialog from "@mui/material/Dialog"
import DialogTitle from "@mui/material/DialogTitle"
import DialogContent from "@mui/material/DialogContent"
import DialogActions from "@mui/material/DialogActions"

type Props = {
    title: string;
    isOpen: boolean;
    children: ReactNode;
}

const Modal: FunctionComponent<Props> = ({
    isOpen, title, children,
}) => {
    if (!isOpen) return null
    return (
        <Dialog open={isOpen} fullWidth maxWidth="lg" className="ConceptApp ag-theme-alpine-fusion">
            {title && <DialogTitle><Typography variant="h2" as="p">{title}</Typography></DialogTitle>}
            {Children.map(children, (child, index) => (index === 0 && <DialogContent>{child}</DialogContent>))}
            {Children.map(children, (child, index) => (index === 1 && <DialogActions>{child}</DialogActions>))}
        </Dialog>
    )
}

export default Modal
