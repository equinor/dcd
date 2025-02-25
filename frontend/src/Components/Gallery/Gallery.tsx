import { useEffect, useState } from "react"
import { Grid } from "@mui/material"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"

import ImageUpload from "./ImageUpload"
import ImageModal from "./ImageModal"
import GalleryImage from "./GalleryImage"
import { useAppStore } from "@/Store/AppStore"
import { getImageService } from "@/Services/ImageService"
import { useProjectContext } from "@/Store/ProjectContext"
import useEditDisabled from "@/Hooks/useEditDisabled"

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
    const { editMode, setSnackBarMessage } = useAppStore()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { caseId } = useParams()
    const { revisionId } = useParams()
    const { projectId } = useProjectContext()
    const { isEditDisabled } = useEditDisabled()

    const [debounceTimeout, setDebounceTimeout] = useState<NodeJS.Timeout | null>(null)

    useEffect(() => {
        const loadImages = async () => {
            const projectIdOrRevisionId = revisionId || projectId
            if (projectId) {
                try {
                    const imageService = getImageService()
                    const imageDtos = caseId
                        ? await imageService.getCaseImages(projectIdOrRevisionId, caseId)
                        : await imageService.getProjectImages(projectIdOrRevisionId)
                    setGallery(imageDtos)
                } catch (error) {
                    console.error("Error loading images:", error)
                    setSnackBarMessage("Error loading images")
                }
            }
        }

        loadImages()
    }, [projectId, caseId, revisionId, setSnackBarMessage])

    const handleDescriptionChange = async (imageId: string, newDescription: string) => {
        if (debounceTimeout) {
            clearTimeout(debounceTimeout)
        }
        setGallery((prevGallery) => prevGallery.map((image) => (image.imageId === imageId ? { ...image, description: newDescription } : image)))

        const timeout = setTimeout(async () => {
            try {
                const imageService = getImageService()
                const image = gallery.find((img) => img.imageId === imageId)
                if (image) {
                    const updateImageDto = {
                        description: newDescription,
                    }

                    if (caseId) {
                        await imageService.updateCaseImage(image.projectId, image.caseId, image.imageId, updateImageDto)
                    } else {
                        await imageService.updateProjectImage(image.projectId, image.imageId, updateImageDto)
                    }

                    setSnackBarMessage("Description saved")
                }
            } catch (error) {
                setSnackBarMessage("Error updating description")
            }
        }, 1000)

        setDebounceTimeout(timeout)
    }

    const handleDelete = async (imageId: string) => {
        try {
            if (projectId) {
                const imageService = getImageService()
                const image = gallery.find((img) => img.imageId === imageId)
                if (image) {

                    if (caseId) {
                        await imageService.deleteCaseImage(projectId, caseId, image.imageId)
                    } else {
                        await imageService.deleteProjectImage(projectId, image.imageId)
                    }

                    setGallery(gallery.filter((img) => img.imageId !== imageId))
                    setExeededLimit(false)
                } else {
                    console.error("Image not found for the provided URL:", imageId)
                }
            }
        } catch (error) {
            console.error("Error deleting image:", error)
        }
    }

    const handleExpand = (image: string) => {
        setExpandedImage(image)
        setModalOpen(true)
    }

    return gallery.length > 0 || (editMode && !isEditDisabled) ? (
        <Grid item xs={12}>
            <ImageModal image={expandedImage} modalOpen={modalOpen} setModalOpen={setModalOpen} />
            <GalleryLabel $warning={exeededLimit}>
                {gallery.length > 0 && (!editMode || isEditDisabled) && "Gallery"}
                {editMode && !isEditDisabled && `Gallery (${gallery.length} / 4)`}
            </GalleryLabel>
            <Wrapper>
                {gallery.map((image) => (
                    <GalleryImage
                        key={image.imageId}
                        image={image}
                        editAllowed={editMode && !isEditDisabled}
                        onDelete={handleDelete}
                        onExpand={handleExpand}
                        onDescriptionChange={handleDescriptionChange}
                    />
                ))}
                {editMode && !isEditDisabled && gallery.length < 4 && (
                    <ImageUpload gallery={gallery} setGallery={setGallery} setExeededLimit={setExeededLimit} />
                )}
            </Wrapper>
        </Grid>
    ) : null
}

export default Gallery
