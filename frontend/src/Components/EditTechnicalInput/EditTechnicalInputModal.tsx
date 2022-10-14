/* eslint-disable max-len */
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Tabs, Typography,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import WellCostsTab from "./WellCostsTab"
import { Project } from "../../models/Project"
import PROSPTab from "./PROSPTab"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { GetExplorationOperationalWellCostsService } from "../../Services/ExplorationOperationalWellCostsService"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetDevelopmentOperationalWellCostsService } from "../../Services/DevelopmentOperationalWellCostsService"
import { Well } from "../../models/Well"
import { IsExplorationWell } from "../../Utils/common"
import { GetWellService } from "../../Services/WellService"

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
    height: 80%;
    width: 90%;
    position: fixed;
    top: 80px;
    left: 3%;
    padding: 20px;
    z-index: 1000;
    background-color: white;
    border: 2px solid gray;
    overflow-y: auto;
`

const Wrapper = styled.div`
    margin-left: auto;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
`

const ButtonsWrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const CancelButton = styled(Button)`
    margin-right: 20px;
`

type Props = {
    toggleEditTechnicalInputModal: any
    isOpen: boolean
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project,
}

const EditTechnicalInputModal = ({
    toggleEditTechnicalInputModal, isOpen, setProject, project,
}: Props) => {
    const [activeTab, setActiveTab] = useState<number>(0)
    const [explorationOperationalWellCosts, setExplorationOperationalWellCosts] = useState<ExplorationOperationalWellCosts | undefined>(project.explorationWellCosts)
    const [developmentOperationalWellCosts, setDevelopmentOperationalWellCosts] = useState<DevelopmentOperationalWellCosts | undefined>(project.developmentWellCosts)
    const [wellProjectWells, setWellProjectWells] = useState<Well[] | undefined>(project?.wells?.filter((w) => !IsExplorationWell(w)))
    const [explorationWells, setExplorationWells] = useState<Well[] | undefined>(project?.wells?.filter((w) => IsExplorationWell(w)))

    useEffect(() => {
        (async () => {
            try {
                if (!project.explorationWellCosts || project.explorationWellCosts.id === EMPTY_GUID) {
                    const res = await (await GetExplorationOperationalWellCostsService()).create({ projectId: project.id })
                    console.log("Result: ", res)
                    setExplorationOperationalWellCosts(res)
                }
                if (!project.developmentWellCosts || project.developmentWellCosts.id === EMPTY_GUID) {
                    const res = await (await GetDevelopmentOperationalWellCostsService()).create({ projectId: project.id })
                    console.log("Result: ", res)
                    setDevelopmentOperationalWellCosts(res)
                }
            } catch (error) {
                console.error()
            }
        })()
    }, [])

    useEffect(() => {
        if (project.wells) {
            const a = project.wells.filter((w) => !IsExplorationWell(w))
            setWellProjectWells(project.wells.filter((w) => !IsExplorationWell(w)))
            setExplorationWells(project.wells.filter((w) => IsExplorationWell(w)))
        }
    }, [project])

    if (!isOpen) return null

    if (!developmentOperationalWellCosts || !explorationOperationalWellCosts) {
        return null
    }

    const wellsToWellsDto = (wells: Well[] | undefined): Components.Schemas.WellDto[] => {
        if (!wells || wells.length === 0) {
            return []
        }
        const wellsDto: Components.Schemas.WellDto[] = wells?.map((w) => Well.toDto(w)) ?? []
        return wellsDto
    }

    const saveWellProjectWells = async () => {
        const newWellProjectWells = wellsToWellsDto(wellProjectWells).filter((w) => w.id === EMPTY_GUID)
        const updatedWellProjectWells = wellsToWellsDto(wellProjectWells).filter((w) => w.id !== EMPTY_GUID)

        const newWellProjectWellsResult = await (await GetWellService()).createMultipleWells(newWellProjectWells)

        const wellProjectWellsResult = await (await GetWellService()).updateMultipleWells(updatedWellProjectWells)
        const filteredWellProjectWellsResult = wellProjectWellsResult.filter((w: any) => !IsExplorationWell(w))
        setWellProjectWells(filteredWellProjectWellsResult)
    }

    const saveExplorationWells = async () => {
        const newExplorationWells = wellsToWellsDto(explorationWells).filter((w) => w.id === EMPTY_GUID)
        const updatedExplorationWells = wellsToWellsDto(explorationWells).filter((w) => w.id !== EMPTY_GUID)

        const newExplorationWellsResult = await (await GetWellService()).createMultipleWells(newExplorationWells)

        const explorationWellsResult = await (await GetWellService()).updateMultipleWells(updatedExplorationWells)
        const filteredExplorationWellsResult = explorationWellsResult.filter((w: any) => IsExplorationWell(w))
        setExplorationWells(filteredExplorationWellsResult)
    }

    const handleSave = async () => {
        // if (!_.isEqual(project.explorationWellCosts, explorationOperationalWellCosts)) {
        const res1 = await (await GetExplorationOperationalWellCostsService()).update({ ...explorationOperationalWellCosts })
        setExplorationOperationalWellCosts(res1)
        // }
        // if (!_.isEqual(project.developmentWellCosts, developmentOperationalWellCosts)) {
        const res = await (await GetDevelopmentOperationalWellCostsService()).update({ ...developmentOperationalWellCosts })
        setDevelopmentOperationalWellCosts(res)
        // }

        await saveWellProjectWells()

        await saveExplorationWells()
    }

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
                        onClick={toggleEditTechnicalInputModal}
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
                            <WellCostsTab
                                project={project}
                                setProject={setProject}
                                developmentOperationalWellCosts={developmentOperationalWellCosts}
                                setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
                                explorationOperationalWellCosts={explorationOperationalWellCosts}
                                setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                                explorationWells={explorationWells}
                                setExplorationWells={setExplorationWells}
                                wellProjectWells={wellProjectWells}
                                setWellProjectWells={setWellProjectWells}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <PROSPTab project={project} setProject={setProject} />
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
                <Wrapper>
                    <ButtonsWrapper>
                        <CancelButton
                            type="button"
                            variant="outlined"
                            onClick={toggleEditTechnicalInputModal}
                        >
                            Cancel
                        </CancelButton>
                        <Button onClick={handleSave}>Save</Button>
                    </ButtonsWrapper>
                </Wrapper>
            </ModalDiv>
        </>
    )
}

export default EditTechnicalInputModal