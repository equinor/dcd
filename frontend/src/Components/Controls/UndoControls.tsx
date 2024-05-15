import React, { useState, useCallback } from "react"
import {
    Typography, Button, Icon, Tooltip,
} from "@equinor/eds-core-react"
import { redo, undo } from "@equinor/eds-icons"
import styled from "styled-components"
import { debounce } from "lodash"
import useDataEdits from "../../Hooks/useDataEdits"

const Container = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
   margin: 0 15px;
`

const UndoControls: React.FC = () => {
    const [isSaving, setIsSaving] = useState(false)
    const { undoEdit, redoEdits } = useDataEdits()

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
                    onClick={undoEdit}
                >
                    <Icon data={undo} />
                </Button>
            </Tooltip>
            <Tooltip title="Redo">
                <Button
                    variant="ghost_icon"
                    onClick={redoEdits}
                >
                    <Icon data={redo} />
                </Button>
            </Tooltip>
        </Container>
    )
}

export default UndoControls
