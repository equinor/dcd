import { useEffect } from "react"
import styled from "styled-components"
import { projectPath } from "../Utils/common"

const Wrapper = styled.main`
    margin:20px;
`

const Welcome = (): JSX.Element => {
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()

    useEffect(() => {
        console.log("currentContext", currentContext)
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
