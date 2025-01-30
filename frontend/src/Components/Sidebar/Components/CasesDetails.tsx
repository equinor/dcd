import {
    Icon,
    Button,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import styled from "styled-components"
import { add } from "@equinor/eds-icons"
import { Stack } from "@mui/material"

import { useModalContext } from "@/Context/ModalContext"
import { useAppContext } from "@/Context/AppContext"
import CasesList from "./CasesList"
import { sharedTimelineStyles } from "../sharedStyles"
import { Header } from "../Sidebar"
import useEditDisabled from "@/Hooks/useEditDisabled"

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
    const { sidebarOpen } = useAppContext()
    const { addNewCase } = useModalContext()
    const { isEditDisabled } = useEditDisabled()

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
                    <Tooltip title="Add new case">
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
