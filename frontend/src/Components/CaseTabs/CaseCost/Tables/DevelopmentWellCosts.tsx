import React, { useEffect, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

interface DevelopmentWellCostsProps {
    tableYears: [number, number];
    developmentWellsGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const DevelopmentWellCosts: React.FC<DevelopmentWellCostsProps> = ({
    tableYears,
    developmentWellsGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const revisionAndProjectData = useDataFetch()
    const [developmentTimeSeriesData, setDevelopmentTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const caseId = apiData?.case.caseId
        const wellProjectOilProducerCostData = apiData.oilProducerCostProfile
        const wellProjectOilProducerCostOverrideData = apiData.oilProducerCostProfileOverride
        const wellProjectGasProducerCostData = apiData.gasProducerCostProfile
        const wellProjectGasProducerCostOverrideData = apiData.gasProducerCostProfileOverride
        const wellProjectWaterInjectorCostData = apiData.waterInjectorCostProfile
        const wellProjectWaterInjectorCostOverrideData = apiData.waterInjectorCostProfileOverride
        const wellProjectGasInjectorCostData = apiData.gasInjectorCostProfile
        const wellProjectGasInjectorCostOverrideData = apiData.gasInjectorCostProfileOverride
        const rigUpgradeCostData = apiData.developmentRigUpgradingCostProfile
        const rigUpgradeCostOverrideData = apiData.developmentRigUpgradingCostProfileOverride
        const rigMobDemobCostData = apiData.developmentRigMobDemob
        const rigMobDemobCostOverrideData = apiData.developmentRigMobDemobOverride

        if (!caseId) {
            console.error("No well project data")

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

        const newDevelopmentTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Rig upgrade",
                profile: rigUpgradeCostData,
                resourceName: ProfileTypes.DevelopmentRigUpgradingCostProfileOverride,
                overrideProfile: rigUpgradeCostOverrideData,
            }),
            createProfileData({
                profileName: "Rig mob/demob",
                profile: rigMobDemobCostData,
                resourceName: ProfileTypes.DevelopmentRigMobDemobOverride,
                overrideProfile: rigMobDemobCostOverrideData,
            }),
            createProfileData({
                profileName: "Oil producer",
                profile: wellProjectOilProducerCostData,
                resourceName: ProfileTypes.OilProducerCostProfileOverride,
                overrideProfile: wellProjectOilProducerCostOverrideData,
            }),
            createProfileData({
                profileName: "Water injector",
                profile: wellProjectWaterInjectorCostData,
                resourceName: ProfileTypes.WaterInjectorCostProfileOverride,
                overrideProfile: wellProjectWaterInjectorCostOverrideData,
            }),
            createProfileData({
                profileName: "Gas producer",
                profile: wellProjectGasProducerCostData,
                resourceName: ProfileTypes.GasProducerCostProfileOverride,
                overrideProfile: wellProjectGasProducerCostOverrideData,
            }),
            createProfileData({
                profileName: "Gas injector",
                profile: wellProjectGasInjectorCostData,
                resourceName: ProfileTypes.GasInjectorCostProfileOverride,
                overrideProfile: wellProjectGasInjectorCostOverrideData,
            }),
        ]

        setDevelopmentTimeSeriesData(newDevelopmentTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={developmentTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="Development well cost"
            gridRef={developmentWellsGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default DevelopmentWellCosts
