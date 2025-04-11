import {
    Icon,
    Button,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { Stack } from "@mui/material"
import styled from "styled-components"

import CasesList from "./CasesList"

import { Header } from "@/Components/Sidebar/SidebarWrapper"
import { sharedTimelineStyles } from "@/Components/Sidebar/sharedStyles"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { useModalContext } from "@/Store/ModalContext"

export const CasesTimeline = styled(Stack)`
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
    const { sidebarOpen } = useAppStore()
    const { addNewCase } = useModalContext()
    const { isEditDisabled } = useCanUserEdit()

    return (
        <>
            <Stack
                direction="row"
                justifyContent={sidebarOpen ? "space-between" : "center"}
            >
                <Header>
                    <Typography variant="overline">Cases</Typography>
                </Header>
                {sidebarOpen && !isEditDisabled && (
                    <Tooltip title="Create a new case">
                        <Button variant="ghost_icon" className="GhostButton" onClick={() => addNewCase()}><Icon data={add} /></Button>
                    </Tooltip>
                )}
            </Stack>
            <GrowBox>
                <CasesTimeline
                    id="casesList"
                    data-timeline
                    direction="column"
                    alignItems="flex-start"
                >
                    <CasesList />
                </CasesTimeline>
            </GrowBox>
        </>
    )
}

export default CasesDetails
