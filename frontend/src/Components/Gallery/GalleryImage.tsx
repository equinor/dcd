import { useEffect, useRef, useState } from "react"
import styled from "styled-components"
import { Box } from "@mui/material"
import {
    Icon,
    Button,
    Input,
} from "@equinor/eds-core-react"
import { delete_to_trash, expand_screen } from "@equinor/eds-icons"
import InputSwitcher from "@/Components/Input/Components/InputSwitcher"

const ImageCard = styled.div`
    display: inline-block;
    width: max-content;

    .input-container {
        p {
            width: 100%;
            max-width: inherit;
            word-wrap: break-word;
            white-space: normal;
        }
    }
`

const ImageWithHover = styled(Box)`
    position: relative;
    height: 240px;
    border-radius: 5px;
    border: 1px solid lightgray;
    display: block;
    & img {
        height: 100%;
        width: auto;
        object-fit: contain;
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

interface GalleryImageProps {
    image: Components.Schemas.ImageDto;
    editAllowed: boolean;
    onDelete: (imageId: string) => void;
    onExpand: (imageData: string) => void;
    onDescriptionChange: (imageId: string, description: string) => void;
}

const GalleryImage = ({
    image,
    editAllowed,
    onDelete,
    onExpand,
    onDescriptionChange,
}: GalleryImageProps) => {
    const [width, setWidth] = useState<number | null>(null)
    const imageRef = useRef<HTMLImageElement | null>(null)

    useEffect(() => {
        const img = imageRef.current
        if (img) {
            const updateWidth = () => {
                setWidth(img.clientWidth)
            }
            img.addEventListener("load", updateWidth)
            if (img.complete) {
                updateWidth()
            }
            return () => {
                img.removeEventListener("load", updateWidth)
                return undefined
            }
        }
        return undefined
    }, [])

    return (
        <ImageCard style={{ width: width ? `${width}px` : "auto" }}>
            <ImageWithHover>
                <img
                    ref={imageRef}
                    src={image.imageData}
                    alt={image.description || "Gallery image"}
                />
                <GalleryControls>
                    {editAllowed && (
                        <Button variant="contained_icon" color="danger" onClick={() => onDelete(image.imageId)}>
                            <Icon size={18} data={delete_to_trash} />
                        </Button>
                    )}
                    <Button variant="contained_icon" color="secondary" onClick={() => onExpand(image.imageData)}>
                        <Icon size={18} data={expand_screen} />
                    </Button>
                </GalleryControls>
            </ImageWithHover>
            <div className="input-container">
                <InputSwitcher label="Description" value={`${image.description || ""}`}>
                    <Input
                        value={image.description || ""}
                        onChange={(e: any) => onDescriptionChange(image.imageId, e.target.value)}
                    />
                </InputSwitcher>
            </div>
        </ImageCard>
    )
}

export default GalleryImage
