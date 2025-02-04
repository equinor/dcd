import React from "react"
import styled from "styled-components"
import { Typography, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"

import { useCaseContext } from "@/Context/CaseContext"
import { useAppContext } from "@/Context/AppContext"
import HistoryButton from "@/Components/Buttons/HistoryButton"
import CaseEditHistory from "@/Components/Sidebar/Components/EditHistory/CaseEditHistory"
import { Header, StyledDivider } from "@/Components/Sidebar/SidebarWrapper"

const Container = styled.div<{ $sidebarOpen: boolean }>`
    display: flex;
    flex-direction: column;
    align-items: ${({ $sidebarOpen }) => ($sidebarOpen ? "space-between" : "center")};
`

const Content = styled.div`
    max-height: 220px;
    padding: 0 10px;
    overflow: auto;
    -ms-overflow-style: none;
    scrollbar-width: none;

    &::-webkit-scrollbar {
        display: none;
    }
`

const NextValue = styled(Typography)`
    max-width: 100px;
    font-size: 12px;
`

const CurrentCaseEditHistory: React.FC = () => {
    const { caseEditsBelongingToCurrentCase } = useCaseContext()
    const { caseId } = useParams()

    const { sidebarOpen } = useAppContext()

        return (
            <Container $sidebarOpen={sidebarOpen}>
                {!sidebarOpen ? <HistoryButton /> : (
                    <Tooltip title="Displays all edits you made to the case in the past hour" placement="right">
                        <Header>
                            <Typography variant="overline">Edit history</Typography>
                        </Header>
                    </Tooltip>
                )}
                <Content>
                    {sidebarOpen && caseId && <CaseEditHistory caseId={caseId} />}
                    {sidebarOpen && caseEditsBelongingToCurrentCase?.length === 0 && <NextValue>No recent edits..</NextValue>}
                </Content>
                <StyledDivider />
            </Container>
        )
}

export default CurrentCaseEditHistory
