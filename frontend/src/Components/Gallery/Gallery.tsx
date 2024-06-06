import { useEffect, useState } from "react"
import { Box, Grid } from "@mui/material"
import styled from "styled-components"
import { Icon, Button, Typography } from "@equinor/eds-core-react"
import { delete_to_trash, expand_screen } from "@equinor/eds-icons"
import ImageUpload from "./ImageUpload"
import ImageModal from "./ImageModal"
import { useAppContext } from "../../Context/AppContext"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { getImageService } from "../../Services/ImageService"

const Wrapper = styled(Grid)`
    padding: 2px;
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(calc(25% - 20px), 1fr));
    justify-content: start;
    gap: 10px;
    overflow: hidden;
    margin-bottom: 30px; 
`

const ImageWithHover = styled(Box)`
    position: relative;
    & img {
        width: 100%;
        height: 200px;
        object-fit: cover;
        border-radius: 5px;
        border: 1px solid lightgray;
    }
    &:hover {
        img {
            opacity: 0.8;
            filter: blur(1px);
        }
    }
`

const Controls = styled(Box)`
    display: none;
    position: absolute;
    top: 10px;
    right: 10px;
    padding: 10px;
    gap: 5px;
    

    ${ImageWithHover}:hover & {
        display: flex;
    }
`

const GalleryLabel = styled(Typography) <{ $warning: boolean }>`
    font-size: 0.750rem;
    font-weight: 500;
    line-height: 1.333em;
    margin-left: 8px;
    margin-right: 8px;
    color: ${({ $warning }) => ($warning ? "red" : "rgba(111, 111, 111, 1)")};
`

const Gallery = () => {
    const { editMode } = useAppContext()
    const [gallery, setGallery] = useState<Components.Schemas.ImageDto[]>([])
    const [modalOpen, setModalOpen] = useState(false)
    const [expandedImage, setExpandedImage] = useState("")
    const [exeededLimit, setExeededLimit] = useState(false)
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    useEffect(() => {
        const loadImages = async () => {
            if (project?.id && projectCase?.id) {
                try {
                    const imageService = await getImageService()
                    const imageDtos = await imageService.getImages(project.id, projectCase.id)
                    setGallery(imageDtos)
                } catch (error) {
                    console.error("Error loading images:", error)
                }
            }
        }

        loadImages()
    }, [project?.id, projectCase?.id])

    const handleDelete = async (imageUrl: string) => {
        try {
            if (project?.id && projectCase?.id) {
                const imageService = await getImageService()
                const image = gallery.find((img) => img.url === imageUrl)
                if (image) {
                    await imageService.deleteImage(project.id, projectCase.id, image.id)
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
                    <ImageWithHover key={`menu-item-${index + 1}`}>
                        <img src={image.url} alt={`upload #${index + 1}`} />
                        <Controls>
                            {editMode && (
                                <Button variant="contained_icon" color="danger" onClick={() => handleDelete(image.url)}>
                                    <Icon size={18} data={delete_to_trash} />
                                </Button>
                            )}
                            <Button variant="contained_icon" color="secondary" onClick={() => handleExpand(image.url)}>
                                <Icon size={18} data={expand_screen} />
                            </Button>
                        </Controls>
                    </ImageWithHover>
                ))}
                {editMode && gallery.length < 4 && (
                    <ImageUpload gallery={gallery} setGallery={setGallery} setExeededLimit={setExeededLimit} />
                )}
            </Wrapper>
        </Grid>
    ) : null
}

export default Gallery
