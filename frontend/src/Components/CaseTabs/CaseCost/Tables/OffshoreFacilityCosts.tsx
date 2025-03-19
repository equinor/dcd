import React, { useEffect, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { Currency, ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"

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
    const revisionAndProjectData = useDataFetch()

    const [capexTimeSeriesData, setCapexTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const surf = apiData?.surf
        const topside = apiData?.topside
        const transport = apiData?.transport
        const substructure = apiData?.substructure
        const onshorePowerSupply = apiData?.onshorePowerSupply
        const surfCostData = apiData.surfCostProfile
        const surfCostOverrideData = apiData.surfCostProfileOverride
        const topsideCostData = apiData.topsideCostProfile
        const topsideCostOverrideData = apiData.topsideCostProfileOverride
        const substructureCostData = apiData.substructureCostProfile
        const substructureCostOverrideData = apiData.substructureCostProfileOverride
        const transportCostData = apiData.transportCostProfile
        const transportCostOverrideData = apiData.transportCostProfileOverride
        const onshorePowerSupplyCostData = apiData.onshorePowerSupplyCostProfile
        const onshorePowerSupplyCostOverrideData = apiData.onshorePowerSupplyCostProfileOverride

        if (!surf || !topside || !substructure || !transport || !onshorePowerSupply) {
            console.error("Missing offshore facility data")

            return
        }

        const newCapexTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Subsea production system",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: surfCostData,
                resourceName: ProfileTypes.SurfCostProfileOverride,
                resourceId: surf.id,
                resourcePropertyKey: ProfileTypes.SurfCostProfileOverride,
                overridable: true,
                overrideProfile: surfCostOverrideData,
                editable: true,
            },
            {
                profileName: "Topside",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: topsideCostData,
                resourceName: ProfileTypes.TopsideCostProfileOverride,
                resourceId: topside.id,
                resourcePropertyKey: ProfileTypes.TopsideCostProfileOverride,
                overridable: true,
                overrideProfile: topsideCostOverrideData,
                editable: true,
            },
            {
                profileName: "Substructure",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: substructureCostData,
                resourceName: ProfileTypes.SubstructureCostProfileOverride,
                resourceId: substructure.id,
                resourcePropertyKey: ProfileTypes.SubstructureCostProfileOverride,
                overridable: true,
                overrideProfile: substructureCostOverrideData,
                editable: true,
            },
            {
                profileName: "Transport system",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: transportCostData,
                resourceName: ProfileTypes.TransportCostProfileOverride,
                resourceId: transport.id,
                resourcePropertyKey: ProfileTypes.TransportCostProfileOverride,
                overridable: true,
                overrideProfile: transportCostOverrideData,
                editable: true,
            },
            {
                profileName: "Onshore (Power from shore)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: onshorePowerSupplyCostData,
                resourceName: ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                resourceId: onshorePowerSupply.id,
                resourcePropertyKey: ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                overridable: true,
                overrideProfile: onshorePowerSupplyCostOverrideData,
                editable: true,
            },
        ]

        setCapexTimeSeriesData(newCapexTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={capexTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="Offshore facility cost"
            gridRef={capexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            isProsp
            sharepointFileId={apiData.case.sharepointFileId ?? undefined}
        />
    )
}

export default OffshoreFacillityCosts
