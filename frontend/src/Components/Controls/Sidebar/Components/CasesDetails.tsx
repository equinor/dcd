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
import { Timeline, Header } from "../Sidebar"

export const FillHeightContainer = styled.div`
    height: 100%;
`

const CasesDetails: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { addNewCase } = useModalContext()

    return (
        <FillHeightContainer>
            <Grid item xs={12} container alignItems="center" justifyContent={sidebarOpen ? "space-between" : "start"}>
                <Header>
                    <Typography variant="overline">Cases</Typography>
                </Header>
                {sidebarOpen && (
                    <Grid item>
                        <Tooltip title="Add new case">
                            <Button variant="ghost_icon" className="GhostButton" onClick={() => addNewCase()}><Icon data={add} /></Button>
                        </Tooltip>
                    </Grid>
                )}
            </Grid>
            <Timeline data-timeline container maxHeight={"350px"} justifyContent="flex-start" alignItems="flex-start" direction="column">
                <CasesList />
            </Timeline>
        </FillHeightContainer>
    )
}

export default CasesDetails
