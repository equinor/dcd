import React, { useEffect, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { Currency, ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"

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
        const caseId = apiData?.case.caseId
        const gAndGAdminCostData = apiData.gAndGAdminCost
        const explorationGAndGAdminCostOverrideData = apiData.gAndGAdminCostOverride

        const seismicAcquisitionAndProcessingData = apiData.seismicAcquisitionAndProcessing
        const countryOfficeCostData = apiData.countryOfficeCost
        const projectSpecificDrillingCostProfileData = apiData.projectSpecificDrillingCostProfile

        const rigUpgradeCostData = apiData.explorationRigUpgradingCostProfile
        const rigUpgradeCostOverrideData = apiData.explorationRigUpgradingCostProfileOverride
        const rigMobDemobCostData = apiData.explorationRigMobDemob
        const rigMobDemobCostOverrideData = apiData.explorationRigMobDemobOverride

        const explorationWellCostProfileData = apiData.explorationWellCostProfile
        const appraisalWellCostProfileData = apiData.appraisalWellCostProfile
        const sidetrackCostProfileData = apiData.sidetrackCostProfile

        if (!caseId) {
            console.error("No exploration data")

            return
        }

        const newExplorationTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Seismic acquisition and processing",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: seismicAcquisitionAndProcessingData,
                resourceName: ProfileTypes.SeismicAcquisitionAndProcessing,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.SeismicAcquisitionAndProcessing,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Country office",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: countryOfficeCostData,
                resourceName: ProfileTypes.CountryOfficeCost,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.CountryOfficeCost,
                editable: true,
                overridable: false,
            },
            {
                profileName: "G&G and admin",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: gAndGAdminCostData,
                resourceName: ProfileTypes.GAndGAdminCostOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.GAndGAdminCostOverride,
                editable: true,
                overridable: true,
                overrideProfile: explorationGAndGAdminCostOverrideData,
            },
            {
                profileName: "Project specific drilling cost",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: projectSpecificDrillingCostProfileData,
                resourceName: ProfileTypes.ProjectSpecificDrillingCostProfile,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.ProjectSpecificDrillingCostProfile,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Rig upgrade",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: rigUpgradeCostData,
                resourceName: ProfileTypes.ExplorationRigUpgradingCostProfileOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.ExplorationRigUpgradingCostProfileOverride,
                editable: true,
                overridable: true,
                overrideProfile: rigUpgradeCostOverrideData,
            },
            {
                profileName: "Rig mob/demob",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: rigMobDemobCostData,
                resourceName: ProfileTypes.ExplorationRigMobDemobOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.ExplorationRigMobDemobOverride,
                editable: true,
                overridable: true,
                overrideProfile: rigMobDemobCostOverrideData,
            },
            {
                profileName: "Exploration well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: explorationWellCostProfileData,
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.ExplorationWellCostProfile,
                editable: false,
                overridable: false,
            },
            {
                profileName: "Appraisal well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: appraisalWellCostProfileData,
                resourceName: ProfileTypes.AppraisalWellCostProfile,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.AppraisalWellCostProfile,
                editable: false,
                overridable: false,
            },
            {
                profileName: "Sidetrack well",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: sidetrackCostProfileData,
                resourceName: ProfileTypes.SidetrackCostProfile,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.SidetrackCostProfile,
                editable: false,
                overridable: false,
            },
        ]

        setExplorationTimeSeriesData(newExplorationTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
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
