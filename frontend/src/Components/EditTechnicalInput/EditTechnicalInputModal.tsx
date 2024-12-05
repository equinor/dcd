import { useEffect, useState } from "react"
import {
    Button,
    Progress,
    Tabs,
    Tooltip,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { EMPTY_GUID } from "@/Utils/constants"
import { isExplorationWell } from "@/Utils/common"
import { useAppContext } from "@/Context/AppContext"
import { GetTechnicalInputService } from "@/Services/TechnicalInputService"
import { projectQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import useEditDisabled from "@/Hooks/useEditDisabled"
import WellCostsTab from "./WellCostsTab"
import PROSPTab from "./PROSPTab"
import CO2Tab from "./CO2Tab"

const {
    List, Tab, Panels, Panel,
} = Tabs

const EditTechnicalInputModal = () => {
    const { editMode } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const [isSaving, setIsSaving] = useState<boolean>(false)

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { addProjectEdit } = useEditProject()

    const [activeTab, setActiveTab] = useState<number>(0)
    const [deletedWells, setDeletedWells] = useState<string[]>([])

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [
        explorationOperationalWellCosts,
        setExplorationOperationalWellCosts,
    ] = useState<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>(apiData?.commonProjectAndRevisionData.explorationOperationalWellCosts)
    const [
        developmentOperationalWellCosts,
        setDevelopmentOperationalWellCosts,
    ] = useState<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>(apiData?.commonProjectAndRevisionData.developmentOperationalWellCosts)

    const [
        originalExplorationOperationalWellCosts,
        setOriginalExplorationOperationalWellCosts,
    ] = useState<Components.Schemas.ExplorationOperationalWellCostsOverviewDto | undefined>(apiData?.commonProjectAndRevisionData.explorationOperationalWellCosts)
    const [
        originalDevelopmentOperationalWellCosts,
        setOriginalDevelopmentOperationalWellCosts,
    ] = useState<Components.Schemas.DevelopmentOperationalWellCostsOverviewDto | undefined>(apiData?.commonProjectAndRevisionData.developmentOperationalWellCosts)

    const [wellProjectWells, setWellProjectWells] = useState<Components.Schemas.WellOverviewDto[]>(apiData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? [])
    const [explorationWells, setExplorationWells] = useState<Components.Schemas.WellOverviewDto[]>(apiData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? [])

    const [originalWellProjectWells, setOriginalWellProjectWells] = useState<Components.Schemas.WellOverviewDto[]>(apiData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? [])
    const [originalExplorationWells, setOriginalExplorationWells] = useState<Components.Schemas.WellOverviewDto[]>(apiData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? [])

    const handleSave = async () => {
        try {
            const dto: Components.Schemas.UpdateTechnicalInputDto = {}
            setIsSaving(true)
            dto.projectDto = { ...apiData?.commonProjectAndRevisionData }

            dto.explorationOperationalWellCostsDto = explorationOperationalWellCosts

            dto.developmentOperationalWellCostsDto = developmentOperationalWellCosts

            const wellDtos = [...explorationWells, ...wellProjectWells]

            dto.createWellDtos = wellDtos.filter((w) => w.id === EMPTY_GUID || w.id === undefined || w.id === null || w.id === "")
            dto.updateWellDtos = wellDtos.filter((w) => w.id !== EMPTY_GUID && w.id !== undefined && w.id !== null && w.id !== "")
            dto.deleteWellDtos = deletedWells.map((id) => ({ id }))

            // refactor to use react-query?
            const result = apiData ? await (await GetTechnicalInputService()).update(apiData?.projectId, dto) : undefined

            if (result?.projectData) {
                addProjectEdit(result?.projectData.projectId, result?.projectData.commonProjectAndRevisionData)
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

    useEffect(() => {
        setExplorationOperationalWellCosts(structuredClone(apiData?.commonProjectAndRevisionData.explorationOperationalWellCosts))
        setDevelopmentOperationalWellCosts(structuredClone(apiData?.commonProjectAndRevisionData.developmentOperationalWellCosts))
        setOriginalExplorationOperationalWellCosts(structuredClone(apiData?.commonProjectAndRevisionData.explorationOperationalWellCosts))
        setOriginalDevelopmentOperationalWellCosts(structuredClone(apiData?.commonProjectAndRevisionData.developmentOperationalWellCosts))
        setWellProjectWells(structuredClone(apiData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? []))
        setExplorationWells(structuredClone(apiData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? []))
        setOriginalWellProjectWells(structuredClone(apiData?.commonProjectAndRevisionData.wells?.filter((w) => !isExplorationWell(w)) ?? []))
        setOriginalExplorationWells(structuredClone(apiData?.commonProjectAndRevisionData.wells?.filter((w) => isExplorationWell(w)) ?? []))
    }, [apiData])

    if (!explorationOperationalWellCosts || !developmentOperationalWellCosts) {
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
                    <>
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
                    </>
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
