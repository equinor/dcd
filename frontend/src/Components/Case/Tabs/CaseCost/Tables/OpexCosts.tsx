import React, { useEffect, useState } from "react"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import CaseTabTable from "../../../Components/CaseTabTable"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { getYearFromDateString } from "@/Utils/DateUtils"

interface OpexCostsProps {
    tableYears: [number, number]
    opexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    apiData: Components.Schemas.CaseWithAssetsDto
    addEdit: any
}

const OpexCosts: React.FC<OpexCostsProps> = ({
    tableYears, opexGridRef, alignedGridsRef, apiData, addEdit,
}) => {
    const revisionAndProjectData = useDataFetch()
    const [opexTimeSeriesData, setOpexTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const historicCostCostProfileData = apiData.historicCostCostProfile
        const wellInterventionCostProfileData = apiData.wellInterventionCostProfile
        const wellInterventionCostProfileOverrideData = apiData.wellInterventionCostProfileOverride
        const offshoreFacilitiesOperationsCostProfileData = apiData.offshoreFacilitiesOperationsCostProfile
        const offshoreFacilitiesOperationsCostProfileOverrideData = apiData.offshoreFacilitiesOperationsCostProfileOverride
        const onshoreRelatedOPEXCostProfileData = apiData.onshoreRelatedOPEXCostProfile
        const additionalOPEXCostProfileData = apiData.additionalOPEXCostProfile
        const caseData = apiData.case

        const newOpexTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Historic cost",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: historicCostCostProfileData,
                resourceName: "historicCostCostProfile",
                resourceId: caseData.caseId,
                resourceProfileId: historicCostCostProfileData?.id,
                resourcePropertyKey: "historicCostCostProfile",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Well intervention",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: wellInterventionCostProfileData,
                resourceName: "wellInterventionCostProfileOverride",
                resourceId: caseData.caseId,
                resourceProfileId: wellInterventionCostProfileOverrideData?.id,
                resourcePropertyKey: "wellInterventionCostProfileOverride",
                overridable: true,
                overrideProfile: wellInterventionCostProfileOverrideData,
                editable: true,
            },
            {
                profileName: "Offshore facilities operations",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: offshoreFacilitiesOperationsCostProfileData,
                resourceName: "offshoreFacilitiesOperationsCostProfileOverride",
                resourceId: caseData.caseId,
                resourceProfileId: offshoreFacilitiesOperationsCostProfileOverrideData?.id,
                resourcePropertyKey: "offshoreFacilitiesOperationsCostProfileOverride",
                overridable: true,
                overrideProfile: offshoreFacilitiesOperationsCostProfileOverrideData,
                editable: true,
            },
            {
                profileName: "Onshore related OPEX (input req.)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: onshoreRelatedOPEXCostProfileData,
                resourceName: "onshoreRelatedOPEXCostProfile",
                resourceId: caseData.caseId,
                resourceProfileId: onshoreRelatedOPEXCostProfileData?.id,
                resourcePropertyKey: "onshoreRelatedOPEXCostProfile",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional OPEX (input req.)",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: additionalOPEXCostProfileData,
                resourceName: "additionalOPEXCostProfile",
                resourceId: caseData.caseId,
                resourceProfileId: additionalOPEXCostProfileData?.id,
                resourcePropertyKey: "additionalOPEXCostProfile",
                editable: true,
                overridable: false,
            },
        ]

        setOpexTimeSeriesData(newOpexTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={getYearFromDateString(apiData.case.dG4Date)}
            tableYears={tableYears}
            tableName="OPEX cost"
            gridRef={opexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            addEdit={addEdit}
        />
    )
}

export default OpexCosts
