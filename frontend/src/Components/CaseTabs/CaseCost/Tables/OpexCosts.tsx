import React, { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

interface OpexCostsProps {
    tableYears: [number, number]
    opexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    apiData: Components.Schemas.CaseWithAssetsDto
}

const OpexCosts: React.FC<OpexCostsProps> = ({
    tableYears, opexGridRef, alignedGridsRef, apiData,
}) => {
    const revisionAndProjectData = useDataFetch()
    const [opexTimeSeriesData, setOpexTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    const calculatedFields = useMemo(() => [
        ProfileTypes.WellInterventionCostProfileOverride,
        ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride,
    ], [])

    useEffect(() => {
        const historicCostCostProfileData = apiData.historicCostCostProfile
        const wellInterventionCostProfileData = apiData.wellInterventionCostProfile
        const wellInterventionCostProfileOverrideData = apiData.wellInterventionCostProfileOverride
        const offshoreFacilitiesOperationsCostProfileData = apiData.offshoreFacilitiesOperationsCostProfile
        const offshoreFacilitiesOperationsCostProfileOverrideData = apiData.offshoreFacilitiesOperationsCostProfileOverride
        const onshoreRelatedOpexCostProfileData = apiData.onshoreRelatedOpexCostProfile
        const additionalOpexCostProfileData = apiData.additionalOpexCostProfile
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

        const newOpexTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Historic cost",
                profile: historicCostCostProfileData,
                resourceName: ProfileTypes.HistoricCostCostProfile,
            }),
            createProfileData({
                profileName: "Well intervention",
                profile: wellInterventionCostProfileData,
                resourceName: ProfileTypes.WellInterventionCostProfileOverride,
                overrideProfile: wellInterventionCostProfileOverrideData,
            }),
            createProfileData({
                profileName: "Offshore facilities operations",
                profile: offshoreFacilitiesOperationsCostProfileData,
                resourceName: ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride,
                overrideProfile: offshoreFacilitiesOperationsCostProfileOverrideData,
            }),
            createProfileData({
                profileName: "Onshore related OPEX (input req.)",
                profile: onshoreRelatedOpexCostProfileData,
                resourceName: ProfileTypes.OnshoreRelatedOpexCostProfile,
            }),
            createProfileData({
                profileName: "Additional OPEX (input req.)",
                profile: additionalOpexCostProfileData,
                resourceName: ProfileTypes.AdditionalOpexCostProfile,
            }),
        ]

        setOpexTimeSeriesData(newOpexTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears, calculatedFields])

    return (
        <CaseBaseTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="Opex costs"
            gridRef={opexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            calculatedFields={calculatedFields}
            decimalPrecision={1}
        />
    )
}

export default OpexCosts
