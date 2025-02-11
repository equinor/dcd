import React, { useEffect, useState } from "react"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { getYearFromDateString } from "@/Utils/DateUtils"

interface DevelopmentWellCostsProps {
    tableYears: [number, number];
    developmentWellsGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
    addEdit: any;
}

const DevelopmentWellCosts: React.FC<DevelopmentWellCostsProps> = ({
    tableYears,
    developmentWellsGridRef,
    alignedGridsRef,
    apiData,
    addEdit,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectOilProducerCostData,
                resourceName: "wellProjectOilProducerCostOverride",
                resourceId: wellProjectId,
                resourcePropertyKey: "wellProjectOilProducerCostOverride",
                overridable: true,
                overrideProfile: wellProjectOilProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas producer",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasProducerCostData,
                resourceName: "wellProjectGasProducerCostOverride",
                resourceId: wellProjectId,
                resourcePropertyKey: "wellProjectGasProducerCostOverride",
                overridable: true,
                overrideProfile: wellProjectGasProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Water injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectWaterInjectorCostData,
                resourceName: "wellProjectWaterInjectorCostOverride",
                resourceId: wellProjectId,
                resourcePropertyKey: "wellProjectWaterInjectorCostOverride",
                overridable: true,
                overrideProfile: wellProjectWaterInjectorCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas injector",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasInjectorCostData,
                resourceName: "wellProjectGasInjectorCostOverride",
                resourceId: wellProjectId,
                resourcePropertyKey: "wellProjectGasInjectorCostOverride",
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
            addEdit={addEdit}
        />
    )
}

export default DevelopmentWellCosts
