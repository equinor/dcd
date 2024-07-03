import React, { useState, useEffect } from "react"
import {
    Typography, Button, Icon, Tooltip, CircularProgress,
} from "@equinor/eds-core-react"
import { redo, undo, check_circle_outlined } from "@equinor/eds-icons"
import styled from "styled-components"
import { useIsMutating } from "react-query"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { getCurrentEditId } from "../../Utils/common"

const Container = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
   margin: 0 15px;
`

const Status = styled.div`
   display: flex;
   align-items: center;
   gap: 5px;
`

const UndoControls: React.FC = () => {
    const {
        projectCase,
        editIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const isMutating = useIsMutating()

    const { undoEdit, redoEdit } = useDataEdits()

    const currentEditId = getCurrentEditId(editIndexes, projectCase)

    const canUndo = () => {
        if (isMutating) {
            return false
        }
        if (!currentEditId || !caseEditsBelongingToCurrentCase) {
            return false
        }

        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === currentEditId)
        return currentEditIndex < caseEditsBelongingToCurrentCase.length && currentEditIndex > -1
    }

    const canRedo = () => {
        if (isMutating) {
            return false
        }

        if (!caseEditsBelongingToCurrentCase) {
            return false
        }
        if (!currentEditId && caseEditsBelongingToCurrentCase.length > 0) {
            return true
        }

        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === currentEditId)
        return currentEditIndex < caseEditsBelongingToCurrentCase.length && currentEditIndex > 0
    }

    const [saving, setSaving] = useState(false)

    const startCountDown = () => {
        setSaving(true)
        setTimeout(() => {
            setSaving(false)
        }, 500)
    }

    useEffect(() => {
        if (isMutating) {
            startCountDown()
        }
    }, [isMutating])

    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            const isMac = navigator.userAgent.indexOf("Mac OS X") !== -1
            const undoKey = isMac ? event.metaKey && event.key === "z" : event.ctrlKey && event.key === "z"
            const redoKey = isMac ? event.metaKey && event.key === "y" : event.ctrlKey && event.key === "y"

            if (undoKey) {
                event.preventDefault()
                event.stopPropagation()
                if (canUndo()) {
                    undoEdit()
                }
            } else if (redoKey) {
                event.preventDefault()
                event.stopPropagation()
                if (canRedo()) {
                    redoEdit()
                }
            }
        }

        window.addEventListener("keydown", handleKeyDown)

        return () => {
            window.removeEventListener("keydown", handleKeyDown)
        }
    }, [undoEdit, redoEdit, canUndo, canRedo])

    return (
        <Container>
            {
                saving
                    ? (
                        <Status>
                            <CircularProgress value={0} size={16} />
                            <Typography variant="caption">saving...</Typography>
                        </Status>
                    )
                    : (
                        <Tooltip title="All changes are saved">
                            <Status>
                                <Icon data={check_circle_outlined} size={16} />
                                <Typography variant="caption">up to date</Typography>
                            </Status>
                        </Tooltip>
                    )
            }
            {/* uncomment for next release
            <Tooltip title={canUndo() ? "Undo" : "No changes to undo"}>
                <Button
                    variant="ghost_icon"
                    onClick={undoEdit}
                    disabled={!canUndo()}
                >
                    <Icon data={undo} />
                </Button>
            </Tooltip>
            <Tooltip title={canRedo() ? "Redo" : "No changes to redo"}>
                <Button
                    variant="ghost_icon"
                    onClick={redoEdit}
                    disabled={!canRedo()}
                >
                    <Icon data={redo} />
                </Button>
            </Tooltip>
            */}
        </Container>
    )
}

export default UndoControls
