import React, { useEffect } from "react"
import { Accept, FileRejection, useDropzone } from "react-dropzone"
import { Box } from "@mui/material"
import styled from "styled-components"
import { Icon, Typography } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useParams } from "react-router-dom"
import { getImageService } from "../../Services/ImageService"
import { useProjectContext } from "../../Context/ProjectContext"
import { useAppContext } from "../../Context/AppContext"

const UploadBox = styled(Box)`
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    width: 240px;
    height: 240px;
    border: 1px dashed ${tokens.colors.interactive.primary__resting.rgba};
    border-radius: 5px;
    cursor: pointer;
    transition: 0.3s;
    gap: 10px;

    & svg {
        fill: ${tokens.colors.interactive.primary__resting.rgba};
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
    setGallery: React.Dispatch<React.SetStateAction<Components.Schemas.ImageDto[]>>
    gallery: Components.Schemas.ImageDto[]
    setExeededLimit: React.Dispatch<React.SetStateAction<boolean>>
}

const ImageUpload: React.FC<ImageUploadProps> = ({ setGallery, gallery, setExeededLimit }) => {
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { setSnackBarMessage } = useAppContext()

    useEffect(() => {
        const loadImages = async () => {
            if (project?.id && caseId) {
                try {
                    const imageService = await getImageService()
                    const imageDtos = await imageService.getImages(project.id, caseId)
                    setGallery(imageDtos)
                } catch (error) {
                    console.error("Error loading images:", error)
                    setSnackBarMessage("Error loading images")
                }
            }
        }
        loadImages()
    }, [setGallery, project?.id, caseId])

    const MAX_FILE_SIZE = 5 * 1024 * 1024
    const MAX_FILES = 4
    const ACCEPTED_FILE_TYPES: Accept = {
        "image/jpeg": [".jpeg", ".jpg"],
        "image/png": [".png"],
        "image/gif": [".gif"],
    }

    const onDrop = async (acceptedFiles: File[], fileRejections: FileRejection[]) => {
        fileRejections.forEach((rejection) => {
            const { file, errors } = rejection
            errors.forEach((error: { code: string }) => {
                if (error.code === "file-too-large") {
                    setSnackBarMessage(`File ${file.name} is too large. Maximum size is 5MB.`)
                } else if (error.code === "file-invalid-type") {
                    setSnackBarMessage(`File ${file.name} is not an accepted image type.`)
                }
            })
        })

        if (gallery.length + acceptedFiles.length > MAX_FILES) {
            setExeededLimit(true)
            return
        }
        setExeededLimit(false)

        if (!project?.id) {
            console.error("Project ID is missing.")
            return
        }

        const imageService = await getImageService()

        const uploadPromises = acceptedFiles.map((file) => imageService.uploadImage(project.id, project.name, file, caseId))
        try {
            const uploadedImageDtos = await Promise.all(uploadPromises)
            if (Array.isArray(uploadedImageDtos)) {
                setGallery((prevGallery) => [...prevGallery, ...uploadedImageDtos])
            } else {
                console.error("Received undefined response from uploadImage")
            }
        } catch (error) {
            console.error("Error uploading images:", error)
            setSnackBarMessage("Error uploading images")
        }
    }

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: ACCEPTED_FILE_TYPES,
        maxSize: MAX_FILE_SIZE,
        maxFiles: MAX_FILES,
    })

    return (
        <UploadBox {...getRootProps()}>
            <input {...getInputProps()} />
            <Icon data={add} size={48} />
            {isDragActive ? (
                <Typography>Drop the images here...</Typography>
            ) : (
                <Typography>Click or drag and drop images here to upload</Typography>
            )}
        </UploadBox>
    )
}

export default ImageUpload
