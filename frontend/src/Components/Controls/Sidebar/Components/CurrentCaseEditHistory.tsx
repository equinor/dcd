import React from "react"
import styled from "styled-components"
import { Typography, Icon, Button } from "@equinor/eds-core-react"
import { arrow_forward, arrow_drop_down, arrow_drop_up } from "@equinor/eds-icons"
import { useCaseContext } from "../../../../Context/CaseContext"
import { formatTime, getCurrentEditId } from "../../../../Utils/common"
import { useAppContext } from "../../../../Context/AppContext"
import HistoryButton from "../../../Buttons/HistoryButton"
import CaseEditHistory from "../../../Case/Components/CaseEditHistory"

const Header = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 10px;
    margin-bottom: 5px;
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

const EditInstance = styled.div<{ $isActive: boolean }>`
    padding: 10px 5px 10px 15px;
    border-left: 2px solid ${({ $isActive }) => ($isActive ? "#007079" : "#DCDCDC")};
`

const PreviousValue = styled(Typography)`
    color: red;
    text-decoration: line-through;
    opacity: 0.5;
    max-width: 100px;
    font-size: 12px;
`

const NextValue = styled(Typography)`
    max-width: 100px;
    font-size: 12px;
`

const ChangeView = styled.div`
    display: flex;
    flex-wrap: nowrap;
    gap: 10px;
    align-items: center;

    & > p {
        white-space: wrap;
        overflow: hidden;
    }
`

const CurrentCaseEditHistory: React.FC = () => {
    const { caseEdits, projectCase } = useCaseContext()
    const editsBelongingToCurrentCase = projectCase && caseEdits.filter((edit) => edit.objectId === projectCase.id)

    const {
        editHistoryIsActive,
        setEditHistoryIsActive,
        sidebarOpen,
    } = useAppContext()
    if (!projectCase) { return null }
    return (
        <Container $sidebarOpen={sidebarOpen}>
            {!sidebarOpen ? <HistoryButton /> : (
                <Header>
                    <Typography variant="overline">Edit history</Typography>
                    <Button variant="ghost_icon" onClick={() => setEditHistoryIsActive(!editHistoryIsActive)}>
                        <Icon size={16} data={editHistoryIsActive ? arrow_drop_up : arrow_drop_down} />
                    </Button>
                </Header>
            )}
            <Content>
                {sidebarOpen && projectCase && editHistoryIsActive && <CaseEditHistory caseId={projectCase.id} />}
                {editHistoryIsActive && editsBelongingToCurrentCase?.length === 0 && <NextValue>No recent edits..</NextValue>}
            </Content>
        </Container>
    )
}

export default CurrentCaseEditHistory
