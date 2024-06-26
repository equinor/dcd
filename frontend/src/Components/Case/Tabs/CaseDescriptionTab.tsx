import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import { useParams } from "react-router"
import { useEffect, useState } from "react"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import Gallery from "../../Gallery/Gallery"
import { useAppContext } from "../../../Context/AppContext"
import { useProjectContext } from "../../../Context/ProjectContext"
import useDataEdits from "../../../Hooks/useDataEdits"
import CaseDescriptionTabSkeleton from "./LoadingSkeletons/CaseDescriptionTabSkeleton"

const CaseDescriptionTab = () => {
    const { project } = useProjectContext()
    const { editMode } = useAppContext()
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null

    const [description, setDescription] = useState("")

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

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const caseData = apiData?.case

    useEffect(() => {
        if (caseData?.description !== undefined) {
            setDescription(caseData.description)
        }
    }, [caseData?.description])

    if (!caseData || !projectId) {
        return <CaseDescriptionTabSkeleton />
    }

    const handleBlur = (markdown: any) => {
        const { value } = markdown.target
        addEdit({
            newValue: value,
            previousValue: caseData.description,
            inputLabel: "Description",
            projectId,
            resourceName: "case",
            resourcePropertyKey: "description",
            resourceId: "",
            caseId: caseData.id,
        })
        setDescription(value)
    }

    const handleChange = (event: any) => {
        const { value } = event.currentTarget
        setDescription(value)
    }

    return (
        <Grid container spacing={2}>
            <Gallery />
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode ? (
                    <MarkdownEditor
                        menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                        value={description}
                        onBlur={handleBlur}
                        onChange={handleChange}
                    />
                ) : (
                    <MarkdownViewer value={caseData.description ?? ""} />
                )}
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

export default CaseDescriptionTab
