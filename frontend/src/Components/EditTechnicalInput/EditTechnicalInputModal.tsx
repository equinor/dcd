/* eslint-disable max-len */
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Progress, Tabs, Typography,
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
import CO2Tab from "./CO2Tab"
import { GetProjectService } from "../../Services/ProjectService"
import { GetTechnicalInputService } from "../../Services/TechnicalInputService"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { WellProject } from "../../models/assets/wellproject/WellProject"

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
    margin-right: 1rem;
`

type Props = {
    toggleEditTechnicalInputModal: () => void
    isOpen: boolean
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project,
    setWells?: Dispatch<SetStateAction<Well[] | undefined>>
    caseId?: string
    setExploration?: Dispatch<SetStateAction<Exploration | undefined>>
    setWellProject?: Dispatch<SetStateAction<WellProject | undefined>>
}

const EditTechnicalInputModal = ({
    toggleEditTechnicalInputModal, isOpen, setProject, project, setWells, caseId, setExploration, setWellProject,
}: Props) => {
    const [activeTab, setActiveTab] = useState<number>(0)

    const [originalProject, setOriginalProject] = useState<Project>(project)

    const [explorationOperationalWellCosts, setExplorationOperationalWellCosts] = useState<ExplorationOperationalWellCosts | undefined>(project.explorationWellCosts)
    const [developmentOperationalWellCosts, setDevelopmentOperationalWellCosts] = useState<DevelopmentOperationalWellCosts | undefined>(project.developmentWellCosts)

    const [originalExplorationOperationalWellCosts, setOriginalExplorationOperationalWellCosts] = useState<ExplorationOperationalWellCosts | undefined>(project.explorationWellCosts)
    const [originalDevelopmentOperationalWellCosts, setOriginalDevelopmentOperationalWellCosts] = useState<DevelopmentOperationalWellCosts | undefined>(project.developmentWellCosts)

    const [wellProjectWells, setWellProjectWells] = useState<Well[]>(project?.wells?.filter((w) => !IsExplorationWell(w)) ?? [])
    const [explorationWells, setExplorationWells] = useState<Well[]>(project?.wells?.filter((w) => IsExplorationWell(w)) ?? [])

    const [originalWellProjectWells, setOriginalWellProjectWells] = useState<Well[]>(project?.wells?.filter((w) => !IsExplorationWell(w)) ?? [])
    const [originalExplorationWells, setOriginalExplorationWells] = useState<Well[]>(project?.wells?.filter((w) => IsExplorationWell(w)) ?? [])

    const [isSaving, setIsSaving] = useState<boolean>()

    useEffect(() => {
        if (project.wells) {
            setWellProjectWells(project.wells.filter((w) => !IsExplorationWell(w)))
            setExplorationWells(project.wells.filter((w) => IsExplorationWell(w)))

            const originalWellProjectWellsResult = structuredClone(project.wells.filter((w) => !IsExplorationWell(w)))
            setOriginalWellProjectWells(originalWellProjectWellsResult)
            const originalExplorationWellsResult = structuredClone(project.wells.filter((w) => IsExplorationWell(w)))
            setOriginalExplorationWells(originalExplorationWellsResult)
        }
    }, [project])

    if (!isOpen) return null

    if (!developmentOperationalWellCosts || !explorationOperationalWellCosts) {
        return null
    }

    const setExplorationWellProjectWellsFromWells = (wells: Well[]) => {
        const filteredExplorationWellsResult = wells.filter((w: Well) => IsExplorationWell(w))
        const filteredWellProjectWellsResult = wells.filter((w: Well) => !IsExplorationWell(w))
        setWellProjectWells(filteredWellProjectWellsResult)
        setExplorationWells(filteredExplorationWellsResult)

        setOriginalWellProjectWells(filteredWellProjectWellsResult)
        setOriginalExplorationWells(filteredExplorationWellsResult)
        if (setWells) {
            setWells(wells)
        }
    }

    const handleSave = async () => {
        try {
            const dto: Components.Schemas.TechnicalInputDto = {}
            setIsSaving(true)
            dto.projectDto = Project.Copy(project)
            if (!(JSON.stringify(project) === JSON.stringify(originalProject))) {
                dto.projectDto.hasChanges = true
            }

            dto.explorationOperationalWellCostsDto = explorationOperationalWellCosts
            if (!(JSON.stringify(explorationOperationalWellCosts) === JSON.stringify(originalExplorationOperationalWellCosts))) {
                dto.explorationOperationalWellCostsDto.hasChanges = true
            }

            dto.developmentOperationalWellCostsDto = developmentOperationalWellCosts
            if (!(JSON.stringify(developmentOperationalWellCosts) === JSON.stringify(originalDevelopmentOperationalWellCosts))) {
                dto.developmentOperationalWellCostsDto.hasChanges = true
            }

            dto.wellDtos = [...explorationWells, ...wellProjectWells]
            const originalWells = [...originalExplorationWells, ...originalWellProjectWells]
            if (dto.wellDtos?.length > 0) {
                dto.wellDtos.forEach((wellDto, index) => {
                    if (wellDto.id !== EMPTY_GUID) {
                        const originalWell = originalWells.find((ow) => ow.id === wellDto.id)
                        if (!(JSON.stringify(wellDto) === JSON.stringify(originalWell))) {
                            dto.wellDtos![index].hasChanges = true
                        }
                    }
                })
            }

            if (caseId && caseId !== "") {
                dto.caseId = caseId
            }

            const result = await (await GetTechnicalInputService()).update(dto)

            if (result.projectDto) {
                setProject(Project.fromJSON(result.projectDto))
            }

            if (result.explorationDto && setExploration) {
                setExploration(Exploration.fromJSON(result.explorationDto))
            }
            if (result.wellProjectDto && setWellProject) {
                setWellProject(WellProject.fromJSON(result.wellProjectDto))
            }

            const explorationOperationalWellCostsResult = result.explorationOperationalWellCostsDto
            setExplorationOperationalWellCosts(explorationOperationalWellCosts)
            setOriginalExplorationOperationalWellCosts(explorationOperationalWellCosts)

            const developmentOperationalWellCostsResult = result.developmentOperationalWellCostsDto
            setDevelopmentOperationalWellCosts(developmentOperationalWellCosts)
            setOriginalDevelopmentOperationalWellCosts(developmentOperationalWellCosts)

            if (result.wellDtos) {
                setExplorationWellProjectWellsFromWells(result.wellDtos)
            }

            setIsSaving(false)
            toggleEditTechnicalInputModal()
        } catch (error) {
            console.error("Error when saving technical input: ", error)
        } finally {
            setIsSaving(false)
            toggleEditTechnicalInputModal()
        }
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
                    <Button
                        onClick={toggleEditTechnicalInputModal}
                        variant="ghost"
                    >
                        <Icon
                            color="gray"
                            data={clear}
                        />
                    </Button>
                </TopWrapper>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>Well Costs</Tab>
                        <Tab>PROSP</Tab>
                        <Tab>CO2</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <WellCostsTab
                                project={project}
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
                        <StyledTabPanel>
                            <CO2Tab project={project} setProject={setProject} />
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
                        {!isSaving ? <Button onClick={handleSave}>Save and close</Button>
                            : (
                                <Button>
                                    <Progress.Dots />
                                </Button>
                            )}
                    </ButtonsWrapper>
                </Wrapper>
            </ModalDiv>
        </>
    )
}

export default EditTechnicalInputModal
