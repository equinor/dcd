import React, { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { projectQueryFn } from "@/Services/QueryFunctions"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import CaseTabTable from "../../../Components/CaseTabTable"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number];
    capexGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
    addEdit: any;
}

const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
    alignedGridsRef,
    apiData,
    addEdit,
}) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: transportCostData,
                resourceName: "transportCostOverride",
                resourceId: transport.id,
                resourceProfileId: transportCostOverrideData?.id,
                resourcePropertyKey: "transportCostOverride",
                overridable: true,
                overrideProfile: transportCostOverrideData,
                editable: true,
            },
            {
                profileName: "Onshore (Power from shore)",
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: onshorePowerSupplyCostData,
                resourceName: "onshorePowerSupplyCostOverride",
                resourceId: onshorePowerSupply.id,
                resourceProfileId: onshorePowerSupplyCostOverrideData?.id,
                resourcePropertyKey: "onshorePowerSupplyCostOverride",
                overridable: true,
                overrideProfile: onshorePowerSupplyCostOverrideData,
                editable: true,
            },
        ]

        setCapexTimeSeriesData(newCapexTimeSeriesData)
    }, [apiData, projectData, tableYears])

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
            addEdit={addEdit}
            isProsp
            sharepointFileId={apiData.case.sharepointFileId ?? undefined}
        />
    )
}

export default OffshoreFacillityCosts
