import { Typography } from "@equinor/eds-core-react"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid2"
import { useState, useEffect } from "react"

import Gallery from "@/Components/Gallery/Gallery"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import CaseDescriptionTabSkeleton from "@/Components/LoadingSkeletons/CaseDescriptionTabSkeleton"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useCaseApiData } from "@/Hooks/useCaseApiData"
import { useDebouncedCallback } from "@/Hooks/useDebounce"
import { useProjectContext } from "@/Store/ProjectContext"

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

const CaseDescriptionTab = (): React.ReactNode => {
    const { projectId } = useProjectContext()
    const { apiData } = useCaseApiData()
    const { canEdit } = useCanUserEdit()

    const [description, setDescription] = useState("")

    const {
        updateProductionStrategyOverview,
        updateArtificialLift,
        updateProducerCount,
        updateWaterInjectorCount,
        updateGasInjectorCount,
        updateFacilitiesAvailability,
        updateDescription,
    } = useCaseMutation()

    // Save the description to the API
    const saveDescription = (newValue: string): void => {
        updateDescription(newValue)
        setDescription(newValue)
    }

    const debouncedAddEdit = useDebouncedCallback((newValue: string) => {
        if (!apiData || !projectId) {
            return
        }

        if (newValue !== description) {
            saveDescription(newValue)
        }
    }, 3000)

    useEffect(() => {
        if (apiData && apiData.case.description !== undefined) {
            const caseDescription = apiData.case.description || ""

            setDescription(caseDescription)
        }
    }, [apiData])

    const handleChange = (e: any): void => {
        // eslint-disable-next-line no-underscore-dangle
        const newValue = e.target._value

        setDescription(newValue) // Update local state immediately
        debouncedAddEdit(newValue)
    }

    if (!apiData || !projectId) {
        return <CaseDescriptionTabSkeleton />
    }

    const caseData = apiData.case

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
                            label="Production wells"
                            value={caseData.producerCount ?? 0}
                            integer
                            min={0}
                            max={100000}
                            id={`case-producer-count-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateProducerCount(newValue)}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Water injector wells"
                            value={caseData.waterInjectorCount ?? 0}
                            integer
                            min={0}
                            max={100000}
                            id={`case-water-injector-count-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateWaterInjectorCount(newValue)}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Gas injector wells"
                            value={caseData.gasInjectorCount ?? 0}
                            integer
                            min={0}
                            max={100000}
                            id={`case-gas-injector-count-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateGasInjectorCount(newValue)}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.productionStrategyOverview ?? 0}
                            options={productionStrategyOptions}
                            label="Production strategy overview"
                            id={`case-production-strategy-overview-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateProductionStrategyOverview(newValue)}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.artificialLift ?? 0}
                            options={artificialLiftOptions}
                            label="Artificial lift"
                            id={`case-artificial-lift-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateArtificialLift(newValue)}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Facilities availability"
                            value={caseData.facilitiesAvailability ?? 0}
                            integer={false}
                            unit="%"
                            min={0}
                            max={100}
                            id={`case-facilities-availability-${caseData.caseId}`}
                            onSubmit={(newValue): Promise<void> => updateFacilitiesAvailability(newValue)}
                        />
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default CaseDescriptionTab
