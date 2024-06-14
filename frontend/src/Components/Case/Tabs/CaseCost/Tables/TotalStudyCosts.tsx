import React, { } from "react"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"

interface CesationCostsProps {
    tableYears: [number, number]
    studyGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const TotalStudyCosts: React.FC<CesationCostsProps> = ({ tableYears, studyGridRef, alignedGridsRef }) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { projectCase, activeTabCase } = useCaseContext()
    const projectId = project?.id || null

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const totalFeasibilityAndConceptStudiesData = apiData?.totalFeasibilityAndConceptStudies
    const totalFeasibilityAndConceptStudiesOverrideData = apiData?.totalFeasibilityAndConceptStudiesOverride
    const totalFEEDStudiesData = apiData?.totalFEEDStudies
    const totalFEEDStudiesOverrideData = apiData?.totalFEEDStudiesOverride
    const totalOtherStudiesData = apiData?.totalOtherStudies

    if (!projectCase) { return null }

    const studyTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & conceptual stud.",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudiesData,
            resourceName: "totalFeasibilityAndConceptStudiesOverride",
            resourceId: projectCase.id,
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
            resourceId: projectCase.id,
            resourceProfileId: totalFEEDStudiesOverrideData?.id,
            resourcePropertyKey: "totalFEEDStudiesOverride",
            overridable: true,
            overrideProfile: totalFEEDStudiesOverrideData,
            editable: true,
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudiesData,
            resourceName: "productionProfileOil",
            resourceId: projectCase?.id,
            resourceProfileId: totalOtherStudiesData?.id,
            resourcePropertyKey: "totalOtherStudies",
            editable: true,
            overridable: true,
        },
    ]

    return (
        <CaseTabTable
            timeSeriesData={studyTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
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
