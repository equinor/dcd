import React, { useState, useEffect } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number]
    cessationGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}
const CessationCosts: React.FC<CesationCostsProps> = ({ tableYears, cessationGridRef, alignedGridsRef }) => {
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

    const cessationWellsCostData = apiData?.cessationWellsCost
    const cessationWellsCostOverrideData = apiData?.cessationWellsCostOverride
    const cessationOffshoreFacilitiesCostData = apiData?.cessationOffshoreFacilitiesCost
    const cessationOffshoreFacilitiesCostOverrideData = apiData?.cessationOffshoreFacilitiesCostOverride
    const cessationOnshoreFacilitiesCostProfileData = apiData?.cessationOnshoreFacilitiesCostProfile

    if (!projectCase) { return null }

    const cessationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Cessation - Development wells",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationWellsCostData,
            resourceName: "cessationWellsCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: cessationWellsCostOverrideData?.id,
            resourcePropertyKey: "cessationWellsCostOverride",
            overridable: true,
            overrideProfile: cessationWellsCostOverrideData,
            editable: true,
        },
        {
            profileName: "Cessation - Offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOffshoreFacilitiesCostData,
            resourceName: "cessationOffshoreFacilitiesCostOverride",
            resourceId: projectCase.id,
            resourceProfileId: cessationOffshoreFacilitiesCostOverrideData?.id,
            resourcePropertyKey: "cessationOffshoreFacilitiesCostOverride",
            overridable: true,
            overrideProfile: cessationOffshoreFacilitiesCostOverrideData,
            editable: true,
        },
        {
            profileName: "CAPEX - Cessation - Onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOnshoreFacilitiesCostProfileData,
            resourceName: "cessationOnshoreFacilitiesCostProfile",
            resourceId: projectCase.id,
            resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
            resourcePropertyKey: "cessationOnshoreFacilitiesCostProfile",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Cessation cost"
            gridRef={cessationGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default CessationCosts
