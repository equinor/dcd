import React, { useState, useEffect } from "react"
import {
    Typography, Button, Icon, Tooltip,
} from "@equinor/eds-core-react"
import { redo, undo } from "@equinor/eds-icons"
import styled from "styled-components"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { getCurrentEditId } from "../../Utils/common"

const Container = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
   margin: 0 15px;
`

const UndoControls: React.FC = () => {
    const {
        caseEdits,
        projectCase,
        editIndexes,
    } = useCaseContext()

    const [isSaving, setIsSaving] = useState(false)
    const { undoEdit, redoEdit } = useDataEdits()

    const editsBelongingToCurrentCase = projectCase && caseEdits.filter((edit) => edit.objectId === projectCase.id)
    const currentEditId = getCurrentEditId(editIndexes, projectCase)

    if (!editsBelongingToCurrentCase || editsBelongingToCurrentCase.length === 0) { return null }

    const canUndo = () => {
        // If there's no current edit, return false.
        if (!currentEditId) {
            return false
        }

        const currentEditIndex = editsBelongingToCurrentCase.findIndex((edit) => edit.uuid === currentEditId)
        return currentEditIndex < editsBelongingToCurrentCase.length && currentEditIndex > -1
    }

    const canRedo = () => {
        // If there's no current edit, and there are edits belonging to the current case, return true.
        if (!currentEditId && editsBelongingToCurrentCase.length > 0) {
            return true
        }

        const currentEditIndex = editsBelongingToCurrentCase.findIndex((edit) => edit.uuid === currentEditId)
        return currentEditIndex < editsBelongingToCurrentCase.length && currentEditIndex > 0
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
                    onClick={undoEdit}
                    disabled={!canUndo()}
                >
                    <Icon data={undo} />
                </Button>
            </Tooltip>
            <Tooltip title="Redo">
                <Button
                    variant="ghost_icon"
                    onClick={redoEdit}
                    disabled={!canRedo()}
                >
                    <Icon data={redo} />
                </Button>
            </Tooltip>
        </Container>
    )
}

export default UndoControls
