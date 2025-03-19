import React, { useEffect, useState } from "react"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { Currency, ProfileTypes } from "@/Models/enums"

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

    useEffect(() => {
        const historicCostCostProfileData = apiData.historicCostCostProfile
        const wellInterventionCostProfileData = apiData.wellInterventionCostProfile
        const wellInterventionCostProfileOverrideData = apiData.wellInterventionCostProfileOverride
        const offshoreFacilitiesOperationsCostProfileData = apiData.offshoreFacilitiesOperationsCostProfile
        const offshoreFacilitiesOperationsCostProfileOverrideData = apiData.offshoreFacilitiesOperationsCostProfileOverride
        const onshoreRelatedOpexCostProfileData = apiData.onshoreRelatedOpexCostProfile
        const additionalOpexCostProfileData = apiData.additionalOpexCostProfile
        const caseData = apiData.case

        const newOpexTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Historic cost",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: historicCostCostProfileData,
                resourceName: ProfileTypes.HistoricCostCostProfile,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.HistoricCostCostProfile,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Well intervention",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: wellInterventionCostProfileData,
                resourceName: ProfileTypes.WellInterventionCostProfileOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.WellInterventionCostProfileOverride,
                overridable: true,
                overrideProfile: wellInterventionCostProfileOverrideData,
                editable: true,
            },
            {
                profileName: "Offshore facilities operations",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: offshoreFacilitiesOperationsCostProfileData,
                resourceName: ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride,
                overridable: true,
                overrideProfile: offshoreFacilitiesOperationsCostProfileOverrideData,
                editable: true,
            },
            {
                profileName: "Onshore related OPEX (input req.)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: onshoreRelatedOpexCostProfileData,
                resourceName: ProfileTypes.OnshoreRelatedOpexCostProfile,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.OnshoreRelatedOpexCostProfile,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional OPEX (input req.)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok ? "MNOK" : "MUSD"}`,
                profile: additionalOpexCostProfileData,
                resourceName: ProfileTypes.AdditionalOpexCostProfile,
                resourceId: caseData.caseId,
                resourcePropertyKey: ProfileTypes.AdditionalOpexCostProfile,
                editable: true,
                overridable: false,
            },
        ]

        setOpexTimeSeriesData(newOpexTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
            tableYears={tableYears}
            tableName="OPEX cost"
            gridRef={opexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default OpexCosts
