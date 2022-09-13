import { useCurrentUser } from "@equinor/fusion"

function Welcome(): JSX.Element {
    const currentUser = useCurrentUser()
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
