import React, { useEffect, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { Currency, ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"

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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: cessationWellsCostData,
                resourceName: ProfileTypes.CessationWellsCostOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.CessationWellsCostOverride,
                overridable: true,
                overrideProfile: cessationWellsCostOverrideData,
                editable: true,
            },
            {
                profileName: "Cessation - Offshore facilities",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
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
        <CaseBaseTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
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
