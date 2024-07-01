import React, { useState } from "react"
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

    const exploration = apiData?.exploration
    const gAndGAdminCostData = apiData?.gAndGAdminCost
    const explorationGAndGAdminCostOverrideData = apiData?.gAndGAdminCostOverride

    const seismicAcquisitionAndProcessingData = apiData?.seismicAcquisitionAndProcessing
    const countryOfficeCostData = apiData?.countryOfficeCost
    const explorationWellCostProfileData = apiData?.explorationWellCostProfile
    const appraisalWellCostProfileData = apiData?.appraisalWellCostProfile
    const sidetrackCostProfileData = apiData?.sidetrackCostProfile

    if (!exploration) { return null }

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: gAndGAdminCostData,
            resourceName: "gAndGAdminCost",
            resourceId: exploration.id,
            resourceProfileId: explorationGAndGAdminCostOverrideData?.id,
            resourcePropertyKey: "gAndGAdminCost",
            editable: true,
            overridable: true,
            overrideProfile: explorationGAndGAdminCostOverrideData,
        },
        {
            profileName: "Seismic acquisition and processing",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: seismicAcquisitionAndProcessingData,
            resourceName: "seismicAcquisitionAndProcessing",
            resourceId: exploration.id,
            resourceProfileId: seismicAcquisitionAndProcessingData?.id,
            resourcePropertyKey: "seismicAcquisitionAndProcessing",
            editable: true,
            overridable: false,
        },
        {
            profileName: "Country office",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: countryOfficeCostData,
            resourceName: "countryOfficeCost",
            resourceId: exploration.id,
            resourceProfileId: countryOfficeCostData?.id,
            resourcePropertyKey: "countryOfficeCost",
            editable: true,
            overridable: false,
        },
        {
            profileName: "Exploration well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationWellCostProfileData,
            resourceName: "explorationWellCostProfile",
            resourceId: exploration.id,
            resourceProfileId: explorationWellCostProfileData?.id,
            resourcePropertyKey: "explorationWellCostProfile",
            editable: false,
            overridable: false,
        },
        {
            profileName: "Appraisal well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: appraisalWellCostProfileData,
            resourceName: "appraisalWellCostProfile",
            resourceId: exploration.id,
            resourceProfileId: appraisalWellCostProfileData?.id,
            resourcePropertyKey: "appraisalWellCostProfile",
            editable: false,
            overridable: false,
        },
        {
            profileName: "Sidetrack well",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: sidetrackCostProfileData,
            resourceName: "sidetrackCostProfile",
            resourceId: exploration.id,
            resourceProfileId: sidetrackCostProfileData?.id,
            resourcePropertyKey: "sidetrackCostProfile",
            editable: false,
            overridable: false,
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
