import { useEffect, useState } from "react"
import { Grid } from "@mui/material"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"

import ImageUpload from "./ImageUpload"
import ImageModal from "./ImageModal"
import GalleryImage from "./GalleryImage"
import { useAppContext } from "@/Context/AppContext"
import { getImageService } from "@/Services/ImageService"
import { useProjectContext } from "@/Context/ProjectContext"

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
    const { editMode, setSnackBarMessage } = useAppContext()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { caseId } = useParams()
    const { revisionId } = useParams()
    const { projectId } = useProjectContext()

    const [debounceTimeout, setDebounceTimeout] = useState<NodeJS.Timeout | null>(null)

    useEffect(() => {
        const loadImages = async () => {
            const projectIdOrRevisionId = revisionId || projectId
            if (projectId) {
                try {
                    const imageService = await getImageService()
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
    }, [projectId, caseId, setSnackBarMessage])

    const handleDescriptionChange = async (imageId: string, newDescription: string) => {
        if (debounceTimeout) {
            clearTimeout(debounceTimeout)
        }
        setGallery((prevGallery) => prevGallery.map((image) => (image.imageId === imageId ? { ...image, description: newDescription } : image)))

        const timeout = setTimeout(async () => {
            try {
                const imageService = await getImageService()
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
                const imageService = await getImageService()
                const image = gallery.find((img) => img.imageId === imageId)
                if (image) {
                    await imageService.deleteImage(projectId, image.imageId)
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

    return gallery.length > 0 || editMode ? (
        <Grid item xs={12}>
            <ImageModal image={expandedImage} modalOpen={modalOpen} setModalOpen={setModalOpen} />
            <GalleryLabel $warning={exeededLimit}>
                {gallery.length > 0 && !editMode && "Gallery"}
                {editMode && `Gallery (${gallery.length} / 4)`}
            </GalleryLabel>
            <Wrapper>
                {gallery.map((image) => (
                    <GalleryImage
                        key={image.imageId}
                        image={image}
                        editMode={editMode}
                        onDelete={handleDelete}
                        onExpand={handleExpand}
                        onDescriptionChange={handleDescriptionChange}
                    />
                ))}
                {editMode && gallery.length < 4 && (
                    <ImageUpload gallery={gallery} setGallery={setGallery} setExeededLimit={setExeededLimit} />
                )}
            </Wrapper>
        </Grid>
    ) : null
}

export default Gallery
