import React from "react"
import {
    Button,
    SideSheet,
    Icon,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import styled from "styled-components"
import { arrow_forward, history } from "@equinor/eds-icons"
import { useCaseContext } from "../../Context/CaseContext"

const EditInstance = styled.div`
    margin: 20px 15px 20px 0;
`

const Header = styled.div`
    display: flex;
    justify-content: space-between;
    gap: 10px;
`

const PreviousValue = styled(Typography)`
    color: red;
    text-decoration: line-through;
    opacity: 0.5;
`

const ChangeView = styled.div`
    display: flex;
    flex-wrap: nowrap;
    gap: 10px;
    align-items: center;
`
const StyledSideSheet = styled(SideSheet)`
    padding-right: 0;
`
const SheetBody = styled.div`
    overflow-y: auto;
    height: 100%;
`
const EditsSideBar: React.FC = () => {
    const { caseEdits, projectCase } = useCaseContext()
    const [toggleSidesheet, setToggleSidesheet] = React.useState<boolean>(false)

    return (
        <>
            <Tooltip title="See edit history">
                <Button variant="ghost_icon" onClick={() => setToggleSidesheet(true)}>
                    <Icon data={history} />
                </Button>
            </Tooltip>
            <StyledSideSheet
                title="Edits"
                open={toggleSidesheet}
                onClose={() => setToggleSidesheet(false)}
                variant="large"
            >
                <SheetBody>
                    {projectCase && caseEdits.map((edit) => (edit.level === "case" && edit.objectId === projectCase.id ? (
                        <EditInstance key={edit.uuid} style={{ marginBottom: "10px" }}>
                            <Header>
                                <Typography variant="caption">{String(edit.inputLabel)}</Typography>
                                <Typography variant="overline">{edit.timeStamp}</Typography>
                            </Header>
                            <ChangeView>
                                <PreviousValue>{edit.previousValue}</PreviousValue>
                                <div>
                                    <Icon data={arrow_forward} size={16} />
                                </div>
                                <Typography>{edit.newValue}</Typography>
                            </ChangeView>
                        </EditInstance>
                    ) : null))}
                </SheetBody>

            </StyledSideSheet>
        </>
    )
}

export default EditsSideBar
