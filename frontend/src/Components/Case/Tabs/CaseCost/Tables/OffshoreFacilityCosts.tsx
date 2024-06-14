import React, { useState, useEffect } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number]
    capexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}
const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
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

    const surfCostData = apiData?.surfCostProfile
    const surfCostOverrideData = apiData?.surfCostProfileOverride
    const topsideCostData = apiData?.topsideCostProfile
    const topsideCostOverrideData = apiData?.topsideCostProfileOverride
    const substructureCostData = apiData?.substructureCostProfile
    const substructureCostOverrideData = apiData?.substructureCostProfileOverride
    const transportCostData = apiData?.transportCostProfile
    const transportCostOverrideData = apiData?.transportCostProfileOverride

    if (!projectCase) { return null }

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Subsea production system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: surfCostData,
            resourceName: "surfCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: surfCostOverrideData?.id,
            resourcePropertyKey: "surfCostOverride",
            overridable: true,
            overrideProfile: surfCostOverrideData,
            editable: true,
        },
        {
            profileName: "Topside",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: topsideCostData,
            resourceName: "topsideCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: topsideCostOverrideData?.id,
            resourcePropertyKey: "topsideCostOverride",
            overridable: true,
            overrideProfile: topsideCostOverrideData,
            editable: true,
        },
        {
            profileName: "Substructure",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: substructureCostData,
            resourceName: "substructureCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: substructureCostOverrideData?.id,
            resourcePropertyKey: "substructureCostOverride",
            overridable: true,
            overrideProfile: substructureCostOverrideData,
            editable: true,
        },
        {
            profileName: "Transport system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: transportCostData,
            resourceName: "transportCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: transportCostOverrideData?.id,
            resourcePropertyKey: "transportCostOverride",
            overridable: true,
            overrideProfile: transportCostOverrideData,
            editable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={capexTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Offshore facilitiy cost"
            gridRef={capexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default OffshoreFacillityCosts
