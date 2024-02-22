import React from "react"
import styled from "styled-components"
import {
    Switch, Typography,
} from "@equinor/eds-core-react"
import { useAppContext } from "../Context/AppContext"

const Wrapper = styled.div`
    padding: 20px;
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

const Controls = styled.div`
    display: flex;
    flex-direction: row;
    gap: 5px;
    align-items: center;


`

const ProjectControls = () => {
    const {
        project, editMode, setEditMode,
    } = useAppContext()

    return (
        <Wrapper>
            <Typography variant="h4">
                {project?.name}
            </Typography>
            <Controls>
                <Switch
                    label="Edit mode"
                    checked={editMode}
                    onChange={() => setEditMode(!editMode)}
                />
            </Controls>
        </Wrapper>
    )
}

export default ProjectControls
