import { useEffect, useState } from "react"
import {
    Button,
    Progress,
    Tabs,
    Tooltip,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"

import { GetTechnicalInputService } from "@/Services/TechnicalInputService"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditProject from "@/Hooks/useEditProject"
import { useAppContext } from "@/Context/AppContext"
import { isExplorationWell } from "@/Utils/common"
import { EMPTY_GUID } from "@/Utils/constants"
import WellCostsTab from "./WellCostsTab"
import PROSPTab from "./PROSPTab"
import CO2Tab from "./CO2Tab"

const {
    List, Tab, Panels, Panel,
} = Tabs

const EditTechnicalInputModal = () => {
    const { editMode } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()

    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [activeTab, setActiveTab] = useState<number>(0)
    const [deletedWells, setDeletedWells] = useState<string[]>([])

    const [
        explorationOperationalWellCosts,
        setExplorationOperationalWellCosts,
    ] = useState<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>(revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts)
    const [
        developmentOperationalWellCosts,
        setDevelopmentOperationalWellCosts,
    ] = useState<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>(revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts)

    const [
        ,
        setOriginalExplorationOperationalWellCosts,
    ] = useState<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>(revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts)
    const [
        ,
        setOriginalDevelopmentOperationalWellCosts,
    ] = useState<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>(revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts)

    const [wellProjectWells, setWellProjectWells] = useState<Components.Schemas.WellOverviewDto[]>(
        revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? [],
    )
    const [explorationWells, setExplorationWells] = useState<Components.Schemas.WellOverviewDto[]>(
        revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? [],
    )

    const [, setOriginalWellProjectWells] = useState<Components.Schemas.WellOverviewDto[]>(
        revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? [],
    )
    const [, setOriginalExplorationWells] = useState<Components.Schemas.WellOverviewDto[]>(
        revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? [],
    )

    const handleSave = async () => {
        if (!revisionAndProjectData) { return }
        try {
            const dto: Components.Schemas.UpdateTechnicalInputDto = {}
            setIsSaving(true)
            dto.projectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }

            dto.explorationOperationalWellCostsDto = explorationOperationalWellCosts
            dto.developmentOperationalWellCostsDto = developmentOperationalWellCosts

            const wellDtos = [...explorationWells, ...wellProjectWells]

            dto.createWellDtos = wellDtos.filter((w) => w.id === EMPTY_GUID || w.id === undefined || w.id === null || w.id === "")
            dto.updateWellDtos = wellDtos.filter((w) => w.id !== EMPTY_GUID && w.id !== undefined && w.id !== null && w.id !== "")
            dto.deleteWellDtos = deletedWells.map((id) => ({ id }))

            // refactor to use react-query?
            const projectData = await (await GetTechnicalInputService()).update(revisionAndProjectData.projectId, dto)

            addProjectEdit(projectData.projectId, projectData.commonProjectAndRevisionData)

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

    useEffect(() => {
        setExplorationOperationalWellCosts(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts))
        setDevelopmentOperationalWellCosts(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts))
        setOriginalExplorationOperationalWellCosts(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.explorationOperationalWellCosts))
        setOriginalDevelopmentOperationalWellCosts(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.developmentOperationalWellCosts))
        setWellProjectWells(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? []))
        setExplorationWells(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? []))
        setOriginalWellProjectWells(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? []))
        setOriginalExplorationWells(structuredClone(revisionAndProjectData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? []))
    }, [revisionAndProjectData])

    if (!revisionAndProjectData || !explorationOperationalWellCosts || !developmentOperationalWellCosts) {
        return (<div>Loading...</div>)
    }

    return (
        <div>
            <Grid container>
                <Grid item xs={12}>
                    <Tabs activeTab={activeTab} onChange={setActiveTab}>
                        <List>
                            <Tab>Well Costs</Tab>
                            <Tab>PROSP</Tab>
                            <Tab>CO2</Tab>
                        </List>
                        <Panels>
                            <Panel>
                                <WellCostsTab
                                    developmentOperationalWellCosts={developmentOperationalWellCosts}
                                    setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
                                    explorationOperationalWellCosts={explorationOperationalWellCosts}
                                    setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                                    explorationWells={explorationWells}
                                    setExplorationWells={setExplorationWells}
                                    wellProjectWells={wellProjectWells}
                                    setWellProjectWells={setWellProjectWells}
                                    setDeletedWells={setDeletedWells}
                                />
                            </Panel>
                            <Panel>
                                <PROSPTab />
                            </Panel>
                            <Panel>
                                <CO2Tab />
                            </Panel>
                        </Panels>
                    </Tabs>
                </Grid>
            </Grid>

            <Grid container justifyContent="flex-end">
                {!isSaving && (
                    <Grid item>
                        <Tooltip title={getEditDisabledText()}>
                            <Button
                                onClick={handleSave}
                                disabled={isEditDisabled || !editMode}
                            >
                                Save
                            </Button>
                        </Tooltip>
                    </Grid>
                )}
                {(editMode) && isSaving && (
                    <Grid item>
                        <Button>
                            <Progress.Dots />
                        </Button>
                    </Grid>
                )}
            </Grid>
        </div>

    )
}

export default EditTechnicalInputModal
