import { styled } from "@mui/material/styles"
import Grid from "@mui/material/Grid2"

export const ModalContent = styled(Grid)({
    width: "100%",
})

export const ModalActions = styled(Grid)(({ theme }) => ({
    padding: theme.spacing(2),
}))

export const FormSection = styled(Grid)(({ theme }) => ({
    marginBottom: theme.spacing(1),
}))

export const InputGroup = styled(Grid)({
    display: "flex",
    flexDirection: "column",
})
