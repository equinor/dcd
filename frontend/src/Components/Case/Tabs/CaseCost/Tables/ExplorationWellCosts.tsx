import React, { useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"
import { useModalContext } from "../../../../../Context/ModalContext"

interface ExplorationWellCostsProps {
    tableYears: [number, number]
    explorationWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const ExplorationWellCosts: React.FC<ExplorationWellCostsProps> = ({
    tableYears,
    explorationWellsGridRef,
    alignedGridsRef,
}) => {
    const { project } = useProjectContext()
    const {
        exploration,
        setExploration,
    } = useModalContext()
    const {
        projectCase,
        activeTabCase,

        explorationWellCostProfile,
        setExplorationWellCostProfile,

        gAndGAdminCost,
        setGAndGAdminCost,

        seismicAcquisitionAndProcessing,
        setSeismicAcquisitionAndProcessing,

        sidetrackCostProfile,
        setSidetrackCostProfile,

        appraisalWellCostProfile,
        setAppraisalWellCostProfile,

        countryOfficeCost,
        setCountryOfficeCost,
    } = useCaseContext()

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin costs",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: gAndGAdminCost,
            // set: setGAndGAdminCost,  this was not used in the original code but should it?
        },
        {
            profileName: "Seismic acquisition and processing",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: seismicAcquisitionAndProcessing,
            set: setSeismicAcquisitionAndProcessing,
        },
        {
            profileName: "Country office cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: countryOfficeCost,
            set: setCountryOfficeCost,
        },
        {
            profileName: "Exploration well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationWellCostProfile,
            set: setExplorationWellCostProfile,
        },
        {
            profileName: "Appraisal well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: appraisalWellCostProfile,
            set: setAppraisalWellCostProfile,
        },
        {
            profileName: "Sidetrack well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: sidetrackCostProfile,
            set: setSidetrackCostProfile,
        },
    ]

    useEffect(() => {
        if (explorationWellsGridRef.current
            && explorationWellsGridRef.current.api
            && explorationWellsGridRef.current.api.refreshCells) {
            explorationWellsGridRef.current.api.refreshCells()

            console.log("Refreshing exploration wells grid")
        }
    }, [gAndGAdminCost])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "explorationWellCostProfile", explorationWellCostProfile)
        }
    }, [explorationWellCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "appraisalWellCostProfile", appraisalWellCostProfile)
        }
    }, [appraisalWellCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "sidetrackCostProfile", sidetrackCostProfile)
        }
    }, [sidetrackCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "seismicAcquisitionAndProcessing", seismicAcquisitionAndProcessing)
        }
    }, [seismicAcquisitionAndProcessing])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "countryOfficeCost", countryOfficeCost)
        }
    }, [countryOfficeCost])

    useEffect(() => {
        if (activeTabCase === 5 && exploration) {
            setSeismicAcquisitionAndProcessing(exploration.seismicAcquisitionAndProcessing)
            setExplorationWellCostProfile(exploration.explorationWellCostProfile)
            setAppraisalWellCostProfile(exploration.appraisalWellCostProfile)
            setSidetrackCostProfile(exploration.sidetrackCostProfile)
            setCountryOfficeCost(exploration.countryOfficeCost)
            setGAndGAdminCost(exploration.gAndGAdminCost)
        }
    }, [activeTabCase])

    return (
        <CaseTabTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Exploration well costs"
            gridRef={explorationWellsGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total exploration cost"
        />
    )
}

export default ExplorationWellCosts
