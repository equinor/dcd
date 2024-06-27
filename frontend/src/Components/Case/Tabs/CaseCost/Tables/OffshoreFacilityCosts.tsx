import React, { } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number]
    capexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
    apiData: Components.Schemas.CaseWithAssetsDto | undefined
}
const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
    alignedGridsRef,
    caseData,
    apiData,
}) => {
    const { project } = useProjectContext()

    const surf = apiData?.surf
    const topside = apiData?.topside
    const transport = apiData?.transport
    const substructure = apiData?.substructure
    const surfCostData = apiData?.surfCostProfile
    const surfCostOverrideData = apiData?.surfCostProfileOverride
    const topsideCostData = apiData?.topsideCostProfile
    const topsideCostOverrideData = apiData?.topsideCostProfileOverride
    const substructureCostData = apiData?.substructureCostProfile
    const substructureCostOverrideData = apiData?.substructureCostProfileOverride
    const transportCostData = apiData?.transportCostProfile
    const transportCostOverrideData = apiData?.transportCostProfileOverride

    if (!surf || !topside || !substructure || !transport) { return null }

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Subsea production system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: surfCostData,
            resourceName: "surfCostOverride",
            resourceId: surf.id,
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
            resourceId: topside.id,
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
            resourceId: substructure.id,
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
            resourceId: transport.id,
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
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
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
