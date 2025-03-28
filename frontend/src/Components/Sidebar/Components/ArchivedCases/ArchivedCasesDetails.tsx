import {
    Icon,
    Button,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import { arrow_drop_up, arrow_drop_down } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid2"
import { useState } from "react"
import styled from "styled-components"

import ArchivedCasesList from "./ArchivedCasesList"

import { Timeline, Header } from "@/Components/Sidebar/SidebarWrapper"
import { useAppStore } from "@/Store/AppStore"

const ClickableTitle = styled.div`
    cursor: pointer;
`

const ArchivedCasesDetails: React.FC = () => {
    const [expandList, setExpandList] = useState(false)
    const { sidebarOpen } = useAppStore()

    return (
        <>
            <Grid container alignItems="start" justifyContent={sidebarOpen ? "space-between" : "start"}>
                <Header>
                    {sidebarOpen ? (
                        <ClickableTitle onClick={() => setExpandList(!expandList)}>
                            <Typography variant="overline">Archived Cases</Typography>
                        </ClickableTitle>
                    ) : (
                        <Tooltip placement="right" title="Expand Archived Cases">
                            <ClickableTitle onClick={() => setExpandList(!expandList)}>
                                <Typography variant="overline">Archived</Typography>
                            </ClickableTitle>
                        </Tooltip>
                    )}
                </Header>
                {sidebarOpen && (
                    <Grid>
                        {!expandList ? (
                            <Tooltip title="Expand Archived Cases">
                                <Button variant="ghost_icon" className="GhostButton"><Icon data={arrow_drop_down} onClick={() => setExpandList(true)} /></Button>
                            </Tooltip>
                        ) : (
                            <Tooltip title="Collapse Archived Cases">
                                <Button variant="ghost_icon" className="GhostButton"><Icon data={arrow_drop_up} onClick={() => setExpandList(false)} /></Button>
                            </Tooltip>
                        )}
                    </Grid>
                )}
            </Grid>
            {!expandList ? null : (
                <Timeline data-timeline container minHeight="100px" justifyContent="flex-start" alignItems="flex-start" direction="column">
                    <ArchivedCasesList />
                </Timeline>
            )}
        </>
    )
}

export default ArchivedCasesDetails
