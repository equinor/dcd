import React from "react"
import styled from "styled-components"
import { Button, Typography, Icon } from "@equinor/eds-core-react"
import { lock } from "@equinor/eds-icons"

const Wrapper = styled.div`
    display: flex;
    width: 100%;
    height: 100vh;
    flex-direction: column;
    justify-content: center;
    align-items: center;
`
const StyledButton = styled(Button)`
    color: white;
    background-color: #007079;
`
const TextWrapper = styled.div`
    display: flex;
    flex-direction: column;
    margin-top: 1.5rem;
    margin-bottom: 1.5rem;
    justify-content: center;
    align-items: center;
`

const openMailToPO = () => {
    window.open(`mailto:${"test@test.com"}`)
}

function UnauthorizedView() {
    return (
        <Wrapper>
            <Icon data={lock} size={48} color="#007079" />
            <Typography variant="h2"><strong>403 - Forbidden</strong></Typography>
            <TextWrapper>
                <Typography variant="h5">The page you are trying to reach has restricted access.</Typography>
                <Typography variant="h5">To request access, please contact your local administrator.</Typography>
            </TextWrapper>
            <StyledButton
                onClick={openMailToPO}
            >
                Request Access
            </StyledButton>
        </Wrapper>
    )
}

export default UnauthorizedView
