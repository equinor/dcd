import React, { useEffect, useState } from "react"

import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { Currency, ProfileTypes } from "@/Models/enums"

interface CessationCostsProps {
    tableYears: [number, number];
    cessationGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const CessationCosts: React.FC<CessationCostsProps> = ({
    tableYears,
    cessationGridRef,
    alignedGridsRef,
    apiData,
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
                resourceName: ProfileTypes.CessationOffshoreFacilitiesCostOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.CessationOffshoreFacilitiesCostOverride,
                overridable: true,
                overrideProfile: cessationOffshoreFacilitiesCostOverrideData,
                editable: true,
            },
            {
                profileName: "CAPEX - Cessation - Onshore facilities",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.NOK ? "MNOK" : "MUSD"}`,
                profile: cessationOnshoreFacilitiesCostProfileData,
                resourceName: ProfileTypes.CessationOnshoreFacilitiesCostProfile,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.CessationOnshoreFacilitiesCostProfile,
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
        />
    )
}

export default CessationCosts
