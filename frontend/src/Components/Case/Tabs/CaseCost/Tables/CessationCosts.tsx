import React, { } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number]
    cessationGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
    apiData: Components.Schemas.CaseWithAssetsDto | undefined
}
const CessationCosts: React.FC<CesationCostsProps> = ({
    tableYears, cessationGridRef, alignedGridsRef, caseData, apiData,
}) => {
    const { project } = useProjectContext()

    const cessationWellsCostData = apiData?.cessationWellsCost
    const cessationWellsCostOverrideData = apiData?.cessationWellsCostOverride
    const cessationOffshoreFacilitiesCostData = apiData?.cessationOffshoreFacilitiesCost
    const cessationOffshoreFacilitiesCostOverrideData = apiData?.cessationOffshoreFacilitiesCostOverride
    const cessationOnshoreFacilitiesCostProfileData = apiData?.cessationOnshoreFacilitiesCostProfile

    const cessationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Cessation - Development wells",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationWellsCostData,
            resourceName: "cessationWellsCostOverride",
            resourceId: caseData.id,
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
            resourceId: caseData.id,
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
            resourceId: caseData.id,
            resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
            resourcePropertyKey: "cessationOnshoreFacilitiesCostProfile",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
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
