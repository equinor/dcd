import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Typography } from "@equinor/eds-core-react"
import Classification from "./Classification"

const Wrapper = styled.div`
    background-color: white;
    padding: 20px;
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

const ProjectControls = () => {
    const { currentContext } = useModuleCurrentContext()

    return (
        <Wrapper>
            <Typography variant="h4">
                {currentContext?.title}
            </Typography>
            <Classification />
        </Wrapper>
    )
}

export default ProjectControls
