/* eslint-disable jsx-a11y/media-has-caption */
import React from "react"
import styled from "styled-components"

interface VideoPlayerProps {
    src: string;
}

const VideoContainer = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 20px 0;
    width: 100%;
`

const StyledVideo = styled.video`
    width: 100%;
    height: auto;
`

const VideoPlayer: React.FC<VideoPlayerProps> = ({ src }) => (
    <VideoContainer>
        <StyledVideo controls>
            <source src={src} type="video/mp4" />
            Your browser does not support the video tag.
        </StyledVideo>
    </VideoContainer>
)

export default VideoPlayer
