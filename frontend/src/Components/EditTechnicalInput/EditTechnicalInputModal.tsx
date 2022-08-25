import {
    Dispatch, FunctionComponent, SetStateAction, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Tabs, Typography,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import WellCostsTab from "./WellCostsTab"
import { Project } from "../../models/Project"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const InvisibleButton = styled(Button)`
    border: 1px solid ;
    background-color: transparent;
`

const ModalDiv = styled.div`
    height: 90%;
    width: 90%;
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 50px;
    z-index: 1000;
    background-color: white;
    border: 2px solid gray;
`

type Props = {
    toggleEditTechnicalInputModal: any
    isOpen: boolean
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
}

export const EditTechnicalInputModal: FunctionComponent<Props> = ({
    toggleEditTechnicalInputModal, isOpen, setProject, project,
}) => {
    const [activeTab, setActiveTab] = useState<number>(0)

    if (!isOpen) return null
    return (
        <>
            <div style={{
                position: "fixed",
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                backgroundColor: "rgba(0,0,0, .7)",
                zIndex: 1000,
            }}
            />
            <ModalDiv>
                <TopWrapper>
                    <Typography variant="h2">Edit Technical Input</Typography>
                    <InvisibleButton
                        onClick={() => toggleEditTechnicalInputModal()}
                    >
                        <Icon
                            color="gray"
                            data={clear}
                        />
                    </InvisibleButton>
                </TopWrapper>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>Well Costs</Tab>
                        <Tab>PROSP</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <WellCostsTab project={project} setProject={setProject} />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            PROSP
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
            </ModalDiv>
        </>
    )
}
