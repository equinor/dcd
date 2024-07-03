import React, { useEffect, useState } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number];
    capexGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const { project } = useProjectContext()

    const [capexTimeSeriesData, setCapexTimeSeriesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const surf = apiData?.surf
        const topside = apiData?.topside
        const transport = apiData?.transport
        const substructure = apiData?.substructure
        const surfCostData = apiData.surfCostProfile
        const surfCostOverrideData = apiData.surfCostProfileOverride
        const topsideCostData = apiData.topsideCostProfile
        const topsideCostOverrideData = apiData.topsideCostProfileOverride
        const substructureCostData = apiData.substructureCostProfile
        const substructureCostOverrideData = apiData.substructureCostProfileOverride
        const transportCostData = apiData.transportCostProfile
        const transportCostOverrideData = apiData.transportCostProfileOverride

        if (!surf || !topside || !substructure || !transport) {
            console.error("Missing offshore facility data")
            return
        }

        const newCapexTimeSeriesData: ITimeSeriesData[] = [
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

        setCapexTimeSeriesData(newCapexTimeSeriesData)
    }, [apiData, project])

    return (
        <CaseTabTable
            timeSeriesData={capexTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Offshore facility cost"
            gridRef={capexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default OffshoreFacillityCosts
