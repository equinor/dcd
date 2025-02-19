import React, { useEffect, useState } from "react"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { Currency, ProfileTypes } from "@/Models/enums"

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
        const wellProjectId = apiData?.wellProjectId
        const wellProjectOilProducerCostData = apiData.oilProducerCostProfile
        const wellProjectOilProducerCostOverrideData = apiData.oilProducerCostProfileOverride
        const wellProjectGasProducerCostData = apiData.gasProducerCostProfile
        const wellProjectGasProducerCostOverrideData = apiData.gasProducerCostProfileOverride
        const wellProjectWaterInjectorCostData = apiData.waterInjectorCostProfile
        const wellProjectWaterInjectorCostOverrideData = apiData.waterInjectorCostProfileOverride
        const wellProjectGasInjectorCostData = apiData.gasInjectorCostProfile
        const wellProjectGasInjectorCostOverrideData = apiData.gasInjectorCostProfileOverride

        if (!wellProjectId) {
            console.error("No well project data")
            return
        }

        const newDevelopmentTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Oil producer",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: wellProjectOilProducerCostData,
                resourceName: ProfileTypes.OilProducerCostProfileOverride,
                resourceId: wellProjectId,
                resourcePropertyKey: ProfileTypes.OilProducerCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectOilProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas producer",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasProducerCostData,
                resourceName: ProfileTypes.GasProducerCostProfileOverride,
                resourceId: wellProjectId,
                resourcePropertyKey: ProfileTypes.GasProducerCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectGasProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Water injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: wellProjectWaterInjectorCostData,
                resourceName: ProfileTypes.WaterInjectorCostProfileOverride,
                resourceId: wellProjectId,
                resourcePropertyKey: ProfileTypes.WaterInjectorCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectWaterInjectorCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasInjectorCostData,
                resourceName: ProfileTypes.GasInjectorCostProfileOverride,
                resourceId: wellProjectId,
                resourcePropertyKey: ProfileTypes.GasInjectorCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectGasInjectorCostOverrideData,
                editable: true,
            },
        ]

        setDevelopmentTimeSeriesData(newDevelopmentTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={developmentTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dG4Date)}
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
