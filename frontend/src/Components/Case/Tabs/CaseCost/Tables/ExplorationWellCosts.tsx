import React, { useEffect } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"
import { useModalContext } from "../../../../../Context/ModalContext"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface ExplorationWellCostsProps {
    tableYears: [number, number]
    explorationWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const ExplorationWellCosts: React.FC<ExplorationWellCostsProps> = ({
    tableYears,
    explorationWellsGridRef,
    alignedGridsRef,
}) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { projectCase, activeTabCase } = useCaseContext()
    const projectId = project?.id || null

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const gAndGAdminCostData = apiData?.gAndGAdminCost
    const seismicAcquisitionAndProcessingData = apiData?.seismicAcquisitionAndProcessing
    const countryOfficeCostData = apiData?.countryOfficeCost
    const explorationWellCostProfileData = apiData?.explorationWellCostProfile
    const appraisalWellCostProfileData = apiData?.appraisalWellCostProfile
    const sidetrackCostProfileData = apiData?.sidetrackCostProfile

    if (!projectCase) { return null }

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: gAndGAdminCostData,
            resourceName: "gAndGAdminCost",
            resourceId: projectCase.id,
            resourceProfileId: gAndGAdminCostData?.id,
            resourcePropertyKey: "gAndGAdminCost",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Seismic acquisition and processing",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: seismicAcquisitionAndProcessingData,
            resourceName: "seismicAcquisitionAndProcessing",
            resourceId: projectCase.id,
            resourceProfileId: seismicAcquisitionAndProcessingData?.id,
            resourcePropertyKey: "seismicAcquisitionAndProcessing",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Country office",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: countryOfficeCostData,
            resourceName: "countryOfficeCost",
            resourceId: projectCase.id,
            resourceProfileId: countryOfficeCostData?.id,
            resourcePropertyKey: "countryOfficeCost",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Exploration well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationWellCostProfileData,
            resourceName: "explorationWellCostProfile",
            resourceId: projectCase.id,
            resourceProfileId: explorationWellCostProfileData?.id,
            resourcePropertyKey: "explorationWellCostProfile",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Appraisal well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: appraisalWellCostProfileData,
            resourceName: "appraisalWellCostProfile",
            resourceId: projectCase.id,
            resourceProfileId: appraisalWellCostProfileData?.id,
            resourcePropertyKey: "appraisalWellCostProfile",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Sidetrack well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: sidetrackCostProfileData,
            resourceName: "sidetrackCostProfile",
            resourceId: projectCase.id,
            resourceProfileId: sidetrackCostProfileData?.id,
            resourcePropertyKey: "sidetrackCostProfile",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Exploration well cost"
            gridRef={explorationWellsGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default ExplorationWellCosts
