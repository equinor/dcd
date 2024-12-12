import { useEffect, useState } from "react"
import { Box, Grid } from "@mui/material"
import styled from "styled-components"
import { Icon, Button, Typography } from "@equinor/eds-core-react"
import { delete_to_trash, expand_screen } from "@equinor/eds-icons"
import { useParams } from "react-router-dom"
import ImageUpload from "./ImageUpload"
import ImageModal from "./ImageModal"
import { useAppContext } from "../../Context/AppContext"
import { getImageService } from "../../Services/ImageService"
import { useProjectContext } from "../../Context/ProjectContext"

const Wrapper = styled.div`
    display: flex;
    justify-content: start;
    gap: 10px;
    margin-bottom: 30px; 
    width: 100%;
    overflow-x: auto;
    overflow-y: hidden;
    flex-wrap: wrap;
`

const ImageWithHover = styled(Box)`
    position: relative;
    height: 240px;
    border-radius: 5px;
    border: 1px solid lightgray;
    & img {
        height: 100%;
    }
    &:hover {
        img {
            opacity: 0.8;
            filter: blur(1px);
        }
    }
`

const GalleryControls = styled.div`
    display: none;
    position: absolute;
    top: 0;
    right: 0;
    gap: 5px;
    ${ImageWithHover}:hover & {
        display: flex;
    }
`

const GalleryLabel = styled(Typography) <{ $warning: boolean }>`
    font-size: 0.750rem;
    font-weight: 500;
    line-height: 1.333em;
    color: ${({ $warning }) => ($warning ? "red" : "rgba(111, 111, 111, 1)")};
`

const Gallery = () => {
    const { editMode, setSnackBarMessage } = useAppContext()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { caseId } = useParams()
    const { projectId } = useProjectContext()

    useEffect(() => {
        const loadImages = async () => {
            if (projectId) {
                try {
                    const imageService = await getImageService()
                    const imageDtos = caseId ?
                     await imageService.getCaseImages(projectId, caseId) :
                     await imageService.getProjectImages(projectId)

                    setGallery(imageDtos)
                } catch (error) {
                    console.error("Error loading images:", error)
                    setSnackBarMessage("Error loading images")
                }
            }
        }

        loadImages()
    }, [projectId, caseId, setSnackBarMessage])

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
                {gallery.map((image, index) => (
                    <div
                        key={`menu-item-${index + 1}`}
                    >
                        <ImageWithHover>
                            <img src={"data:image/jpeg;base64, " + image.imageData} alt={`upload #${index + 1}`} />
                            <GalleryControls>
                                {editMode && (
                                    <Button variant="contained_icon" color="danger" onClick={() => handleDelete(image.imageId)}>
                                        <Icon size={18} data={delete_to_trash} />
                                    </Button>
                                )}
                                <Button variant="contained_icon" color="secondary" onClick={() => handleExpand(image.imageId)}>
                                    <Icon size={18} data={expand_screen} />
                                </Button>
                            </GalleryControls>
                        </ImageWithHover>
                    </div>
                ))}
                {editMode && gallery.length < 4 && (
                    <ImageUpload gallery={gallery} setGallery={setGallery} setExeededLimit={setExeededLimit} />
                )}
            </Wrapper>
        </Grid>
    ) : null
}

export default Gallery
