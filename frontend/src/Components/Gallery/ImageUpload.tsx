import React, { useEffect } from "react"
import { useDropzone } from "react-dropzone"
import { Box, Typography } from "@mui/material"
import styled from "styled-components"
import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { getImageService } from "../../Services/ImageService"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"

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
    setGallery: React.Dispatch<React.SetStateAction<string[]>>;
    gallery: string[];
    setExeededLimit: React.Dispatch<React.SetStateAction<boolean>>;
}

const ImageUpload: React.FC<ImageUploadProps> = ({ setGallery, gallery, setExeededLimit }) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    useEffect(() => {
        // Function to load images from the backend
        const loadImages = async () => {
            if (project?.id && projectCase?.id) {
                try {
                    const imageService = await getImageService()
                    const imageUrls = await imageService.getImages(project.id, projectCase.id)
                    setGallery(imageUrls)
                } catch (error) {
                    console.error("Error loading images:", error)
                }
            }
        }
        loadImages()
    }, [setGallery, project?.id, projectCase?.id])

    const onDrop = async (acceptedFiles: File[]) => {
        if (gallery.length + acceptedFiles.length > 4) {
            setExeededLimit(true)
            return
        }
        setExeededLimit(false)

        if (!project?.id || !projectCase?.id) {
            console.error("Project ID or Case ID is missing.")
            return
        }

        const imageService = await getImageService()

        const uploadPromises = acceptedFiles.map((file) => imageService.uploadImage(project.id, projectCase.id, file))
        try {
            const uploadedImageUrls = await Promise.all(uploadPromises)
            setGallery((prevGallery) => [...prevGallery, ...uploadedImageUrls])
        } catch (error) {
            console.error("Error uploading images:", error)
        }
    }

    const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop })

    return (
        <UploadBox {...getRootProps()}>
            <input {...getInputProps()} />
            <Icon data={add} size={48} />
            {isDragActive ? (
                <Typography variant="body1">Drop the images here...</Typography>
            ) : (
                <Typography variant="body1">Click or drag and drop images here to upload</Typography>
            )}
        </UploadBox>
    )
}

export default ImageUpload
