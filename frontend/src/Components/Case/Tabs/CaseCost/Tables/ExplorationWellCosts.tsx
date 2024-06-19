import React from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface ExplorationWellCostsProps {
    tableYears: [number, number]
    explorationWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
    apiData: Components.Schemas.CaseWithAssetsDto | undefined
}

const ExplorationWellCosts: React.FC<ExplorationWellCostsProps> = ({
    tableYears,
    explorationWellsGridRef,
    alignedGridsRef,
    caseData,
    apiData,
}) => {
    const { project } = useProjectContext()

    const gAndGAdminCostData = apiData?.gAndGAdminCost
    const seismicAcquisitionAndProcessingData = apiData?.seismicAcquisitionAndProcessing
    const countryOfficeCostData = apiData?.countryOfficeCost
    const explorationWellCostProfileData = apiData?.explorationWellCostProfile
    const appraisalWellCostProfileData = apiData?.appraisalWellCostProfile
    const sidetrackCostProfileData = apiData?.sidetrackCostProfile

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: gAndGAdminCostData,
            resourceName: "gAndGAdminCost",
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
            resourceProfileId: sidetrackCostProfileData?.id,
            resourcePropertyKey: "sidetrackCostProfile",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
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
