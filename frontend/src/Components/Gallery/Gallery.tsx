import { Typography } from "@equinor/eds-core-react"
import { Grid } from "@mui/material"
import { useQuery } from "@tanstack/react-query"
import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import GalleryImage from "./GalleryImage"
import ImageModal from "./ImageModal"
import ImageUpload from "./ImageUpload"

import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useEditGallery } from "@/Hooks/useEditGallery"
import { galleryImagesQueryFn } from "@/Services/QueryFunctions"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

const Wrapper = styled.div`
    display: flex;
    justify-content: start;
    gap: 10px;
    margin-bottom: 30px;
    padding: 10px;
    width: 100%;
    overflow-x: auto;
    overflow-y: hidden;
    flex-wrap: wrap;
`

const GalleryLabel = styled(Typography) <{ $warning: boolean }>`
    font-size: 12px;
    font-weight: 500;
    line-height: 21px;
    color: ${({ $warning }) => ($warning ? "red" : "rgba(111, 111, 111, 1)")};
`

const Gallery = () => {
    const { setSnackBarMessage } = useAppStore()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { caseId } = useParams()
    const { revisionId } = useParams()
    const { projectId } = useProjectContext()
    const { canEdit } = useCanUserEdit()
    const { updateImageDescription, deleteImage } = useEditGallery()

    const { data: images, error } = useQuery({
        queryKey: ["gallery", projectId, caseId, revisionId].filter(Boolean),
        queryFn: () => galleryImagesQueryFn(projectId, caseId, revisionId),
        enabled: !!projectId,
    })

    const handleDescriptionChange = (imageId: string, newDescription: string) => {
        setGallery((prevGallery) => prevGallery.map((image) => (
            image.imageId === imageId
                ? { ...image, description: newDescription }
                : image
        )))
        if (projectId) {
            updateImageDescription(projectId, imageId, newDescription, caseId)
        }
    }

    const handleDelete = async (imageId: string) => {
        if (projectId) {
            deleteImage(projectId, imageId, caseId)
        }
    }

    const handleExpand = (image: string) => {
        setExpandedImage(image)
        setModalOpen(true)
    }

    useEffect(() => {
        if (error) {
            setSnackBarMessage("Error loading images")
        }
    }, [error, setSnackBarMessage])

    useEffect(() => {
        if (images) {
            setGallery(images)
            setExeededLimit(images.length >= 4)
        }
    }, [images])

    return gallery.length > 0 || canEdit() ? (
        <Grid item xs={12}>
            <ImageModal image={expandedImage} modalOpen={modalOpen} setModalOpen={setModalOpen} />
            <GalleryLabel $warning={exeededLimit}>
                {gallery.length > 0 && !canEdit() && "Gallery"}
                {canEdit() && `Gallery (${gallery.length} / 4)`}
            </GalleryLabel>
            <Wrapper>
                {gallery.map((image) => (
                    <GalleryImage
                        key={image.imageId}
                        image={image}
                        editAllowed={canEdit()}
                        onDelete={handleDelete}
                        onExpand={handleExpand}
                        onDescriptionChange={handleDescriptionChange}
                    />
                ))}
                {canEdit() && gallery.length < 4 && (
                    <ImageUpload gallery={gallery} />
                )}
            </Wrapper>
        </Grid>
    ) : null
}

export default Gallery
