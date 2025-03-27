import React, { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

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

        const currency = revisionAndProjectData?.commonProjectAndRevisionData.currency
        const physUnit = revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit

        interface CreateProfileDataParams {
            profileName: string;
            profile: any;
            resourceName: ProfileTypes;
            overrideProfile?: any;
            editable?: boolean;
        }

        const createProfileData = ({
            profileName,
            profile,
            resourceName,
            overrideProfile,
            editable = true,
        }: CreateProfileDataParams): ITimeSeriesTableData => {
            const isCalculatedField = calculatedFields.includes(resourceName)

            return ({
                profileName,
                unit: getUnitByProfileName(profileName, physUnit, currency),
                profile,
                resourceName,
                resourceId: caseData.caseId,
                resourcePropertyKey: resourceName,
                editable,
                overridable: isCalculatedField,
                ...(overrideProfile && { overrideProfile }),
            })
        }

        const newStudyTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Feasibility & conceptual stud.",
                profile: totalFeasibilityAndConceptStudiesData,
                resourceName: ProfileTypes.TotalFeasibilityAndConceptStudiesOverride,
                overrideProfile: totalFeasibilityAndConceptStudiesOverrideData,
            }),
            createProfileData({
                profileName: "FEED studies (DG2-DG3)",
                profile: totalFeedStudiesData,
                resourceName: ProfileTypes.TotalFeedStudiesOverride,
                overrideProfile: totalFeedStudiesOverrideData,
            }),
            createProfileData({
                profileName: "Other studies",
                profile: totalOtherStudiesCostProfileData,
                resourceName: ProfileTypes.TotalOtherStudiesCostProfile,
            }),
        ]

        setStudyTimeSeriesData(newStudyTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears, calculatedFields])

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
            decimalPrecision={1}
        />
    )
}

export default TotalStudyCosts
