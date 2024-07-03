import React, { useEffect, useState } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number];
    cessationGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto ;
}

const CessationCosts: React.FC<CesationCostsProps> = ({
    tableYears,
    cessationGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const { project } = useProjectContext()

    const [cessationTimeSeriesData, setCessationTimeSeriesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const cessationWellsCostData = apiData.cessationWellsCost
        const cessationWellsCostOverrideData = apiData.cessationWellsCostOverride
        const cessationOffshoreFacilitiesCostData = apiData.cessationOffshoreFacilitiesCost
        const cessationOffshoreFacilitiesCostOverrideData = apiData.cessationOffshoreFacilitiesCostOverride
        const cessationOnshoreFacilitiesCostProfileData = apiData.cessationOnshoreFacilitiesCostProfile
        const caseData = apiData.case

        const newCessationTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Cessation - Development wells",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationWellsCostData,
                resourceName: "cessationWellsCostOverride",
                resourceId: caseData.id,
                resourceProfileId: cessationWellsCostOverrideData?.id,
                resourcePropertyKey: "cessationWellsCostOverride",
                overridable: true,
                overrideProfile: cessationWellsCostOverrideData,
                editable: true,
            },
            {
                profileName: "Cessation - Offshore facilities",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationOffshoreFacilitiesCostData,
                resourceName: "cessationOffshoreFacilitiesCostOverride",
                resourceId: caseData.id,
                resourceProfileId: cessationOffshoreFacilitiesCostOverrideData?.id,
                resourcePropertyKey: "cessationOffshoreFacilitiesCostOverride",
                overridable: true,
                overrideProfile: cessationOffshoreFacilitiesCostOverrideData,
                editable: true,
            },
            {
                profileName: "CAPEX - Cessation - Onshore facilities",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationOnshoreFacilitiesCostProfileData,
                resourceName: "cessationOnshoreFacilitiesCostProfile",
                resourceId: caseData.id,
                resourceProfileId: cessationOnshoreFacilitiesCostProfileData?.id,
                resourcePropertyKey: "cessationOnshoreFacilitiesCostProfile",
                editable: true,
                overridable: false,
            },
        ]

        setCessationTimeSeriesData(newCessationTimeSeriesData)
    }, [apiData, project])

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
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
