import { useEffect } from "react"
import styled from "styled-components"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

const Wrapper = styled.main`
    margin:20px;
`

const Welcome = (): JSX.Element => {
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()

    useEffect(() => {
        if (currentContext?.externalId) {
            navigate(currentContext.id)
        }
    }, [currentContext])

    return (
        <Wrapper>
            <h1>Hello</h1>
            <p>
                Welcome to ConceptApp. Please begin by searching for a project.
            </p>
        </Wrapper>
    )
}

export default Welcome
