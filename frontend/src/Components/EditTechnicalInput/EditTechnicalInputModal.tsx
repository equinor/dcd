import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
import {
    Button, Icon, Progress, Tabs, Typography,
} from "@equinor/eds-core-react"
import { clear } from "@equinor/eds-icons"
import WellCostsTab from "./WellCostsTab"
import PROSPTab from "./PROSPTab"
import { EMPTY_GUID } from "../../Utils/constants"
import { isExplorationWell } from "../../Utils/common"
import CO2Tab from "./CO2Tab"
import { GetTechnicalInputService } from "../../Services/TechnicalInputService"

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
    setProject: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>
    project: Components.Schemas.ProjectDto,
    caseId?: string
    setExploration?: Dispatch<SetStateAction<Components.Schemas.ExplorationDto | undefined>>
    setWellProject?: Dispatch<SetStateAction<Components.Schemas.WellProjectDto | undefined>>
}

const EditTechnicalInputModal = ({
    toggleEditTechnicalInputModal,
    isOpen,
    setProject,
    project,
    caseId,
    setExploration,
    setWellProject,
}: Props) => {
    const [activeTab, setActiveTab] = useState<number>(0)

    const [originalProject] = useState<Components.Schemas.ProjectDto>(project)

    const [explorationOperationalWellCosts, setExplorationOperationalWellCosts] = useState<Components.Schemas.ExplorationOperationalWellCostsDto>(project.explorationOperationalWellCosts)
    const [developmentOperationalWellCosts, setDevelopmentOperationalWellCosts] = useState<Components.Schemas.DevelopmentOperationalWellCostsDto>(project.developmentOperationalWellCosts)

    const [originalExplorationOperationalWellCosts, setOriginalExplorationOperationalWellCosts] = useState<Components.Schemas.ExplorationOperationalWellCostsDto>(project.explorationOperationalWellCosts)
    const [originalDevelopmentOperationalWellCosts, setOriginalDevelopmentOperationalWellCosts] = useState<Components.Schemas.DevelopmentOperationalWellCostsDto>(project.developmentOperationalWellCosts)

    const [wellProjectWells, setWellProjectWells] = useState<Components.Schemas.WellDto[]>(project?.wells?.filter((w) => !isExplorationWell(w)) ?? [])
    const [explorationWells, setExplorationWells] = useState<Components.Schemas.WellDto[]>(project?.wells?.filter((w) => isExplorationWell(w)) ?? [])

    const [originalWellProjectWells, setOriginalWellProjectWells] = useState<Components.Schemas.WellDto[]>(project?.wells?.filter((w) => !isExplorationWell(w)) ?? [])
    const [originalExplorationWells, setOriginalExplorationWells] = useState<Components.Schemas.WellDto[]>(project?.wells?.filter((w) => isExplorationWell(w)) ?? [])

    const [isSaving, setIsSaving] = useState<boolean>()

    useEffect(() => {
        if (project.wells) {
            setWellProjectWells(project.wells.filter((w) => !isExplorationWell(w)))
            setExplorationWells(project.wells.filter((w) => isExplorationWell(w)))

            const originalWellProjectWellsResult = structuredClone(project.wells.filter((w) => !isExplorationWell(w)))
            setOriginalWellProjectWells(originalWellProjectWellsResult)
            const originalExplorationWellsResult = structuredClone(project.wells.filter((w) => isExplorationWell(w)))
            setOriginalExplorationWells(originalExplorationWellsResult)
        }
    }, [project])

    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === "Escape") {
                toggleEditTechnicalInputModal()
            }
        }

        if (isOpen) {
            window.addEventListener("keydown", handleKeyDown)
        }

        return () => {
            window.removeEventListener("keydown", handleKeyDown)
        }
    }, [isOpen, toggleEditTechnicalInputModal])

    if (!isOpen) return null

    if (!developmentOperationalWellCosts || !explorationOperationalWellCosts) {
        return null
    }

    const handleSave = async () => {
        try {
            const dto: Components.Schemas.TechnicalInputDto = {}
            setIsSaving(true)
            dto.projectDto = { ...project }
            if (!(JSON.stringify(project) === JSON.stringify(originalProject))) {
                // dto.projectDto.hasChanges = true
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

            const result = await (await GetTechnicalInputService()).update(project.id, dto)

            if (result.projectDto) {
                setProject({ ...result.projectDto })
            }

            if (result.explorationDto && setExploration) {
                setExploration(result.explorationDto)
            }
            if (result.wellProjectDto && setWellProject) {
                setWellProject(result.wellProjectDto)
            }

            setOriginalExplorationOperationalWellCosts(explorationOperationalWellCosts)
            setOriginalDevelopmentOperationalWellCosts(developmentOperationalWellCosts)
            setOriginalWellProjectWells([...wellProjectWells])
            setOriginalExplorationWells([...explorationWells])

            // Reset the changes made flag as changes have been successfully saved
            setIsSaving(false) // End saving process
        } catch (error) {
            console.error("Error when saving technical input: ", error)
            setIsSaving(false)
        } finally {
            setIsSaving(false)
        }
    }

    const handleSaveAndClose = async () => {
        try {
            await handleSave()
            toggleEditTechnicalInputModal()
        } catch (e) {
            console.error("Error during save operation: ", e)
        }
    }

    const handleCancel = () => {
        // Revert the operational costs to their original state
        setExplorationOperationalWellCosts(originalExplorationOperationalWellCosts)
        setDevelopmentOperationalWellCosts(originalDevelopmentOperationalWellCosts)

        // Revert the wells to their original state
        setWellProjectWells([...originalWellProjectWells])
        setExplorationWells([...originalExplorationWells])

        // Close the modal in all cases
        toggleEditTechnicalInputModal()
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
                            onClick={handleCancel}
                        >
                            Cancel
                        </CancelButton>
                        {!isSaving ? (
                            <>
                                <Button style={{ marginRight: "8px" }} onClick={handleSave}>Save</Button>
                                <Button onClick={handleSaveAndClose}>Save and Close</Button>
                            </>
                        ) : (
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
