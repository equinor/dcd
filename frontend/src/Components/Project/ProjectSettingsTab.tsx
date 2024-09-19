import { useState, ChangeEventHandler, useEffect } from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery, useQueryClient } from "react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useProjectContext } from "../../Context/ProjectContext"
import InputSwitcher from "../Input/Components/InputSwitcher"
import { PROJECT_CLASSIFICATION } from "../../Utils/constants"
import useProjectDataEdits from "../../Hooks/useProjectDataEdits"

const ProjectSettingsTab = () => {
    const { project } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()
    const [classification, setClassification] = useState<number | undefined>(undefined)
    const [dummyRole, setDummyRole] = useState(0) // TODO: Get role from user
    const queryClient = useQueryClient()

    const { addProjectEdit } = useProjectDataEdits()

    const projectId = currentContext?.externalId || null

    const { data: apiData } = useQuery<Components.Schemas.ProjectWithAssetsDto | undefined>(
        ["apiData", { projectId }],
        () => queryClient.getQueryData(["apiData", { projectId }]),
        {
            enabled: !!projectId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId }]),
        },
    )

    const projectData = apiData

    useEffect(() => {
        if (project) {
            setClassification(project.classification)
        }
    }, [project])

    const handlePhysicalUnitChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1 && projectData) {
            const newPhysicalUnit: Components.Schemas.PhysUnit = Number(e.currentTarget.value) as Components.Schemas.PhysUnit
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...projectData }
            newProject.physicalUnit = newPhysicalUnit

            addProjectEdit({
                projectId: projectData.id,
                newResourceObject: newProject,
                previousResourceObject: projectData,
            })
        }
    }

    const handleCurrencyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && projectData) {
            const newCurrency: Components.Schemas.Currency = Number(e.currentTarget.value) as Components.Schemas.Currency
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...projectData }
            newProject.currency = newCurrency

            addProjectEdit({
                projectId: projectData.id,
                newResourceObject: newProject,
                previousResourceObject: projectData,
            })
        }
    }

    const handleClassificationChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1 && projectData) {
            setClassification(Number(e.currentTarget.value))
            const newClassification: Components.Schemas.ProjectClassification = Number(e.currentTarget.value) as unknown as Components.Schemas.ProjectClassification
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...projectData }
            newProject.classification = newClassification

            addProjectEdit({
                projectId: projectData.id,
                newResourceObject: newProject,
                previousResourceObject: projectData,
            })
        }
    }

    const physicalUnitOptions = {
        0: "SI",
        1: "Oil field",
    }

    const currencyOptions = {
        1: "NOK",
        2: "USD",
    }

    if (!projectData) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container direction="column" spacing={2}>
            <Grid item>
                <InputSwitcher
                    value={physicalUnitOptions[projectData.physicalUnit]}
                    label="Physical unit"
                >
                    <NativeSelect
                        id="physicalUnit"
                        label=""
                        onChange={(e) => handlePhysicalUnitChange(e)}
                        value={projectData.physicalUnit}
                    >
                        {Object.entries(physicalUnitOptions).map(([key, val]) => (
                            <option key={key} value={key}>{val}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                <InputSwitcher
                    value={currencyOptions[projectData.currency]}
                    label="Currency"
                >
                    <NativeSelect
                        id="currency"
                        label=""
                        onChange={(e) => handleCurrencyChange(e)}
                        value={projectData.currency}
                    >
                        {Object.entries(currencyOptions).map(([key, val]) => (
                            <option key={key} value={key}>{val}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item>
                {dummyRole === 0 && (
                    <InputSwitcher
                        value={projectData.classification !== undefined ? PROJECT_CLASSIFICATION[projectData.classification].label : "Not set"}
                        label="Classification"
                    >
                        <NativeSelect
                            id="classification"
                            label=""
                            onChange={(e) => handleClassificationChange(e)}
                            value={projectData.classification || undefined}
                        >
                            {Object.entries(PROJECT_CLASSIFICATION).map(([key, val]) => (
                                <option key={key} value={key}>{val.label}</option>
                            ))}
                        </NativeSelect>
                    </InputSwitcher>
                )}
            </Grid>
        </Grid>
    )
}

export default ProjectSettingsTab
