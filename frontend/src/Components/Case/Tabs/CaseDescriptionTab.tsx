import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import Gallery from "../../Gallery/Gallery"
import { useCaseContext } from "../../../Context/CaseContext"
import { useAppContext } from "../../../Context/AppContext"
import { useProjectContext } from "../../../Context/ProjectContext"
import useDataEdits from "../../../Hooks/useDataEdits"

const CaseDescriptionTab = () => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()
    const { editMode } = useAppContext()
    const { addEdit } = useDataEdits()
    const queryClient = useQueryClient()

    if (!projectCase || !project) { return null }

    const caseId = projectCase.id
    const projectId = project.id
    // const caseData = queryClient.getQueryData([{ projectId: project.id, caseId: projectCase.id, resourceId: "" }]) as Components.Schemas.CaseDto

    const { data: caseData } = useQuery<Components.Schemas.CaseDto | undefined>(
        [{ projectId, caseId, resourceId: "" }],
        () => queryClient.getQueryData([{ projectId, caseId, resourceId: "" }]),
        {
            enabled: !!project && !!projectId,
            initialData: () => queryClient.getQueryData([{ projectId: project?.id, caseId, resourceId: "" }]) as Components.Schemas.CaseDto,
        },
    )
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
        if (caseData) {
            console.log("newValue: ", value)
            console.log("previousValue: ", caseData?.description)
            console.log("inputLabel: ", "Description")
            console.log("projectId: ", project!.id)
            console.log("resourceName: ", "case")
            console.log("resourcePropertyKey: ", "description")
            addEdit({
                newValue: value,
                previousValue: caseData.description,
                inputLabel: "Description",
                projectId: project!.id,
                resourceName: "case",
                resourcePropertyKey: "description",
                resourceId: "",
                caseId: caseData.id,
            })
        }
    }

    if (caseData) {
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
                                {caseData.description ?? ""}
                            </MarkdownEditor>
                        )
                        : <MarkdownViewer value={caseData.description ?? ""} />}
                </Grid>
                <Grid item xs={12} md={4}>
                    <SwitchableNumberInput
                        resourceName="case"
                        resourcePropertyKey="producerCount"
                        label="Production wells"
                        value={caseData.producerCount ?? 0}
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
                        value={caseData.waterInjectorCount ?? 0}
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
                        value={caseData.gasInjectorCount ?? 0}
                        integer
                        min={0}
                        max={100000}
                    />
                </Grid>
                <Grid item xs={12} md={4}>
                    <SwitchableDropdownInput
                        value={caseData.productionStrategyOverview ?? 0}
                        resourceName="case"
                        resourcePropertyKey="productionStrategyOverview"
                        options={productionStrategyOptions}
                        label="Production strategy overview"
                    />
                </Grid>
                <Grid item xs={12} md={4}>
                    <SwitchableDropdownInput
                        value={caseData.artificialLift ?? 0}
                        resourceName="case"
                        resourcePropertyKey="artificialLift"
                        options={artificialLiftOptions}
                        label="Artificial lift"
                    />
                </Grid>
                <Grid item xs={12} md={4}>
                    <SwitchableNumberInput
                        resourceName="case"
                        resourcePropertyKey="facilitiesAvailability"
                        label="Facilities availability"
                        value={caseData.facilitiesAvailability ?? 0}
                        integer={false}
                        unit="%"
                        min={0}
                        max={100}
                    />
                </Grid>
            </Grid>
        )
    }

    return null
}

export default CaseDescriptionTab
