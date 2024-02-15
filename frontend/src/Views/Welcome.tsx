import { useEffect } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useNavigate } from "react-router"
import { projectPath } from "../Utils/common"

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
        <main>
            <h1>Hello</h1>
            <p>
                Welcome to ConceptApp. Please begin by searching for a project.
            </p>
        </main>
    )
}

export default Welcome
