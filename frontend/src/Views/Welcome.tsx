import { useCurrentContext, useHistory } from "@equinor/fusion"
import { useEffect } from "react"
import { projectPath } from "../Utils/common"

const Welcome = (): JSX.Element => {
    const history = useHistory()
    const currentProject = useCurrentContext()

    useEffect(() => {
        if (currentProject?.externalId) {
            history.push(projectPath(currentProject?.externalId))
        }
    }, [currentProject?.externalId])

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
