import {
    Icon,
    Button,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import styled from "styled-components"
import { add } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"

import { useModalContext } from "@/Context/ModalContext"
import { useAppContext } from "@/Context/AppContext"
import CasesList from "../Components/CasesList"
import { sharedTimelineStyles } from "../sharedStyles"
import { Header } from "../Sidebar"
import useEditDisabled from "@/Hooks/useEditDisabled"

export const CasesTimeline = styled(Grid)`
    height: 100%;
    ${sharedTimelineStyles}
`

const GrowBox = styled.div`
    flex-grow: 1;
    flex-shrink: 1;
    overflow-y: auto;
    min-height: 100px;
`

const CasesDetails: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { addNewCase } = useModalContext()
    const { isEditDisabled } = useEditDisabled()

    return (
        <>
            <Grid item container alignItems="flex-start" justifyContent={sidebarOpen ? "space-between" : "start"}>
                <Header>
                    <Typography variant="overline">Cases</Typography>
                </Header>
                {sidebarOpen && !isEditDisabled && (
                    <Grid item>
                        <Tooltip title="Add new case">
                            <Button variant="ghost_icon" className="GhostButton" onClick={() => addNewCase()}><Icon data={add} /></Button>
                        </Tooltip>
                    </Grid>
                )}
            </Grid>
            <GrowBox>
                <CasesTimeline data-timeline container justifyContent="flex-start" alignItems="flex-start" direction="column">
                    <CasesList />
                </CasesTimeline>
            </GrowBox>
        </>
    )
}

export default CasesDetails
