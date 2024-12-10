import React, { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { projectQueryFn } from "@/Services/QueryFunctions"
import CaseTabTable from "../../../Components/CaseTabTable"

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
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const [opexTimeSeriesData, setOpexTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
    }, [apiData, projectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
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
