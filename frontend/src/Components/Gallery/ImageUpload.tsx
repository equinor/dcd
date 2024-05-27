import { useDropzone } from "react-dropzone"
import { Box, Typography } from "@mui/material"
import styled from "styled-components"
import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import axios from "axios"

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
    const onDrop = async (acceptedFiles: File[]) => {
        // Check if the gallery limit is exceeded
        if (gallery.length + acceptedFiles.length > 4) {
            const newImages = acceptedFiles.slice(0, 4 - gallery.length).map((file) => URL.createObjectURL(file))
            setGallery((prevGallery) => [...prevGallery, ...newImages])
            setExeededLimit(true)
            return
        }
        setExeededLimit(false)

        // Map each file to an upload promise
        const uploadPromises = acceptedFiles.map(async (file) => {
            // Get the SAS token and blobName from the backend
            const sasResponse = await axios.get("/api/images/sas-token")
            const { sasUrl, blobName } = sasResponse.data
            console.log("SAS URL:", sasUrl)

            const formData = new FormData()
            formData.append("image", file) // Match the parameter name expected by the backend

            // Upload the file using the SAS token
            const uploadResponse = await axios.put(sasUrl, file, {
                headers: {
                    "Content-Type": file.type, // Set the content type to the file's type
                    "x-ms-blob-type": "BlockBlob", // Required for Azure Blob Storage
                },
            })

            // Return the image URL to be added to the gallery
            const responseUrl = uploadResponse.config?.url ?? ""
            console.log("responseUrl:", responseUrl)

            // Construct the image URL without the SAS token
            const imageUrl = new URL(responseUrl)
            console.log("imageUrl:", imageUrl)

            imageUrl.search = "" // Remove the query string (SAS token)
            return imageUrl.href + blobName // Append the blob name to the base URL
        })

        // Wait for all uploads to complete
        try {
            const uploadedImageUrls = await Promise.all(uploadPromises)
            setGallery((prevGallery) => [...prevGallery, ...uploadedImageUrls])
        } catch (error) {
            console.error("Error uploading images:", error)
            // Handle the error appropriately
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
                <Typography variant="body1">
                    Click or drag and drop images here to upload
                </Typography>
            )}
        </UploadBox>
    )
}

export default ImageUpload
