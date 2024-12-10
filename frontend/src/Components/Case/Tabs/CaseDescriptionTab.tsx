import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useEffect, useState } from "react"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import Gallery from "../../Gallery/Gallery"
import { useAppContext } from "../../../Context/AppContext"
import CaseDescriptionTabSkeleton from "../../LoadingSkeletons/CaseDescriptionTabSkeleton"
import { caseQueryFn } from "../../../Services/QueryFunctions"
import { useProjectContext } from "../../../Context/ProjectContext"

const CaseDescriptionTab = ({ addEdit }: { addEdit: any }) => {
    const { editMode } = useAppContext()
    const { caseId, revisionId, tab } = useParams()
    const { projectId, isRevision } = useProjectContext()

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

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    useEffect(() => {
        if (apiData && apiData.case.description !== undefined) {
            setDescription(apiData.case.description)
        }
    }, [apiData])

    if (!apiData || !projectId) {
        return <CaseDescriptionTabSkeleton />
    }
    const caseData = apiData.case

    const handleBlur = (e: any) => {
        // eslint-disable-next-line no-underscore-dangle
        const newValue = e.target._value
        const previousResourceObject = structuredClone(caseData)
        const newResourceObject = structuredClone(caseData)
        newResourceObject.description = newValue

        addEdit({
            newDisplayValue: newValue,
            previousDisplayValue: caseData.description,
            previousResourceObject,
            newResourceObject,
            inputLabel: "Description",
            projectId,
            resourceName: "case",
            resourcePropertyKey: "description",
            resourceId: "",
            caseId: caseData.caseId,
            tabName: tab,
        })
        setDescription(newValue)
    }

    return (
        <Grid container spacing={2}>
            <Gallery />
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode ? (
                    <div
                        key="input"
                        id="Description"
                    >
                        <MarkdownEditor
                            id="case-description-editor"
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            value={description}
                            onBlur={(e) => handleBlur(e)}
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
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
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
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
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
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
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
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    addEdit={addEdit}
                    value={caseData.productionStrategyOverview ?? 0}
                    resourceName="case"
                    resourcePropertyKey="productionStrategyOverview"
                    options={productionStrategyOptions}
                    previousResourceObject={caseData}
                    label="Production strategy overview"
                    resourceId={caseData.caseId}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableDropdownInput
                    addEdit={addEdit}
                    value={caseData.artificialLift ?? 0}
                    resourceName="case"
                    resourcePropertyKey="artificialLift"
                    options={artificialLiftOptions}
                    previousResourceObject={caseData}
                    label="Artificial lift"
                    resourceId={caseData.caseId}
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
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
    )
}

export default CaseDescriptionTab
