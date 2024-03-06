import { useDropzone } from "react-dropzone"
import { Box, Typography } from "@mui/material"
import styled from "styled-components"
import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"

const UploadBox = styled(Box)`
    display: flex;
    align-items: center;
    flex-direction: column;
    width: calc(100% - 40px);
    height: 160px;
    border: 1px dashed ${tokens.colors.interactive.primary__resting.rgba};
    border-radius: 5px;
    cursor: pointer;
    padding: 20px;
    transition: 0.3s;
    gap: 10px;

    & svg {
        fill: ${tokens.colors.interactive.primary__resting.rgba};
        margin-top: 20px;
    }

    &:hover {
        background-color: ${tokens.colors.interactive.primary__hover_alt.rgba};
    }

    & p {
        text-align: center;
        opacity: 0.9;
    }
`

interface ImageUploadProps {
    setGallery: React.Dispatch<React.SetStateAction<string[]>>
    gallery: string[]
    setExeededLimit: React.Dispatch<React.SetStateAction<boolean>>
}

const ImageUpload = ({ setGallery, gallery, setExeededLimit }: ImageUploadProps) => {
    const onDrop = (acceptedFiles: File[]) => {
        // adds only the amount of images that can fit in the gallery
        if (gallery.length + acceptedFiles.length > 4) {
            const newImages = acceptedFiles.slice(0, 4 - gallery.length).map((file) => URL.createObjectURL(file))
            setGallery((prevGallery) => [...prevGallery, ...newImages])
            setExeededLimit(true)
            return
        }
        setExeededLimit(false)
        const newImages = acceptedFiles.map((file) => URL.createObjectURL(file))
        setGallery((prevGallery) => [...prevGallery, ...newImages])
    }

    const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop })

    return (
        <UploadBox {...getRootProps()}>
            <input {...getInputProps()} />
            <Icon data={add} size={48} />
            {isDragActive ? (
                <Typography variant="body1">Drop the images here...</Typography>
            ) : (
                <Typography variant="body1">
                    Click or drag and drop images here to upload
                </Typography>
            )}
        </UploadBox>
    )
}

export default ImageUpload
