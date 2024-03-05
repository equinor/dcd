import React from "react"
import styled from "styled-components"
import { Switch, Typography } from "@equinor/eds-core-react"
import { useAppContext } from "../Context/AppContext"
import { useModalContext } from "../Context/ModalContext"

const Wrapper = styled.div`
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
    const { caseModalEditMode, setCaseModalEditMode } = useModalContext()

    return (
        <Wrapper>
            <Typography variant="h4">
                {project?.name}
            </Typography>
            <Controls>
                <Switch
                    label="Edit mode"
                    checked={caseModalEditMode}
                    onChange={() => setCaseModalEditMode(!caseModalEditMode)}
                />
            </Controls>
        </Wrapper>
    )
}

export default ProjectControls
