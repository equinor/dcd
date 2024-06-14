import React, { useState, useEffect } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import { useModalContext } from "../../../../../Context/ModalContext"
import { updateObject } from "../../../../../Utils/common"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface DevelopmentWellCostsProps {
    tableYears: [number, number]
    developmentWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const DevelopmentWellCosts: React.FC<DevelopmentWellCostsProps> = ({
    tableYears,
    developmentWellsGridRef,
    alignedGridsRef,
}) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { projectCase, activeTabCase } = useCaseContext()
    const projectId = project?.id || null

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const wellProjectOilProducerCostData = apiData?.oilProducerCostProfile
    const wellProjectOilProducerCostOverrideData = apiData?.oilProducerCostProfileOverride
    const wellProjectGasProducerCostData = apiData?.gasProducerCostProfile
    const wellProjectGasProducerCostOverrideData = apiData?.gasProducerCostProfileOverride
    const wellProjectWaterInjectorCostData = apiData?.waterInjectorCostProfile
    const wellProjectWaterInjectorCostOverrideData = apiData?.waterInjectorCostProfileOverride
    const wellProjectGasInjectorCostData = apiData?.gasInjectorCostProfile
    const wellProjectGasInjectorCostOverrideData = apiData?.gasInjectorCostProfileOverride

    if (!projectCase) { return null }

    const developmentTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil producer",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectOilProducerCostData,
            resourceName: "wellProjectOilProducerCostOverride",
            resourceId: projectCase.id,
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
            resourceId: projectCase.id,
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
            resourceId: projectCase.id,
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
            resourceId: projectCase.id,
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
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
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
