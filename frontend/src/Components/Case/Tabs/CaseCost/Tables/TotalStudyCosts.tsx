import React, { } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number]
    studyGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
    caseData: Components.Schemas.CaseDto
    apiData: Components.Schemas.CaseWithAssetsDto | undefined
}

const TotalStudyCosts: React.FC<CesationCostsProps> = ({
    tableYears, studyGridRef, alignedGridsRef, caseData, apiData,
}) => {
    const { project } = useProjectContext()
    const totalFeasibilityAndConceptStudiesData = apiData?.totalFeasibilityAndConceptStudies
    const totalFeasibilityAndConceptStudiesOverrideData = apiData?.totalFeasibilityAndConceptStudiesOverride
    const totalFEEDStudiesData = apiData?.totalFEEDStudies
    const totalFEEDStudiesOverrideData = apiData?.totalFEEDStudiesOverride
    const totalOtherStudiesCostProfileData = apiData?.totalOtherStudiesCostProfile

    const studyTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & conceptual stud.",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudiesData,
            resourceName: "totalFeasibilityAndConceptStudiesOverride",
            resourceId: caseData.id,
            resourceProfileId: totalFeasibilityAndConceptStudiesOverrideData?.id,
            resourcePropertyKey: "totalFeasibilityAndConceptStudiesOverride",
            overridable: true,
            overrideProfile: totalFeasibilityAndConceptStudiesOverrideData,
            editable: true,
        },
        {
            profileName: "FEED studies (DG2-DG3)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudiesData,
            resourceName: "totalFEEDStudiesOverride",
            resourceId: caseData.id,
            resourceProfileId: totalFEEDStudiesOverrideData?.id,
            resourcePropertyKey: "totalFEEDStudiesOverride",
            overridable: true,
            overrideProfile: totalFEEDStudiesOverrideData,
            editable: true,
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudiesCostProfileData,
            resourceName: "totalOtherStudiesCostProfile",
            resourceId: caseData?.id,
            resourceProfileId: totalOtherStudiesCostProfileData?.id,
            resourcePropertyKey: "totalOtherStudiesCostProfile",
            editable: true,
            overridable: false,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={studyTimeSeriesData}
            dg4Year={caseData?.dG4Date ? new Date(caseData?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Total study cost"
            gridRef={studyGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default TotalStudyCosts
