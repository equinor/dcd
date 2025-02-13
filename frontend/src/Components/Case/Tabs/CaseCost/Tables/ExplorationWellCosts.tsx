import React, { useEffect, useState } from "react"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { Currency, ProfileTypes } from "@/Models/enums"

interface ExplorationWellCostsProps {
    tableYears: [number, number];
    explorationWellsGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const ExplorationWellCosts: React.FC<ExplorationWellCostsProps> = ({
    tableYears,
    explorationWellsGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const revisionAndProjectData = useDataFetch()

    const [explorationTimeSeriesData, setExplorationTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const explorationId = apiData?.explorationId
        const gAndGAdminCostData = apiData.gAndGAdminCost
        const explorationGAndGAdminCostOverrideData = apiData.gAndGAdminCostOverride

        const seismicAcquisitionAndProcessingData = apiData.seismicAcquisitionAndProcessing
        const countryOfficeCostData = apiData.countryOfficeCost
        const projectSpecificDrillingCostProfileData = apiData.projectSpecificDrillingCostProfile

        const explorationWellCostProfileData = apiData.explorationWellCostProfile
        const appraisalWellCostProfileData = apiData.appraisalWellCostProfile
        const sidetrackCostProfileData = apiData.sidetrackCostProfile

        if (!explorationId) {
            console.error("No exploration data")
            return
        }

        const newExplorationTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Seismic acquisition and processing",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: seismicAcquisitionAndProcessingData,
                resourceName: ProfileTypes.SeismicAcquisitionAndProcessing,
                resourceId: explorationId,
                resourcePropertyKey: ProfileTypes.SeismicAcquisitionAndProcessing,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Country office",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: countryOfficeCostData,
                resourceName: "countryOfficeCost",
                resourceId: explorationId,
                resourcePropertyKey: "countryOfficeCost",
                editable: true,
                overridable: false,
            },
            {
                profileName: "G&G and admin",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: gAndGAdminCostData,
                resourceName: "gAndGAdminCost",
                resourceId: explorationId,
                resourcePropertyKey: "gAndGAdminCost",
                editable: true,
                overridable: true,
                overrideProfile: explorationGAndGAdminCostOverrideData,
            },
            {
                profileName: "Project specific drilling cost",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: projectSpecificDrillingCostProfileData,
                resourceName: ProfileTypes.ProjectSpecificDrillingCostProfile,
                resourceId: explorationId,
                resourcePropertyKey: ProfileTypes.ProjectSpecificDrillingCostProfile,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Exploration well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: explorationWellCostProfileData,
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                resourceId: explorationId,
                resourcePropertyKey: ProfileTypes.ExplorationWellCostProfile,
                editable: false,
                overridable: false,
            },
            {
                profileName: "Appraisal well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: appraisalWellCostProfileData,
                resourceName: ProfileTypes.AppraisalWellCostProfile,
                resourceId: explorationId,
                resourcePropertyKey: ProfileTypes.AppraisalWellCostProfile,
                editable: false,
                overridable: false,
            },
            {
                profileName: "Sidetrack well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: sidetrackCostProfileData,
                resourceName: "sidetrackCostProfile",
                resourceId: explorationId,
                resourcePropertyKey: "sidetrackCostProfile",
                editable: false,
                overridable: false,
            },
        ]

        setExplorationTimeSeriesData(newExplorationTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dG4Date)}
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
