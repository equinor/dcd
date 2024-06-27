import { FunctionComponent, ReactNode } from "react"
import { Typography } from "@equinor/eds-core-react"
import Dialog from "@mui/material/Dialog"
import DialogTitle from "@mui/material/DialogTitle"
import DialogContent from "@mui/material/DialogContent"
import DialogActions from "@mui/material/DialogActions"

type Props = {
    title: string;
    isOpen: boolean;
    size?: false | "xs" | "sm" | "md" | "lg" | "xl" | undefined;
    onClose?: () => void;
    content: ReactNode;
    actions?: ReactNode;
}

const Modal: FunctionComponent<Props> = ({
    isOpen,
    title,
    size,
    onClose,
    content,
    actions,
}) => {
    if (!isOpen) { return null }
    return (
        <Dialog
            open={isOpen}
            fullWidth
            maxWidth={size || "lg"}
            className="ConceptApp ag-theme-alpine-fusion"
            onClose={onClose}
        >
            <DialogTitle><Typography variant="h2" as="p">{title}</Typography></DialogTitle>
            <DialogContent>{content}</DialogContent>
            {actions && <DialogActions>{actions}</DialogActions>}
        </Dialog>
    )
}

export default Modal
