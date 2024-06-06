import React, { useEffect } from "react"
import { Accept, FileRejection, useDropzone } from "react-dropzone"
import { Box, Typography } from "@mui/material"
import styled from "styled-components"
import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { getImageService } from "../../Services/ImageService"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { useAppContext } from "../../Context/AppContext"

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

const ImageUpload: React.FC<ImageUploadProps> = ({ setGallery, gallery, setExeededLimit }) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()
    const { setSnackBarMessage } = useAppContext()

    useEffect(() => {
        const loadImages = async () => {
            if (project?.id && projectCase?.id) {
                try {
                    const imageService = await getImageService()
                    const imageDtos = await imageService.getImages(project.id, projectCase.id)
                    const imageUrls = imageDtos.map((dto) => dto.url)
                    setGallery(imageUrls)
                } catch (error) {
                    console.error("Error loading images:", error)
                }
            }
        }
        loadImages()
    }, [setGallery, project?.id, projectCase?.id])

    const MAX_FILE_SIZE = 5 * 1024 * 1024
    const MAX_FILES = 4
    const ACCEPTED_FILE_TYPES: Accept = {
        "image/jpeg": [".jpeg", ".jpg"],
        "image/png": [".png"],
        "image/gif": [".gif"],
    }

    const onDrop = async (acceptedFiles: File[], fileRejections: FileRejection[]) => {
        setSnackBarMessage("")

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

        if (!project?.id || !projectCase?.id) {
            console.error("Project ID or Case ID is missing.")
            return
        }

        const imageService = await getImageService()

        const uploadPromises = acceptedFiles.map((file) => imageService.uploadImage(project.id, project.name, projectCase.id, file))
        try {
            const uploadedImageDtos = await Promise.all(uploadPromises)
            if (Array.isArray(uploadedImageDtos)) {
                const uploadedImageUrls = uploadedImageDtos.map((dto) => dto.url)
                setGallery((prevGallery) => [...prevGallery, ...uploadedImageUrls])
            } else {
                console.error("Received undefined response from uploadImage")
            }
        } catch (error) {
            console.error("Error uploading images:", error)
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
            {/* ... (existing code) */}
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
