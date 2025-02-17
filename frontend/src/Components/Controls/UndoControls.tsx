import React, { useEffect } from "react"
import {
    Typography, Icon, Tooltip, CircularProgress,
} from "@equinor/eds-core-react"
import { check_circle_outlined } from "@equinor/eds-icons"
import styled from "styled-components"
import { useAppStore } from "../../Store/AppStore"

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
    const { isSaving } = useAppStore()

    // Mock functions - will be reimplemented later
    const canUndo = () => false
    const canRedo = () => false
    const undoEdit = () => { }
    const redoEdit = () => { }

    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            const isMac = navigator.userAgent.indexOf("Mac OS X") !== -1
            const undoKey = isMac ? event.metaKey && event.key === "z" : event.ctrlKey && event.key === "z"
            const redoKey = isMac
                ? event.metaKey && event.shiftKey && event.key === "z"
                : event.ctrlKey && event.key === "y"

            if (undoKey && canUndo()) {
                event.preventDefault()
                undoEdit()
            } else if (redoKey && canRedo()) {
                event.preventDefault()
                redoEdit()
            }
        }

        window.addEventListener("keydown", handleKeyDown)
        return () => window.removeEventListener("keydown", handleKeyDown)
    }, [])

    return (
        <Container>
            {
                isSaving
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
            {/* Undo/Redo buttons temporarily disabled
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
