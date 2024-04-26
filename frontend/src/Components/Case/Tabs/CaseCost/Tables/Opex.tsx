import React, { useState, useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"

interface OpexProps {
    tableYears: [number, number]
    opexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const Opex: React.FC<OpexProps> = ({ tableYears, opexGridRef, alignedGridsRef }) => {
    const { project } = useProjectContext()
    const {
        historicCostCostProfile,
        setHistoricCostCostProfile,
        wellInterventionCostProfile,
        setWellInterventionCostProfile,
        offshoreFacilitiesOperationsCostProfile,
        setOffshoreFacilitiesOperationsCostProfile,
        onshoreRelatedOPEXCostProfile,
        setOnshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
        setAdditionalOPEXCostProfile,
    } = useCaseContext()

    const {
        projectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        activeTabCase,
    } = useCaseContext()

    const [offshoreFacilitiesOperationsCostProfileOverride, setOffshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()
    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic Cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCostCostProfile,
            set: setHistoricCostCostProfile,
        },
        {
            profileName: "Well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellInterventionCostProfile,
            overridable: true,
            overrideProfile: wellInterventionCostProfileOverride,
            overrideProfileSet: setWellInterventionCostProfileOverride,
        },
        {
            profileName: "Offshore facilities operations",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: offshoreFacilitiesOperationsCostProfile,
            overridable: true,
            overrideProfile: offshoreFacilitiesOperationsCostProfileOverride,
            overrideProfileSet: setOffshoreFacilitiesOperationsCostProfileOverride,
        },
        {
            profileName: "Onshore related OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: onshoreRelatedOPEXCostProfile,
            set: setOnshoreRelatedOPEXCostProfile,
        },
        {
            profileName: "Additional OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: additionalOPEXCostProfile,
            set: setAdditionalOPEXCostProfile,
        },
    ]

    useEffect(() => {
        if (opexGridRef.current
            && opexGridRef.current.api
            && opexGridRef.current.api.refreshCells) {
            opexGridRef.current.api.refreshCells()

            console.log("Refreshing opex grid")
        }
    }, [
        offshoreFacilitiesOperationsCostProfile,
        wellInterventionCostProfile,
        historicCostCostProfile,
        onshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
    ])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "wellInterventionCostProfileOverride", wellInterventionCostProfileOverride)
        }
    }, [wellInterventionCostProfileOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "offshoreFacilitiesOperationsCostProfileOverride", offshoreFacilitiesOperationsCostProfileOverride)
        }
    }, [offshoreFacilitiesOperationsCostProfileOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "onshoreRelatedOPEXCostProfile", onshoreRelatedOPEXCostProfile)
        }
    }, [onshoreRelatedOPEXCostProfile])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "historicCostCostProfile", historicCostCostProfile)
        }
    }, [historicCostCostProfile])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "additionalOPEXCostProfile", additionalOPEXCostProfile)
        }
    }, [additionalOPEXCostProfile])

    useEffect(() => {
        if (activeTabCase === 5 && projectCase) {
            setWellInterventionCostProfile(projectCase.wellInterventionCostProfile)
            setWellInterventionCostProfileOverride(projectCase.wellInterventionCostProfileOverride)

            setOffshoreFacilitiesOperationsCostProfile(projectCase.offshoreFacilitiesOperationsCostProfile)
            setOffshoreFacilitiesOperationsCostProfileOverride(projectCase.offshoreFacilitiesOperationsCostProfileOverride)

            setHistoricCostCostProfile(projectCase.historicCostCostProfile)
            setOnshoreRelatedOPEXCostProfile(projectCase.onshoreRelatedOPEXCostProfile)
            setAdditionalOPEXCostProfile(projectCase.additionalOPEXCostProfile)
        }
    }, [activeTabCase])

    return (
        <CaseTabTable
            timeSeriesData={opexTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="OPEX"
            gridRef={opexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total OPEX cost"
        />
    )
}

export default Opex
