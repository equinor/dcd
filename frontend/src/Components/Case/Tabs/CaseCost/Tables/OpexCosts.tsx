import React, { useEffect, useState } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface OpexCostsProps {
    tableYears: [number, number]
    opexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    apiData: Components.Schemas.CaseWithAssetsDto
}

const OpexCosts: React.FC<OpexCostsProps> = ({
    tableYears, opexGridRef, alignedGridsRef, apiData,
}) => {
    const { project } = useProjectContext()

    const [opexTimeSeriesData, setOpexTimeSeriesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const historicCostCostProfileData = apiData.historicCostCostProfile
        const wellInterventionCostProfileData = apiData.wellInterventionCostProfile
        const wellInterventionCostProfileOverrideData = apiData.wellInterventionCostProfileOverride
        const offshoreFacilitiesOperationsCostProfileData = apiData.offshoreFacilitiesOperationsCostProfile
        const offshoreFacilitiesOperationsCostProfileOverrideData = apiData.offshoreFacilitiesOperationsCostProfileOverride
        const onshoreRelatedOPEXCostProfileData = apiData.onshoreRelatedOPEXCostProfile
        const additionalOPEXCostProfileData = apiData.additionalOPEXCostProfile
        const caseData = apiData.case

        const newOpexTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Historic cost",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: historicCostCostProfileData,
                resourceName: "historicCostCostProfile",
                resourceId: caseData.id,
                resourceProfileId: historicCostCostProfileData?.id,
                resourcePropertyKey: "historicCostCostProfile",
                editable: true,
                overridable: false,
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
                overridable: false,
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
                overridable: false,
            },
        ]

        setOpexTimeSeriesData(newOpexTimeSeriesData)
    }, [apiData, project])

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
        />
    )
}

export default OpexCosts
