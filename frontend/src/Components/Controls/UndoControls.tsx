import React, { useState } from "react"
import {
    Typography, Button, Icon, Tooltip,
} from "@equinor/eds-core-react"
import { redo, undo } from "@equinor/eds-icons"
import styled from "styled-components"

const Container = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
   margin: 0 15px;
`

const UndoControls: React.FC = () => {
    const [isSaving, setIsSaving] = useState(false)

    const pretendToSave = async () => {
        setIsSaving(true)
        setTimeout(() => {
            setIsSaving(false)
        }, 2000)
    }

    return (
        <Container>
            {
                isSaving
                    ? <Typography variant="caption">Saving...</Typography>
                    : <Typography variant="caption">Saved</Typography>
            }
            <Tooltip title="Undo">
                <Button
                    variant="ghost_icon"
                    onClick={pretendToSave}
                >
                    <Icon data={undo} />
                </Button>
            </Tooltip>
            <Tooltip title="Redo">
                <Button
                    variant="ghost_icon"
                    onClick={pretendToSave}
                >
                    <Icon data={redo} />
                </Button>
            </Tooltip>
        </Container>
    )
}

export default UndoControls
