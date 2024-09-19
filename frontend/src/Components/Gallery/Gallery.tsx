import { useEffect, useState } from "react"
import { Box, Grid } from "@mui/material"
import styled from "styled-components"
import { Icon, Button, Typography } from "@equinor/eds-core-react"
import { delete_to_trash, expand_screen } from "@equinor/eds-icons"
import { useParams } from "react-router-dom"
import ImageUpload from "./ImageUpload"
import ImageModal from "./ImageModal"
import { useAppContext } from "../../Context/AppContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { getImageService } from "../../Services/ImageService"

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
    const { editMode } = useAppContext()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { caseId } = useParams()
    const { project } = useProjectContext()

    useEffect(() => {
        const loadImages = async () => {
            if (project?.id) {
                try {
                    const imageService = await getImageService()
                    if (caseId) {
                        const imageDtos = await imageService.getImages(project.id, caseId)
                        setGallery(imageDtos)
                    } else {
                        const imageDtos = await imageService.getProjectImages(project.id)
                        setGallery(imageDtos)
                    }
                } catch (error) {
                    console.error("Error loading images:", error)
                }
            }
        }

        loadImages()
    }, [project?.id, caseId])

    const handleDelete = async (imageUrl: string) => {
        try {
            if (project?.id) {
                const imageService = await getImageService()
                const image = gallery.find((img) => img.url === imageUrl)
                if (image) {
                    if (caseId) {
                        await imageService.deleteImage(project.id, image.id, caseId)
                    } else {
                        await imageService.deleteImage(project.id, image.id)
                    }
                    setGallery(gallery.filter((img) => img.url !== imageUrl))
                    setExeededLimit(false)
                } else {
                    console.error("Image not found for the provided URL:", imageUrl)
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
                            <img src={image.url} alt={`upload #${index + 1}`} />
                            <GalleryControls>
                                {editMode && (
                                    <Button variant="contained_icon" color="danger" onClick={() => handleDelete(image.url)}>
                                        <Icon size={18} data={delete_to_trash} />
                                    </Button>
                                )}
                                <Button variant="contained_icon" color="secondary" onClick={() => handleExpand(image.url)}>
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
