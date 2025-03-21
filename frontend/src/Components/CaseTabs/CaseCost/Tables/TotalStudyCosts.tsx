import React, { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { Currency, ProfileTypes } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { getYearFromDateString } from "@/Utils/DateUtils"

interface TotalStudyCostsProps {
    tableYears: [number, number];
    studyGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const TotalStudyCosts: React.FC<TotalStudyCostsProps> = ({
    tableYears,
    studyGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const { isCalculatingTotalStudyCostOverrides } = useAppStore()
    const revisionAndProjectData = useDataFetch()

    const calculatedFields = useMemo(() => [
        ProfileTypes.TotalFeasibilityAndConceptStudiesOverride,
        ProfileTypes.TotalFeedStudiesOverride,
    ], [])

    const [studyTimeSeriesData, setStudyTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const totalFeasibilityAndConceptStudiesData = apiData.totalFeasibilityAndConceptStudies
        const totalFeasibilityAndConceptStudiesOverrideData = apiData.totalFeasibilityAndConceptStudiesOverride
        const totalFeedStudiesData = apiData.totalFeedStudies
        const totalFeedStudiesOverrideData = apiData.totalFeedStudiesOverride
        const totalOtherStudiesCostProfileData = apiData.totalOtherStudiesCostProfile
        const caseData = apiData.case

        const newStudyTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Feasibility & conceptual stud.",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: totalFeasibilityAndConceptStudiesData,
                resourceName: ProfileTypes.TotalFeasibilityAndConceptStudiesOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.TotalFeasibilityAndConceptStudiesOverride,
                overridable: true,
                overrideProfile: totalFeasibilityAndConceptStudiesOverrideData,
                editable: true,
            },
            {
                profileName: "FEED studies (DG2-DG3)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: totalFeedStudiesData,
                resourceName: ProfileTypes.TotalFeedStudiesOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.TotalFeedStudiesOverride,
                overridable: true,
                overrideProfile: totalFeedStudiesOverrideData,
                editable: true,
            },
            {
                profileName: "Other studies",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: totalOtherStudiesCostProfileData,
                resourceName: ProfileTypes.TotalOtherStudiesCostProfile,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.TotalOtherStudiesCostProfile,
                editable: true,
                overridable: false,
            },
        ]

        setStudyTimeSeriesData(newStudyTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={studyTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="Total study cost"
            gridRef={studyGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            ongoingCalculation={isCalculatingTotalStudyCostOverrides}
            calculatedFields={calculatedFields}
        />
    )
}

export default TotalStudyCosts
