import { Icon, Typography } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { Box } from "@mui/material"
import React from "react"
import { Accept, FileRejection, useDropzone } from "react-dropzone"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import { useAppStore } from "../../Store/AppStore"

import { useEditGallery } from "@/Hooks/useEditGallery"
import { useProjectContext } from "@/Store/ProjectContext"

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
    gallery: Components.Schemas.ImageDto[]
}

const ImageUpload: React.FC<ImageUploadProps> = ({ gallery }) => {
    const { caseId } = useParams()
    const { setSnackBarMessage } = useAppStore()
    const { projectId } = useProjectContext()
    const { addImage } = useEditGallery()

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
            setSnackBarMessage("Error uploading images. Maximum number of images is 4.")

            return
        }

        if (!projectId) {
            console.error("Project ID is missing.")

            return
        }
        acceptedFiles.map((file) => addImage(projectId, file, caseId))
    }

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: ACCEPTED_FILE_TYPES,
        maxSize: MAX_FILE_SIZE,
        maxFiles: MAX_FILES,
    })

    return (
        <div>
            <UploadBox {...getRootProps()}>
                <input {...getInputProps()} />
                <Icon data={add} size={48} />
                {isDragActive ? (
                    <Typography>Drop the images here...</Typography>
                ) : (
                    <Typography>Click or drag and drop images here to upload</Typography>
                )}
            </UploadBox>
        </div>
    )
}

export default ImageUpload
