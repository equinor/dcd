import { ChangeEventHandler } from "react"
import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import Gallery from "../../Gallery/Gallery"
import { useCaseContext } from "../../../Context/CaseContext"
import { useAppContext } from "../../../Context/AppContext"
import { useProjectContext } from "../../../Context/ProjectContext"
import { setNonNegativeNumberState } from "../../../Utils/common"
import useDataEdits from "../../../Hooks/useDataEdits"

const CaseDescriptionTab = () => {
    const { projectCase, projectCaseEdited, setProjectCaseEdited } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !project) { return null }

    const { editMode } = useAppContext()
    const { addEdit } = useDataEdits()

    const productionStrategyOptions = {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }

    const artificialLiftOptions = {
        0: "No lift",
        1: "Gas lift",
        2: "Electrical submerged pumps",
        3: "Subsea booster pumps",
    }

    function handleDescriptionChange(value: string) {
        if (projectCaseEdited) {
            const updatedProjectCase = { ...projectCaseEdited, description: value }
            setProjectCaseEdited(updatedProjectCase)
            addEdit({
                newValue: value,
                previousValue: projectCaseEdited.description,
                inputLabel: "Description",
                projectId: project!.id,
                resourceName: "case",
                resourcePropertyKey: "description",
            })
        }
    }

    // TODO: the value is manipulated before submition. find out how to handle that with the service implementation
    const handleFacilitiesAvailabilityChange = (value: number): void => {
        const newCase = { ...projectCase }
        const newfacilitiesAvailability = value > 0
            ? Math.min(Math.max(value, 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        setProjectCaseEdited(newCase)
    }

    const handleProductionStrategyChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3, 4].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.ProductionStrategyOverview = Number(e.currentTarget.value) as Components.Schemas.ProductionStrategyOverview
            const newCase = { ...projectCase }
            newCase.productionStrategyOverview = newProductionStrategy
            setProjectCaseEdited(newCase)
        }
    }

    const handleArtificialLiftChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newArtificialLift: Components.Schemas.ArtificialLift = Number(e.currentTarget.value) as Components.Schemas.ArtificialLift
            const newCase = { ...projectCase }
            newCase.artificialLift = newArtificialLift
            setProjectCaseEdited(newCase)
        }
    }

    const getFacilitiesAvailabilityDefaultValue = () => {
        if (projectCaseEdited) {
            return projectCaseEdited.facilitiesAvailability !== undefined
                ? projectCaseEdited.facilitiesAvailability * 100
                : undefined
        }
        return projectCase?.facilitiesAvailability !== undefined
            ? projectCase.facilitiesAvailability * 100
            : undefined
    }

    const defaultValue = getFacilitiesAvailabilityDefaultValue()

    return (
        <Grid container spacing={2}>
            <Gallery />
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode
                    ? (
                        <MarkdownEditor
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            onBlur={(markdown) => {
                                // eslint-disable-next-line no-underscore-dangle
                                const value = (markdown as any).target._value
                                handleDescriptionChange(value)
                            }}
                        >
                            {projectCaseEdited ? projectCaseEdited.description : projectCase?.description ?? ""}
                        </MarkdownEditor>
                    )
                    : <MarkdownViewer value={projectCase.description} />}
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount"
                    label="Production wells"
                    onSubmit={(value) => {
                        setNonNegativeNumberState(value, "producerCount", projectCaseEdited, setProjectCaseEdited)
                    }}
                    value={projectCaseEdited ? projectCaseEdited.producerCount : projectCase?.producerCount}
                    integer
                    min={0}
                    max={100000}
                />
            </Grid>
            <Grid item xs={12} md={4}>

                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="waterInjectorCount"
                    label="Water injector wells"
                    onSubmit={(value) => {
                        setNonNegativeNumberState(value, "waterInjectorCount", projectCaseEdited, setProjectCaseEdited)
                    }}
                    value={projectCaseEdited ? projectCaseEdited.waterInjectorCount : projectCase?.waterInjectorCount}
                    integer
                    disabled={false}
                    min={0}
                    max={100000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="gasInjectorCount"
                    label="Gas injector wells"
                    onSubmit={(value) => {
                        setNonNegativeNumberState(value, "gasInjectorCount", projectCaseEdited, setProjectCaseEdited)
                    }}
                    value={projectCaseEdited ? projectCaseEdited.gasInjectorCount : projectCase?.gasInjectorCount}
                    integer
                    min={0}
                    max={100000}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    value={projectCaseEdited ? projectCaseEdited.productionStrategyOverview : projectCase.productionStrategyOverview}
                    resourceName="case"
                    resourcePropertyKey="productionStrategyOverview"
                    options={productionStrategyOptions}
                    label="Production strategy overview"
                    onSubmit={handleProductionStrategyChange}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    value={projectCaseEdited ? projectCaseEdited.artificialLift : projectCase.artificialLift}
                    resourceName="case"
                    resourcePropertyKey="artificialLift"
                    options={artificialLiftOptions}
                    label="Artificial lift"
                    onSubmit={handleArtificialLiftChange}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="facilitiesAvailability"
                    label="Facilities availability"
                    onSubmit={handleFacilitiesAvailabilityChange}
                    value={defaultValue}
                    integer={false}
                    unit="%"
                    min={0}
                    max={100}
                />
            </Grid>
        </Grid>
    )
}

export default CaseDescriptionTab
