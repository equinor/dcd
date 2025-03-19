import {
    Snackbar, Typography, Button, Icon,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import styled from "styled-components"

import CreateRevisionModal from "@/Components/Modal/CreateRevisionModal"
import { useDataFetch } from "@/Hooks"
import { useAppStore } from "@/Store/AppStore"
import { useFeatureContext } from "@/Store/FeatureContext"
import { useProjectContext } from "@/Store/ProjectContext"

const SnackbarCentering = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
`

const SnackbarComponent = () => {
    const { snackBarMessage, setSnackBarMessage } = useAppStore()
    const { showRevisionReminder, setShowRevisionReminder } = useAppStore()
    const { Features } = useFeatureContext()
    const { setIsCreateRevisionModalOpen } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    function handleCreateRevision() {
        setIsCreateRevisionModalOpen(true)
        setShowRevisionReminder(false)
    }

    return (
        <>
            <Snackbar open={snackBarMessage !== undefined} autoHideDuration={6000} onClose={() => setSnackBarMessage(undefined)}>
                {snackBarMessage}
            </Snackbar>
            {Features?.revisionEnabled && (
                <>
                    <Snackbar open={showRevisionReminder} placement="bottom-right" autoHideDuration={300000000} onClose={() => setShowRevisionReminder(false)}>
                        <SnackbarCentering>
                            <Button variant="ghost_icon" onClick={() => setShowRevisionReminder(false)}>
                                <Icon data={clear} />
                            </Button>
                            <Typography variant="body_short" color="#FFF" style={{ marginLeft: "10px" }}>
                                Remember to create a new revision after completing a project phase!
                            </Typography>
                            <Snackbar.Action>
                                <Button
                                    variant="ghost"
                                    onClick={() => handleCreateRevision()}
                                    disabled={!revisionAndProjectData?.userActions.canCreateRevision}
                                >
                                    Create revision
                                </Button>
                            </Snackbar.Action>
                        </SnackbarCentering>
                    </Snackbar>
                    <CreateRevisionModal />
                </>
            )}
        </>
    )
}

export default SnackbarComponent
