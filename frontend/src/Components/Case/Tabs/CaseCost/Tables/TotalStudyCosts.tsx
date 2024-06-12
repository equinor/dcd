import React, { useState, useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"

interface CesationCostsProps {
    tableYears: [number, number]
    studyGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const TotalStudyCosts: React.FC<CesationCostsProps> = ({ tableYears, studyGridRef, alignedGridsRef }) => {
    const { project } = useProjectContext()
    const {
        projectCase,
        activeTabCase,
        projectCaseEdited,
        setProjectCaseEdited,

        totalFeasibilityAndConceptStudiesOverride,
        setTotalFeasibilityAndConceptStudiesOverride, // why is this in context while other overrides are local states?

        totalFEEDStudiesOverride,
        setTotalFEEDStudiesOverride,

        totalFeasibilityAndConceptStudies,
        setTotalFeasibilityAndConceptStudies,

        totalFEEDStudies,
        setTotalFEEDStudies,

        totalOtherStudies,
        setTotalOtherStudies,
    } = useCaseContext()

    const studyTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & conceptual stud.",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudies,
            overridable: true,
            overrideProfile: totalFeasibilityAndConceptStudiesOverride,
            overrideProfileSet: setTotalFeasibilityAndConceptStudiesOverride,
        },
        {
            profileName: "FEED studies (DG2-DG3)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudies,
            overridable: true,
            overrideProfile: totalFEEDStudiesOverride,
            overrideProfileSet: setTotalFEEDStudiesOverride,
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudies,
            set: setTotalOtherStudies,
        },
    ]

    useEffect(() => {
        if (activeTabCase === 5 && projectCase) {
            setTotalOtherStudies(projectCase.totalOtherStudies)

            setTotalFeasibilityAndConceptStudies(projectCase.totalFeasibilityAndConceptStudies)
            setTotalFeasibilityAndConceptStudiesOverride(projectCase.totalFeasibilityAndConceptStudiesOverride)

            setTotalFEEDStudies(projectCase.totalFEEDStudies)
            setTotalFEEDStudiesOverride(projectCase.totalFEEDStudiesOverride)
        }
    }, [activeTabCase])

    useEffect(() => {
        if (studyGridRef.current
            && studyGridRef.current.api
            && studyGridRef.current.api.refreshCells) {
            studyGridRef.current.api.refreshCells()
        }
    }, [totalFeasibilityAndConceptStudies, totalFEEDStudies, totalOtherStudies])

    useEffect(() => {
        if (projectCaseEdited && totalFeasibilityAndConceptStudiesOverride && projectCaseEdited.totalFeasibilityAndConceptStudiesOverride !== totalFeasibilityAndConceptStudiesOverride) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalFeasibilityAndConceptStudiesOverride", totalFeasibilityAndConceptStudiesOverride)
        }
    }, [projectCaseEdited, totalFeasibilityAndConceptStudiesOverride])

    useEffect(() => {
        if (projectCaseEdited && totalFEEDStudiesOverride && projectCaseEdited.totalFEEDStudiesOverride !== totalFEEDStudiesOverride) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalFEEDStudiesOverride", totalFEEDStudiesOverride)
        }
    }, [projectCaseEdited, totalFEEDStudiesOverride])

    useEffect(() => {
        if (projectCaseEdited && totalOtherStudies && projectCaseEdited.totalOtherStudies !== totalOtherStudies) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalOtherStudies", totalOtherStudies)
        }
    }, [projectCaseEdited, totalOtherStudies])

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
