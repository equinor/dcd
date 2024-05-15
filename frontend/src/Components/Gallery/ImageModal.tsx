import React from "react"
import { Dialog } from "@mui/material"
import styled from "styled-components"

const StyledImage = styled.img`
    width: 100%;
    height: 100%;
    object-fit: contain;
`

interface ExpandedImageProps {
    image: string;
    modalOpen: boolean;
    setModalOpen: (open: boolean) => void;
}

const ExpandedImage: React.FC<ExpandedImageProps> = ({
    image,
    modalOpen,
    setModalOpen,
}) => (
    <Dialog
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onClick={() => setModalOpen(false)}
    >
        <StyledImage src={image} alt="Expanded" />
    </Dialog>
)

export default ExpandedImage
