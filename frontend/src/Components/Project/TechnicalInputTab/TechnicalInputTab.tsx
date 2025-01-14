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
import { EMPTY_GUID } from "@/Utils/constants"
import WellCostsTab from "./WellCostTab/WellCostsTab"
import PROSPTab from "./PROSPTab/PROSPTab"
import CO2Tab from "./CO2Tab/CO2Tab"

const {
    List, Tab, Panels, Panel,
} = Tabs

const TechnicalInputTab = () => {
    const { editMode } = useAppContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()
    const { addProjectEdit } = useEditProject()

    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [activeTab, setActiveTab] = useState<number>(0)

    // const handleSave = async () => {
    //     if (!revisionAndProjectData) { return }
    //     try {
    //         setIsSaving(true)

    //         const wellDtos = [...explorationWells, ...wellProjectWells]

    //         const updateTechnicalInputDto = {
    //             projectDto: { ...revisionAndProjectData.commonProjectAndRevisionData },

    //             explorationOperationalWellCostsDto: explorationOperationalWellCosts || {},
    //             developmentOperationalWellCostsDto: developmentOperationalWellCosts || {},

    //             createWellDtos: wellDtos.filter((w) => w.id === EMPTY_GUID || w.id === undefined || w.id === null || w.id === ""),
    //             updateWellDtos: wellDtos.filter((w) => w.id !== EMPTY_GUID && w.id !== undefined && w.id !== null && w.id !== ""),
    //             deleteWellDtos: deletedWells.map((id) => ({ id })),
    //         }

    //         // refactor to use react-query?
    //         const projectData = await (await GetTechnicalInputService()).update(revisionAndProjectData.projectId, updateTechnicalInputDto)

    //         addProjectEdit(projectData.projectId, projectData.commonProjectAndRevisionData)

    //         setOriginalExplorationOperationalWellCosts(explorationOperationalWellCosts)
    //         setOriginalDevelopmentOperationalWellCosts(developmentOperationalWellCosts)
    //         setOriginalWellProjectWells([...wellProjectWells])
    //         setOriginalExplorationWells([...explorationWells])

    //         // Reset the changes made flag as changes have been successfully saved
    //         setIsSaving(false) // End saving process
    //     } catch (error) {
    //         console.error("Error when saving technical input: ", error)
    //         setIsSaving(false)
    //     } finally {
    //         setIsSaving(false)
    //     }
    // }

    if (!revisionAndProjectData) {
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
                                <WellCostsTab />
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

export default TechnicalInputTab
