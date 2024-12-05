import React, { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { projectQueryFn } from "@/Services/QueryFunctions"
import CaseTabTable from "../../../Components/CaseTabTable"

interface CessationCostsProps {
    tableYears: [number, number];
    cessationGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
    addEdit: any;
}

const CessationCosts: React.FC<CessationCostsProps> = ({
    tableYears,
    cessationGridRef,
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

    const [cessationTimeSeriesData, setCessationTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const cessationWellsCostData = apiData.cessationWellsCost
        const cessationWellsCostOverrideData = apiData.cessationWellsCostOverride
        const cessationOffshoreFacilitiesCostData = apiData.cessationOffshoreFacilitiesCost
        const cessationOffshoreFacilitiesCostOverrideData = apiData.cessationOffshoreFacilitiesCostOverride
        const cessationOnshoreFacilitiesCostProfileData = apiData.cessationOnshoreFacilitiesCostProfile
        const caseData = apiData.case

        const newCessationTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Cessation - Development wells",
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationOnshoreFacilitiesCostProfileData,
                resourceName: "cessationOnshoreFacilitiesCostProfile",
                resourceId: caseData.id,
                resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
                resourcePropertyKey: "cessationOnshoreFacilitiesCostProfile",
                editable: true,
                overridable: false,
            },
        ]

        setCessationTimeSeriesData(newCessationTimeSeriesData)
    }, [apiData, projectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Cessation cost"
            gridRef={cessationGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            addEdit={addEdit}
        />
    )
}

export default CessationCosts
