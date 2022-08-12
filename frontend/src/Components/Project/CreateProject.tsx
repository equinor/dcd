import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import { Project } from "../../models/Project"

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>,
}

const CreateProject = ({
    setProject,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {

    }

    return (
        <p>Test</p>
    )
}

export default CreateProject
