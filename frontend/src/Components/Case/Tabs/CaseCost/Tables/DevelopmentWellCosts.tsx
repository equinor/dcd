import React, { } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface DevelopmentWellCostsProps {
    tableYears: [number, number]
    developmentWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
    apiData: Components.Schemas.CaseWithAssetsDto | undefined
}

const DevelopmentWellCosts: React.FC<DevelopmentWellCostsProps> = ({
    tableYears,
    developmentWellsGridRef,
    alignedGridsRef,
    caseData,
    apiData,
}) => {
    const { project } = useProjectContext()

    const wellProjectOilProducerCostData = apiData?.oilProducerCostProfile
    const wellProjectOilProducerCostOverrideData = apiData?.oilProducerCostProfileOverride
    const wellProjectGasProducerCostData = apiData?.gasProducerCostProfile
    const wellProjectGasProducerCostOverrideData = apiData?.gasProducerCostProfileOverride
    const wellProjectWaterInjectorCostData = apiData?.waterInjectorCostProfile
    const wellProjectWaterInjectorCostOverrideData = apiData?.waterInjectorCostProfileOverride
    const wellProjectGasInjectorCostData = apiData?.gasInjectorCostProfile
    const wellProjectGasInjectorCostOverrideData = apiData?.gasInjectorCostProfileOverride

    const developmentTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil producer",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectOilProducerCostData,
            resourceName: "wellProjectOilProducerCostOverride",
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
            resourceProfileId: wellProjectGasInjectorCostOverrideData?.id,
            resourcePropertyKey: "wellProjectGasInjectorCostOverride",
            overridable: true,
            overrideProfile: wellProjectGasInjectorCostOverrideData,
            editable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={developmentTimeSeriesData}
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
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
