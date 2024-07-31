import React, { useRef, useEffect, useState } from "react"
import { useQuery, useQueryClient } from "react-query"
import {
    Icon, Button, Input, Typography,
} from "@equinor/eds-core-react"
import { arrow_back } from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { useNavigate } from "react-router-dom"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetCaseService } from "../../Services/CaseService"
import { useAppContext } from "../../Context/AppContext"
import { ChooseReferenceCase, ReferenceCaseIcon } from "../Case/Components/ReferenceCaseIcon"
import Classification from "./Classification"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetProjectService } from "../../Services/ProjectService"
import useDataEdits from "../../Hooks/useDataEdits"

interface props {
    backToProject: () => void;
    projectId: string;
    caseId: string;
}

const CaseControls: React.FC<props> = ({ backToProject, projectId, caseId }) => {
    const nameInputRef = useRef<HTMLInputElement>(null)
    const { project, setProject } = useProjectContext()
    const { setSnackBarMessage, editMode } = useAppContext()
    const { addEdit } = useDataEdits()
    const navigate = useNavigate()
    const queryClient = useQueryClient()

    const [caseName, setCaseName] = useState("")

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
                const totalOtherStudiesCostProfileData = result.totalOtherStudiesCostProfile
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
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: EMPTY_GUID,
                }], caseData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: topsideData.id, resourceProfileId: EMPTY_GUID,
                }], topsideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: surfData.id, resourceProfileId: EMPTY_GUID,
                }], surfData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: substructureData.id, resourceProfileId: EMPTY_GUID,
                }], substructureData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: transportData.id, resourceProfileId: EMPTY_GUID,
                }], transportData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: drainageStrategyData.id, resourceProfileId: EMPTY_GUID,
                }], drainageStrategyData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: explorationData.id, resourceProfileId: EMPTY_GUID,
                }], explorationData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: wellProjectData.id, resourceProfileId: EMPTY_GUID,
                }], wellProjectData)

                // Case
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: cessationWellsCostData?.id,
                }], cessationWellsCostData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: cessationWellsCostOverrideData?.id,
                }], cessationWellsCostOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: cessationOffshoreFacilitiesCostData?.id,
                }], cessationOffshoreFacilitiesCostData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: cessationOffshoreFacilitiesCostOverrideData?.id,
                }], cessationOffshoreFacilitiesCostOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
                }], cessationOnshoreFacilitiesCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: totalFeasibilityAndConceptStudiesData?.id,
                }], totalFeasibilityAndConceptStudiesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: totalFeasibilityAndConceptStudiesOverrideData?.id,
                }], totalFeasibilityAndConceptStudiesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: totalFEEDStudiesData?.id,
                }], totalFEEDStudiesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: totalFEEDStudiesOverrideData?.id,
                }], totalFEEDStudiesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: totalOtherStudiesCostProfileData?.id,
                }], totalOtherStudiesCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: historicCostCostProfileData?.id,
                }], historicCostCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: wellInterventionCostProfileData?.id,
                }], wellInterventionCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: wellInterventionCostProfileOverrideData?.id,
                }], wellInterventionCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: offshoreFacilitiesOperationsCostProfileData?.id,
                }], offshoreFacilitiesOperationsCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: offshoreFacilitiesOperationsCostProfileOverrideData?.id,
                }], offshoreFacilitiesOperationsCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: onshoreRelatedOPEXCostProfileData?.id,
                }], onshoreRelatedOPEXCostProfileData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: additionalOPEXCostProfileData?.id,
                }], additionalOPEXCostProfileData)

                // Prosp assets
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: topsideCostProfileOverrideData?.id,
                }], topsideCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: surfCostProfileOverrideData?.id,
                }], surfCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: transportCostProfileOverrideData?.id,
                }], transportCostProfileOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: substructureCostProfileOverrideData?.id,
                }], substructureCostProfileOverrideData)

                // Drainage strategy
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: productionProfileOilData?.id,
                }], productionProfileOilData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: productionProfileGasData?.id,
                }], productionProfileGasData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: productionProfileWaterData?.id,
                }], productionProfileWaterData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: productionProfileWaterInjectionData?.id,
                }], productionProfileWaterInjectionData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: fuelFlaringAndLossesData?.id,
                }], fuelFlaringAndLossesData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: fuelFlaringAndLossesOverrideData?.id,
                }], fuelFlaringAndLossesOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: netSalesGasData?.id,
                }], netSalesGasData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: netSalesGasOverrideData?.id,
                }], netSalesGasOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: co2EmissionsData?.id,
                }], co2EmissionsData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: co2EmissionsOverrideData?.id,
                }], co2EmissionsOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: productionProfileNGLData?.id,
                }], productionProfileNGLData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: importedElectricityData?.id,
                }], importedElectricityData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: importedElectricityOverrideData?.id,
                }], importedElectricityOverrideData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: co2IntensityData?.id,
                }], co2IntensityData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: deferredOilProductionData?.id,
                }], deferredOilProductionData)
                queryClient.setQueryData([{
                    projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: deferredGasProductionData?.id,
                }], deferredGasProductionData)
            },
            onError: (error: Error) => {
                console.error("Error fetching data:", error)
                setSnackBarMessage("Case data not found. Redirecting back to project")
                navigate("/")
            },
        },
    )

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
        if (caseData?.name) {
            setCaseName(caseData.name)
        }
    }, [caseData?.name])

    const handleCaseNameChange = (name: string) => {
        if (caseData) {
            addEdit({
                newValue: name,
                previousValue: caseData.name,
                inputLabel: "caseName",
                projectId,
                resourceName: "case",
                resourcePropertyKey: "name",
                resourceId: EMPTY_GUID,
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
                <Button onClick={backToProject} variant="ghost_icon">
                    <Icon data={arrow_back} />
                </Button>
            </Grid>

            <Grid item xs display="flex" alignItems="center" gap={1}>
                {editMode ? (
                    <>
                        <ChooseReferenceCase
                            projectRefCaseId={project?.referenceCaseId}
                            projectCaseId={caseId}
                            handleReferenceCaseChange={() => handleReferenceCaseChange(caseId)}
                        />
                        <Input
                            id="caseName"
                            ref={nameInputRef}
                            type="text"
                            value={caseName}
                            onChange={(e: any) => setCaseName(e.target.value)}
                            onBlur={() => handleCaseNameChange(nameInputRef.current?.value || "")}
                        />
                    </>
                ) : (
                    <>
                        {project?.referenceCaseId === caseId && <ReferenceCaseIcon />}
                        <Typography variant="h4">{caseData.name}</Typography>
                        <Classification />
                    </>
                )}
            </Grid>
        </>
    )
}

export default CaseControls
