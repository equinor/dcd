import React, { useEffect, useState } from "react"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { Currency } from "@/Models/enums"

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
    const revisionAndProjectData = useDataFetch()

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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: cessationWellsCostData,
                resourceName: "cessationWellsCostOverride",
                resourceId: caseData.caseId,
                resourcePropertyKey: "cessationWellsCostOverride",
                overridable: true,
                overrideProfile: cessationWellsCostOverrideData,
                editable: true,
            },
            {
                profileName: "Cessation - Offshore facilities",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: cessationOffshoreFacilitiesCostData,
                resourceName: "cessationOffshoreFacilitiesCostOverride",
                resourceId: caseData.caseId,
                resourcePropertyKey: "cessationOffshoreFacilitiesCostOverride",
                overridable: true,
                overrideProfile: cessationOffshoreFacilitiesCostOverrideData,
                editable: true,
            },
            {
                profileName: "CAPEX - Cessation - Onshore facilities",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: cessationOnshoreFacilitiesCostProfileData,
                resourceName: "cessationOnshoreFacilitiesCostProfile",
                resourceId: caseData.caseId,
                resourcePropertyKey: "cessationOnshoreFacilitiesCostProfile",
                editable: true,
                overridable: false,
            },
        ]

        setCessationTimeSeriesData(newCessationTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dG4Date)}
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
