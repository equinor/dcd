import React, { useRef } from "react"
import { useQuery, useQueryClient } from "react-query"
import {
    Icon,
    Button,
    Input,
    Typography,
} from "@equinor/eds-core-react"
import { arrow_back } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetCaseService } from "../../Services/CaseService"
import { useAppContext } from "../../Context/AppContext"
import { ChooseReferenceCase, ReferenceCaseIcon } from "../Case/Components/ReferenceCaseIcon"
import Classification from "./Classification"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetProjectService } from "../../Services/ProjectService"
import useDataEdits from "../../Hooks/useDataEdits"

interface props {
    backToProject: () => void
    projectId: string
    caseId: string
}

const CaseControls: React.FC<props> = ({ backToProject, projectId, caseId }) => {
    const nameInput = useRef<any>(null)
    const { project, setProject } = useProjectContext()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useDataEdits()

    const queryClient = useQueryClient()

    const fetchCaseData = async () => {
        const caseService = await GetCaseService()
        return caseService.getCaseWithAssets(projectId, caseId)
    }

    useQuery(
        ["apiData", { projectId, caseId }],
        fetchCaseData,
        {
            enabled: !!projectId && !!caseId,
            onSuccess: (result) => {
                const caseData = result.case
                const drainageStrategyData = result.drainageStrategy
                const explorationData = result.exploration
                const substructureData = result.substructure
                const surfData = result.surf
                const topsideData = result.topside
                const transportData = result.transport
                const wellProjectData = result.wellProject

                const cessationWellsCostData = result.cessationWellsCost
                const cessationWellsCostOverrideData = result.cessationWellsCostOverride
                const cessationOffshoreFacilitiesCostData = result.cessationOffshoreFacilitiesCost
                const cessationOffshoreFacilitiesCostOverrideData = result.cessationOffshoreFacilitiesCostOverride
                const cessationOnshoreFacilitiesCostProfileData = result.cessationOnshoreFacilitiesCostProfile
                const totalFeasibilityAndConceptStudiesData = result.totalFeasibilityAndConceptStudies
                const totalFeasibilityAndConceptStudiesOverrideData = result.totalFeasibilityAndConceptStudiesOverride
                const totalFEEDStudiesData = result.totalFEEDStudies
                const totalFEEDStudiesOverrideData = result.totalFEEDStudiesOverride
                const totalOtherStudiesData = result.totalOtherStudies
                const historicCostCostProfileData = result.historicCostCostProfile
                const wellInterventionCostProfileData = result.wellInterventionCostProfile
                const wellInterventionCostProfileOverrideData = result.wellInterventionCostProfileOverride
                const offshoreFacilitiesOperationsCostProfileData = result.offshoreFacilitiesOperationsCostProfile
                const offshoreFacilitiesOperationsCostProfileOverrideData = result.offshoreFacilitiesOperationsCostProfileOverride
                const onshoreRelatedOPEXCostProfileData = result.onshoreRelatedOPEXCostProfile
                const additionalOPEXCostProfileData = result.additionalOPEXCostProfile

                const topsideCostProfileOverrideData = result.topsideCostProfileOverride
                const surfCostProfileOverrideData = result.surfCostProfileOverride
                const transportCostProfileOverrideData = result.transportCostProfileOverride
                const substructureCostProfileOverrideData = result.substructureCostProfileOverride

                const productionProfileOilData = result.productionProfileOil
                const productionProfileGasData = result.productionProfileGas
                const productionProfileWaterData = result.productionProfileWater
                const productionProfileWaterInjectionData = result.productionProfileWaterInjection
                const fuelFlaringAndLossesData = result.fuelFlaringAndLosses
                const fuelFlaringAndLossesOverrideData = result.fuelFlaringAndLossesOverride
                const netSalesGasData = result.netSalesGas
                const netSalesGasOverrideData = result.netSalesGasOverride
                const co2EmissionsData = result.co2Emissions
                const co2EmissionsOverrideData = result.co2EmissionsOverride
                const productionProfileNGLData = result.productionProfileNGL
                const importedElectricityData = result.importedElectricity
                const importedElectricityOverrideData = result.importedElectricityOverride
                const co2IntensityData = result.co2Intensity
                const deferredOilProductionData = result.deferredOilProduction
                const deferredGasProductionData = result.deferredGasProduction

                // Assets
                queryClient.setQueryData([{ projectId, caseId, resourceId: "" }], caseData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: topsideData.id }], topsideData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: surfData.id }], surfData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: substructureData.id }], substructureData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: transportData.id }], transportData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: drainageStrategyData.id }], drainageStrategyData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: explorationData.id }], explorationData)
                queryClient.setQueryData([{ projectId, caseId, resourceId: wellProjectData.id }], wellProjectData)

                // Case
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: cessationWellsCostData?.id,
                }], cessationWellsCostData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: cessationWellsCostOverrideData?.id,
                }], cessationWellsCostOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: cessationOffshoreFacilitiesCostData?.id,
                }], cessationOffshoreFacilitiesCostData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: cessationOffshoreFacilitiesCostOverrideData?.id,
                }], cessationOffshoreFacilitiesCostOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
                }], cessationOnshoreFacilitiesCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: totalFeasibilityAndConceptStudiesData?.id,
                }], totalFeasibilityAndConceptStudiesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: totalFeasibilityAndConceptStudiesOverrideData?.id,
                }], totalFeasibilityAndConceptStudiesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: totalFEEDStudiesData?.id,
                }], totalFEEDStudiesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: totalFEEDStudiesOverrideData?.id,
                }], totalFEEDStudiesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: totalOtherStudiesData?.id,
                }], totalOtherStudiesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: historicCostCostProfileData?.id,
                }], historicCostCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: wellInterventionCostProfileData?.id,
                }], wellInterventionCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: wellInterventionCostProfileOverrideData?.id,
                }], wellInterventionCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: offshoreFacilitiesOperationsCostProfileData?.id,
                }], offshoreFacilitiesOperationsCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: offshoreFacilitiesOperationsCostProfileOverrideData?.id,
                }], offshoreFacilitiesOperationsCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: onshoreRelatedOPEXCostProfileData?.id,
                }], onshoreRelatedOPEXCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: additionalOPEXCostProfileData?.id,
                }], additionalOPEXCostProfileData)

                // Prosp assets
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: topsideCostProfileOverrideData?.id,
                }], topsideCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: surfCostProfileOverrideData?.id,
                }], surfCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: transportCostProfileOverrideData?.id,
                }], transportCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: substructureCostProfileOverrideData?.id,
                }], substructureCostProfileOverrideData)

                // Drainage strategy
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: productionProfileOilData?.id,
                }], productionProfileOilData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: productionProfileGasData?.id,
                }], productionProfileGasData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: productionProfileWaterData?.id,
                }], productionProfileWaterData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: productionProfileWaterInjectionData?.id,
                }], productionProfileWaterInjectionData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: fuelFlaringAndLossesData?.id,
                }], fuelFlaringAndLossesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: fuelFlaringAndLossesOverrideData?.id,
                }], fuelFlaringAndLossesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: netSalesGasData?.id,
                }], netSalesGasData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: netSalesGasOverrideData?.id,
                }], netSalesGasOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: co2EmissionsData?.id,
                }], co2EmissionsData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: co2EmissionsOverrideData?.id,
                }], co2EmissionsOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: productionProfileNGLData?.id,
                }], productionProfileNGLData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: importedElectricityData?.id,
                }], importedElectricityData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: importedElectricityOverrideData?.id,
                }], importedElectricityOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: co2IntensityData?.id,
                }], co2IntensityData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: deferredOilProductionData?.id,
                }], deferredOilProductionData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: "", resourceProfileId: deferredGasProductionData?.id,
                }], deferredGasProductionData)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage(error.message)
            },
        },
    )

    const { data: caseData } = useQuery<Components.Schemas.CaseDto | undefined>(
        [{ projectId, caseId, resourceId: "" }],
        () => queryClient.getQueryData([{ projectId, caseId, resourceId: "" }]),
        {
            enabled: !!project && !!projectId,
            initialData: () => queryClient.getQueryData([{ projectId: project?.id, caseId, resourceId: "" }]) as Components.Schemas.CaseDto,
        },
    )

    const handleCaseNameChange = (name: string) => {
        if (caseData) {
            addEdit({
                newValue: name,
                previousValue: caseData.name,
                inputLabel: "Name",
                projectId,
                resourceName: "case",
                resourcePropertyKey: "name",
                resourceId: "",
                caseId,
            })
        }
    }

    const handleReferenceCaseChange = async (referenceCaseId: string) => {
        if (project) {
            const newProject = {
                ...project,
            }
            if (newProject.referenceCaseId === referenceCaseId) {
                newProject.referenceCaseId = EMPTY_GUID
            } else {
                newProject.referenceCaseId = referenceCaseId ?? ""
            }
            const updateProject = await (await GetProjectService()).updateProject(projectId, newProject)
            setProject(updateProject)
        }
    }

    if (!caseData) {
        return null
    }

    return (
        <>
            <Grid item xs={0}>
                <Button
                    onClick={backToProject}
                    variant="ghost_icon"
                >
                    <Icon data={arrow_back} />
                </Button>
            </Grid>

            <Grid item xs display="flex" alignItems="center" gap={1}>
                {editMode
                    ? (
                        <>
                            <ChooseReferenceCase
                                projectRefCaseId={project?.referenceCaseId}
                                projectCaseId={caseId}
                                handleReferenceCaseChange={() => handleReferenceCaseChange(caseId)}
                            />
                            <Input // todo: should not be allowed to be empty
                                ref={nameInput}
                                type="text"
                                defaultValue={caseData.name}
                                onBlur={() => handleCaseNameChange(nameInput.current.value)}
                            />
                        </>
                    )
                    : (
                        <>
                            {project?.referenceCaseId === caseId && (
                                <ReferenceCaseIcon />
                            )}
                            <Typography variant="h4">
                                {caseData.name}
                            </Typography>
                            <Classification />
                        </>
                    )}
            </Grid>

        </>
    )
}

export default CaseControls
