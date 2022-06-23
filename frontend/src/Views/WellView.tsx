import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

function WellView() {
    return (
        <Wrapper>
            <Typography variant="h3">Implementation of Wellview in progress</Typography>
        </Wrapper>
    )
}

export default WellView
