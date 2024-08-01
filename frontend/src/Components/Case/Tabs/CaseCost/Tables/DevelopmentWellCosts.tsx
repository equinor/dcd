import React, { useEffect, useState } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

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
    const { project } = useProjectContext()

    const [developmentTimeSeriesData, setDevelopmentTimeSeriesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const wellProject = apiData?.wellProject
        const wellProjectOilProducerCostData = apiData.oilProducerCostProfile
        const wellProjectOilProducerCostOverrideData = apiData.oilProducerCostProfileOverride
        const wellProjectGasProducerCostData = apiData.gasProducerCostProfile
        const wellProjectGasProducerCostOverrideData = apiData.gasProducerCostProfileOverride
        const wellProjectWaterInjectorCostData = apiData.waterInjectorCostProfile
        const wellProjectWaterInjectorCostOverrideData = apiData.waterInjectorCostProfileOverride
        const wellProjectGasInjectorCostData = apiData.gasInjectorCostProfile
        const wellProjectGasInjectorCostOverrideData = apiData.gasInjectorCostProfileOverride

        if (!wellProject) {
            console.error("No well project data")
            return
        }

        const newDevelopmentTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Oil producer",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectOilProducerCostData,
                resourceName: "wellProjectOilProducerCostOverride",
                resourceId: wellProject.id,
                resourceProfileId: wellProjectOilProducerCostOverrideData?.id,
                resourcePropertyKey: "wellProjectOilProducerCostOverride",
                overridable: true,
                overrideProfile: wellProjectOilProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas producer",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasProducerCostData,
                resourceName: "wellProjectGasProducerCostOverride",
                resourceId: wellProject.id,
                resourceProfileId: wellProjectGasProducerCostOverrideData?.id,
                resourcePropertyKey: "wellProjectGasProducerCostOverride",
                overridable: true,
                overrideProfile: wellProjectGasProducerCostOverrideData,
                editable: true,
            },
            {
                profileName: "Water injector",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectWaterInjectorCostData,
                resourceName: "wellProjectWaterInjectorCostOverride",
                resourceId: wellProject.id,
                resourceProfileId: wellProjectWaterInjectorCostOverrideData?.id,
                resourcePropertyKey: "wellProjectWaterInjectorCostOverride",
                overridable: true,
                overrideProfile: wellProjectWaterInjectorCostOverrideData,
                editable: true,
            },
            {
                profileName: "Gas injector",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellProjectGasInjectorCostData,
                resourceName: "wellProjectGasInjectorCostOverride",
                resourceId: wellProject.id,
                resourceProfileId: wellProjectGasInjectorCostOverrideData?.id,
                resourcePropertyKey: "wellProjectGasInjectorCostOverride",
                overridable: true,
                overrideProfile: wellProjectGasInjectorCostOverrideData,
                editable: true,
            },
        ]

        setDevelopmentTimeSeriesData(newDevelopmentTimeSeriesData)
    }, [apiData, project])

    return (
        <CaseTabTable
            timeSeriesData={developmentTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
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
