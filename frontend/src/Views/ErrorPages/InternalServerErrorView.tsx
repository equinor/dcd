import styled from "styled-components"
import { Button, Typography } from "@equinor/eds-core-react"

const Wrapper = styled.div`
    display: flex;
    width: 100%;
    height: 100vh;
    flex-direction: column;
    justify-content: center;
    align-items: center;
`
const TextWrapper = styled.div`
    display: flex;
    flex-direction: column;
    margin-top: 1.5rem;
    margin-bottom: 1.5rem;
    justify-content: center;
    align-items: center;
`

function InternalServerErrorView() {
    return (
        <Wrapper>
            <Typography variant="h2"><strong>We are sorry ...</strong></Typography>
            <TextWrapper>
                <Typography variant="h5">The app is having some technical difficulties.</Typography>
                <Typography variant="h5">Please contact system administrator.</Typography>
            </TextWrapper>
        </Wrapper>
    )
}

export default InternalServerErrorView
