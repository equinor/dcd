import { useState } from "react"
import {
    Icon,
    Button,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import { arrow_drop_up, arrow_drop_down } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"

import { useAppContext } from "@/Context/AppContext"
import { Timeline, Header } from "../Sidebar"
import ArchivedCasesList from "./ArchivedCasesList"

const ArchivedCasesDetails: React.FC = () => {
    const [expandList, setExpandList] = useState(false);
    const { sidebarOpen } = useAppContext()

    return (
        <>
            <Grid item container alignItems="start" justifyContent={sidebarOpen ? "space-between" : "start"}>
                <Header>
                    <Typography variant="overline">Archived Cases</Typography>
                </Header>
                {sidebarOpen && (
                    <Grid item>
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
                <Timeline data-timeline container minHeight={"150px"} justifyContent="flex-start" alignItems="flex-start" direction="column">
                    <ArchivedCasesList />
                </Timeline>
            )}
        </>
    )
}

export default ArchivedCasesDetails
