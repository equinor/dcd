import React from "react"
import styled from "styled-components"
import { Typography, Tooltip } from "@equinor/eds-core-react"
import { useCaseContext } from "../../../../Context/CaseContext"
import { useAppContext } from "../../../../Context/AppContext"
import HistoryButton from "../../../Buttons/HistoryButton"
import CaseEditHistory from "../../../Case/Components/CaseEditHistory"

const Header = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 10px;
    margin: 15px 0;
`

const Container = styled.div<{ $sidebarOpen: boolean }>`
    display: flex;
    flex-direction: column;
    width: 100%;
    padding: ${({ $sidebarOpen }) => ($sidebarOpen ? "0 8px 20px 8px" : "0")};
    align-items: ${({ $sidebarOpen }) => ($sidebarOpen ? "space-between" : "center")};
    border-bottom: 1px solid #DCDCDC;
`

const Content = styled.div`
    max-height: 300px;
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
    const { projectCase, caseEditsBelongingToCurrentCase } = useCaseContext()

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
                {sidebarOpen && projectCase && <CaseEditHistory caseId={projectCase.id} />}
                {caseEditsBelongingToCurrentCase?.length === 0 && <NextValue>No recent edits..</NextValue>}
            </Content>
        </Container>
    )
}

export default CurrentCaseEditHistory
