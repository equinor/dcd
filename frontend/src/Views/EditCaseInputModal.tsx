/* eslint-disable @typescript-eslint/no-unused-vars */
import { FunctionComponent, useState } from "react"
import { FocusOn } from "react-focus-on"
import styled from "styled-components"
import {
    Button, Icon, Tabs, Typography,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import { Portal } from "../Components/Portal"
import ProductionProfilesTab from "./ProductionProfilesTab"
import { Case } from "../models/case/Case"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const TransparentButton = styled(Button)`
    color: #007079;
    background-color: white;
    border: 1px solid #007079;
`

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const ButtonsDiv = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: end;
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
    toggleEditCaseModal: any
    caseItem: Case | undefined
    isOpen: boolean;
    shards: any[];
}

export const EditCaseInputModal: FunctionComponent<Props> = ({
    toggleEditCaseModal, caseItem, isOpen, shards,
}) => {
    const [activeTab, setActiveTab] = useState<number>(0)
    const onMoreClick = (target: any) => {

    }

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
            <FocusOn shards={shards}>
                <ModalDiv>
                    <TopWrapper>
                        <Typography variant="h2">Edit case input</Typography>
                        <InvisibleButton
                            onClick={(e) => toggleEditCaseModal()}
                        >
                            <Icon
                                color="gray"
                                data={clear}
                            />
                        </InvisibleButton>
                    </TopWrapper>
                    <Tabs activeTab={activeTab} onChange={setActiveTab}>
                        <List>
                            <Tab>PROSP </Tab>
                            <Tab>Production Profiles </Tab>
                        </List>
                        <Panels>
                            <StyledTabPanel>
                                <p> PROSP</p>
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <ProductionProfilesTab caseItem={caseItem} />
                            </StyledTabPanel>
                        </Panels>
                    </Tabs>
                    <ButtonsDiv>
                        <TransparentButton
                            style={{ marginRight: "1.5rem" }}
                            onClick={(e) => toggleEditCaseModal()}
                        >
                            Cancel
                        </TransparentButton>
                        <Button
                            onClick={() => console.log("Save clicked")}
                        >
                            Save

                        </Button>
                    </ButtonsDiv>
                </ModalDiv>
            </FocusOn>
        </>
    )
}
