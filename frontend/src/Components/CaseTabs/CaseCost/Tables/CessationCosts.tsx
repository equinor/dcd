import React, { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

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

    const calculatedFields = useMemo(() => [
        ProfileTypes.CessationWellsCostOverride,
        ProfileTypes.CessationOffshoreFacilitiesCostOverride,
    ], [])

    useEffect(() => {
        const cessationWellsCostData = apiData.cessationWellsCost
        const cessationWellsCostOverrideData = apiData.cessationWellsCostOverride
        const cessationOffshoreFacilitiesCostData = apiData.cessationOffshoreFacilitiesCost
        const cessationOffshoreFacilitiesCostOverrideData = apiData.cessationOffshoreFacilitiesCostOverride
        const cessationOnshoreFacilitiesCostProfileData = apiData.cessationOnshoreFacilitiesCostProfile
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

        const newCessationTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Cessation - Development wells",
                profile: cessationWellsCostData,
                resourceName: ProfileTypes.CessationWellsCostOverride,
                overrideProfile: cessationWellsCostOverrideData,
            }),
            createProfileData({
                profileName: "Cessation - Offshore facilities",
                profile: cessationOffshoreFacilitiesCostData,
                resourceName: ProfileTypes.CessationOffshoreFacilitiesCostOverride,
                overrideProfile: cessationOffshoreFacilitiesCostOverrideData,
            }),
            createProfileData({
                profileName: "CAPEX - Cessation - Onshore facilities",
                profile: cessationOnshoreFacilitiesCostProfileData,
                resourceName: ProfileTypes.CessationOnshoreFacilitiesCostProfile,
                overridable: false,
            }),
        ]

        setCessationTimeSeriesData(newCessationTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears, calculatedFields])

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
            calculatedFields={calculatedFields}
            decimalPrecision={1}
        />
    )
}

export default CessationCosts
