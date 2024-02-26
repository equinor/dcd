import React from "react"
import styled from "styled-components"
import {
    Switch, Typography,
} from "@equinor/eds-core-react"
import { useAppContext } from "../Context/AppContext"
import { useModalContext } from "../Context/ModalContext"

const Wrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 2;
    background-color: white;
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
    const { project } = useAppContext()
    const { editMode, setEditMode } = useModalContext()

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
