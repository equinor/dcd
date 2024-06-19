import React, { } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface OpexCostsProps {
    tableYears: [number, number]
    opexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
}

const OpexCosts: React.FC<OpexCostsProps> = ({
    tableYears, opexGridRef, alignedGridsRef, caseData,
}) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const projectId = project?.id || null

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const historicCostCostProfileData = apiData?.historicCostCostProfile
    const wellInterventionCostProfileData = apiData?.wellInterventionCostProfile
    const wellInterventionCostProfileOverrideData = apiData?.wellInterventionCostProfileOverride
    const offshoreFacilitiesOperationsCostProfileData = apiData?.offshoreFacilitiesOperationsCostProfile
    const offshoreFacilitiesOperationsCostProfileOverrideData = apiData?.offshoreFacilitiesOperationsCostProfileOverride
    const onshoreRelatedOPEXCostProfileData = apiData?.onshoreRelatedOPEXCostProfile
    const additionalOPEXCostProfileData = apiData?.additionalOPEXCostProfile

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCostCostProfileData,
            resourceName: "historicCostCostProfile",
            resourceId: caseData.id,
            resourceProfileId: historicCostCostProfileData?.id,
            resourcePropertyKey: "historicCostCostProfile",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellInterventionCostProfileData,
            resourceName: "wellInterventionCostProfileOverride",
            resourceId: caseData.id,
            resourceProfileId: wellInterventionCostProfileOverrideData?.id,
            resourcePropertyKey: "wellInterventionCostProfileOverride",
            overridable: true,
            overrideProfile: wellInterventionCostProfileOverrideData,
            editable: true,
        },
        {
            profileName: "Offshore facilities operations",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: offshoreFacilitiesOperationsCostProfileData,
            resourceName: "offshoreFacilitiesOperationsCostProfileOverride",
            resourceId: caseData.id,
            resourceProfileId: offshoreFacilitiesOperationsCostProfileOverrideData?.id,
            resourcePropertyKey: "offshoreFacilitiesOperationsCostProfileOverride",
            overridable: true,
            overrideProfile: offshoreFacilitiesOperationsCostProfileOverrideData,
            editable: true,
        },
        {
            profileName: "Onshore related OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: onshoreRelatedOPEXCostProfileData,
            resourceName: "onshoreRelatedOPEXCostProfile",
            resourceId: caseData.id,
            resourceProfileId: onshoreRelatedOPEXCostProfileData?.id,
            resourcePropertyKey: "onshoreRelatedOPEXCostProfile",
            editable: true,
            overridable: true,
        },
        {
            profileName: "Additional OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: additionalOPEXCostProfileData,
            resourceName: "additionalOPEXCostProfile",
            resourceId: caseData.id,
            resourceProfileId: additionalOPEXCostProfileData?.id,
            resourcePropertyKey: "additionalOPEXCostProfile",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
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
