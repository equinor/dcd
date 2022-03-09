import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

const Wrapper = styled.div`
    display: flex;
    width: 70%;
    flex-direction: column;
`

function DescriptionView() {
    return (
        <Wrapper>
            <Typography variant="h4">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua.
            </Typography>
        </Wrapper>
    )
}

export default DescriptionView
