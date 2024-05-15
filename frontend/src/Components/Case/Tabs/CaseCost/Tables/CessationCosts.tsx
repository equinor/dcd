import React, { useState, useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"

interface CesationCostsProps {
    tableYears: [number, number]
    cessationGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}
const CessationCosts: React.FC<CesationCostsProps> = ({ tableYears, cessationGridRef, alignedGridsRef }) => {
    const { project } = useProjectContext()
    const {
        projectCase,
        activeTabCase,

        projectCaseEdited,
        setProjectCaseEdited,

        cessationWellsCost,
        setCessationWellsCost,

        cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost,

        cessationOnshoreFacilitiesCostProfile,
        setCessationOnshoreFacilitiesCostProfile,

        cessationOffshoreFacilitiesCostOverride,
        setCessationOffshoreFacilitiesCostOverride,
    } = useCaseContext()

    const [cessationWellsCostOverride, setCessationWellsCostOverride] = useState<Components.Schemas.CessationWellsCostOverrideDto>()

    const cessationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Cessation - Development wells",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationWellsCost,
            overridable: true,
            overrideProfile: cessationWellsCostOverride,
            overrideProfileSet: setCessationWellsCostOverride,
        },
        {
            profileName: "Cessation - Offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOffshoreFacilitiesCost,
            overridable: true,
            overrideProfile: cessationOffshoreFacilitiesCostOverride,
            overrideProfileSet: setCessationOffshoreFacilitiesCostOverride,
        },
        {
            profileName: "CAPEX - Cessation - Onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOnshoreFacilitiesCostProfile,
            set: setCessationOnshoreFacilitiesCostProfile,
        },
    ]

    useEffect(() => {
        if (cessationGridRef.current
            && cessationGridRef.current.api
            && cessationGridRef.current.api.refreshCells) {
            cessationGridRef.current.api.refreshCells()
        }
    }, [
        cessationWellsCost,
        cessationOffshoreFacilitiesCost,
        cessationOnshoreFacilitiesCostProfile,
    ])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "cessationWellsCostOverride", cessationWellsCostOverride)
        }
    }, [cessationWellsCostOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "cessationOffshoreFacilitiesCostOverride", cessationOffshoreFacilitiesCostOverride)
        }
    }, [cessationOffshoreFacilitiesCostOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "cessationOnshoreFacilitiesCostProfile", cessationOnshoreFacilitiesCostProfile)
        }
    }, [cessationOnshoreFacilitiesCostProfile])

    useEffect(() => {
        if (activeTabCase === 5 && projectCase) {
            setCessationWellsCost(projectCase.cessationWellsCost)
            setCessationWellsCostOverride(projectCase.cessationWellsCostOverride)

            setCessationOffshoreFacilitiesCost(projectCase?.cessationOffshoreFacilitiesCost)
            setCessationOffshoreFacilitiesCostOverride(projectCase.cessationOffshoreFacilitiesCostOverride)

            setCessationOnshoreFacilitiesCostProfile(projectCase?.cessationOnshoreFacilitiesCostProfile)
        }
    }, [activeTabCase])

    return (
        <CaseTabTable
            timeSeriesData={cessationTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
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
