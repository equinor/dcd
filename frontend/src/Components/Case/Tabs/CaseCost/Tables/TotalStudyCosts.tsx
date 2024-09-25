import React, { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"
import { useAppContext } from "../../../../../Context/AppContext"
import { projectQueryFn } from "../../../../../Services/QueryFunctions"

interface TotalStudyCostsProps {
    tableYears: [number, number];
    studyGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
    addEdit: any;
}

const TotalStudyCosts: React.FC<TotalStudyCostsProps> = ({
    tableYears,
    studyGridRef,
    alignedGridsRef,
    apiData,
    addEdit,
}) => {
    const { isCalculatingTotalStudyCostOverrides } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const calculatedFields = [
        "totalFeasibilityAndConceptStudiesOverride",
        "totalFEEDStudiesOverride",
    ]

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
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
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
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
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
    }, [apiData, projectData])

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
            ongoingCalculation={isCalculatingTotalStudyCostOverrides}
            calculatedFields={calculatedFields}
            addEdit={addEdit}
        />
    )
}

export default TotalStudyCosts
