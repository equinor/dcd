import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid2"
import { useEffect, useState } from "react"
import { v4 as uuidv4 } from "uuid"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import Gallery from "@/Components/Gallery/Gallery"
import CaseDescriptionTabSkeleton from "@/Components/LoadingSkeletons/CaseDescriptionTabSkeleton"
import { useProjectContext } from "@/Store/ProjectContext"
import { useCaseApiData } from "@/Hooks"
import useEditCase from "@/Hooks/useEditCase"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

const CaseDescriptionTab = () => {
    const { apiData } = useCaseApiData()
    const { projectId } = useProjectContext()

    const [description, setDescription] = useState("")
    const { canEdit } = useCanUserEdit()

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

    useEffect(() => {
        if (apiData && apiData.case.description !== undefined) {
            setDescription(apiData.case.description)
        }
    }, [apiData])

    if (!apiData || !projectId) {
        return <CaseDescriptionTabSkeleton />
    }
    const caseData = apiData.case

    const handleChange = (e: any) => {
        // eslint-disable-next-line no-underscore-dangle
        const newValue = e.target._value
        const { addEdit } = useEditCase()
        const resourceObject = structuredClone(caseData)
        resourceObject.description = newValue

        addEdit({
            uuid: uuidv4(),
            resourceObject,
            projectId,
            resourceName: "case",
            resourcePropertyKey: "description",
            resourceId: "",
            caseId: caseData.caseId,
        })
        setDescription(newValue)
    }

    return (
        <Grid container spacing={2}>
            <Gallery />
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={12} sx={{ marginBottom: canEdit() ? "32px" : 0 }}>
                        <Typography group="input" variant="label">Description</Typography>
                        {canEdit() ? (
                            <div
                                key="input"
                                id="Description"
                            >
                                <MarkdownEditor
                                    id="case-description-editor"
                                    menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                                    value={description}
                                    onInput={handleChange}
                                />
                            </div>
                        ) : (
                            <div
                                key="input"
                            >
                                <MarkdownViewer
                                    value={caseData.description ?? ""}
                                    id="case-description-viewer"
                                />
                            </div>
                        )}
                    </Grid>
                </Grid>
            </Grid>

            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Production wells"
                            value={caseData.producerCount ?? 0}
                            previousResourceObject={caseData}
                            integer
                            min={0}
                            max={100000}
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="waterInjectorCount"
                            label="Water injector wells"
                            value={caseData.waterInjectorCount ?? 0}
                            previousResourceObject={caseData}
                            integer
                            disabled={false}
                            min={0}
                            max={100000}
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="gasInjectorCount"
                            label="Gas injector wells"
                            value={caseData.gasInjectorCount ?? 0}
                            previousResourceObject={caseData}
                            integer
                            min={0}
                            max={100000}
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.productionStrategyOverview ?? 0}
                            resourceName="case"
                            resourcePropertyKey="productionStrategyOverview"
                            options={productionStrategyOptions}
                            previousResourceObject={caseData}
                            label="Production strategy overview"
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.artificialLift ?? 0}
                            resourceName="case"
                            resourcePropertyKey="artificialLift"
                            options={artificialLiftOptions}
                            previousResourceObject={caseData}
                            label="Artificial lift"
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="facilitiesAvailability"
                            label="Facilities availability"
                            value={caseData.facilitiesAvailability ?? 0}
                            previousResourceObject={caseData}
                            integer={false}
                            unit="%"
                            min={0}
                            max={100}
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default CaseDescriptionTab
