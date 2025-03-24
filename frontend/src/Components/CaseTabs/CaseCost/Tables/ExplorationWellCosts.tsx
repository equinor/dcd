import React, { useEffect, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

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

        const currency = revisionAndProjectData?.commonProjectAndRevisionData.currency
        const physUnit = revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit

        interface CreateProfileDataParams {
            profileName: string;
            profile: any;
            resourceName: ProfileTypes;
            overrideProfile?: any;
            editable?: boolean;
            overridable?: boolean;
        }

        const createProfileData = ({
            profileName,
            profile,
            resourceName,
            overrideProfile,
            editable = true,
            overridable,
        }: CreateProfileDataParams): ITimeSeriesTableData => ({
            profileName,
            unit: getUnitByProfileName(profileName, physUnit, currency),
            profile,
            resourceName,
            resourceId: caseId,
            resourcePropertyKey: resourceName,
            editable,
            overridable: overridable ?? !!overrideProfile,
            ...(overrideProfile && { overrideProfile }),
        })

        const newExplorationTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Seismic acquisition and processing",
                profile: seismicAcquisitionAndProcessingData,
                resourceName: ProfileTypes.SeismicAcquisitionAndProcessing,
            }),
            createProfileData({
                profileName: "Country office",
                profile: countryOfficeCostData,
                resourceName: ProfileTypes.CountryOfficeCost,
            }),
            createProfileData({
                profileName: "G&G and admin",
                profile: gAndGAdminCostData,
                resourceName: ProfileTypes.GAndGAdminCostOverride,
                overrideProfile: explorationGAndGAdminCostOverrideData,
            }),
            createProfileData({
                profileName: "Project specific drilling cost",
                profile: projectSpecificDrillingCostProfileData,
                resourceName: ProfileTypes.ProjectSpecificDrillingCostProfile,
            }),
            createProfileData({
                profileName: "Rig upgrade",
                profile: rigUpgradeCostData,
                resourceName: ProfileTypes.ExplorationRigUpgradingCostProfileOverride,
                overrideProfile: rigUpgradeCostOverrideData,
            }),
            createProfileData({
                profileName: "Rig mob/demob",
                profile: rigMobDemobCostData,
                resourceName: ProfileTypes.ExplorationRigMobDemobOverride,
                overrideProfile: rigMobDemobCostOverrideData,
            }),
            createProfileData({
                profileName: "Exploration well",
                profile: explorationWellCostProfileData,
                resourceName: ProfileTypes.ExplorationWellCostProfile,
                editable: false,
            }),
            createProfileData({
                profileName: "Appraisal well",
                profile: appraisalWellCostProfileData,
                resourceName: ProfileTypes.AppraisalWellCostProfile,
                editable: false,
            }),
            createProfileData({
                profileName: "Sidetrack well",
                profile: sidetrackCostProfileData,
                resourceName: ProfileTypes.SidetrackCostProfile,
                editable: false,
            }),
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
