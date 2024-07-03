import React, { useEffect, useState } from "react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number];
    studyGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
}

const TotalStudyCosts: React.FC<CesationCostsProps> = ({
    tableYears,
    studyGridRef,
    alignedGridsRef,
    apiData,
}) => {
    const { project } = useProjectContext()

    const [studyTimeSeriesData, setStudyTimeSeriesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const totalFeasibilityAndConceptStudiesData = apiData.totalFeasibilityAndConceptStudies
        const totalFeasibilityAndConceptStudiesOverrideData = apiData.totalFeasibilityAndConceptStudiesOverride
        const totalFEEDStudiesData = apiData.totalFEEDStudies
        const totalFEEDStudiesOverrideData = apiData.totalFEEDStudiesOverride
        const totalOtherStudiesCostProfileData = apiData.totalOtherStudiesCostProfile
        const caseData = apiData.case

        const newStudyTimeSeriesData: ITimeSeriesData[] = [
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
                resourceId: caseData.id,
                resourceProfileId: totalOtherStudiesCostProfileData?.id,
                resourcePropertyKey: "totalOtherStudiesCostProfile",
                editable: true,
                overridable: false,
            },
        ]

        setStudyTimeSeriesData(newStudyTimeSeriesData)
    }, [apiData, project])

    return (
        <CaseTabTable
            timeSeriesData={studyTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
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
