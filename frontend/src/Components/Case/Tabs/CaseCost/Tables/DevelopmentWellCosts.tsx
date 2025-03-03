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
        const caseId = apiData?.case.caseId
        const wellProjectOilProducerCostData = apiData.oilProducerCostProfile
        const wellProjectOilProducerCostOverrideData = apiData.oilProducerCostProfileOverride
        const wellProjectGasProducerCostData = apiData.gasProducerCostProfile
        const wellProjectGasProducerCostOverrideData = apiData.gasProducerCostProfileOverride
        const wellProjectWaterInjectorCostData = apiData.waterInjectorCostProfile
        const wellProjectWaterInjectorCostOverrideData = apiData.waterInjectorCostProfileOverride
        const wellProjectGasInjectorCostData = apiData.gasInjectorCostProfile
        const wellProjectGasInjectorCostOverrideData = apiData.gasInjectorCostProfileOverride

        if (!caseId) {
            console.error("No well project data")
            return
        }

        const newDevelopmentTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Oil producer",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: wellProjectOilProducerCostData,
                resourceName: ProfileTypes.OilProducerCostProfileOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.OilProducerCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectOilProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas producer",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasProducerCostData,
                resourceName: ProfileTypes.GasProducerCostProfileOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.GasProducerCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectGasProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Water injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: wellProjectWaterInjectorCostData,
                resourceName: ProfileTypes.WaterInjectorCostProfileOverride,
                resourceId: caseId,
                resourcePropertyKey: ProfileTypes.WaterInjectorCostProfileOverride,
                overridable: true,
                overrideProfile: wellProjectWaterInjectorCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasInjectorCostData,
                resourceName: ProfileTypes.GasInjectorCostProfileOverride,
                resourceId: caseId,
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
