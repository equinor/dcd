import React, { useState, useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import { useModalContext } from "../../../../../Context/ModalContext"
import { updateObject } from "../../../../../Utils/common"
import CaseTabTable from "../../../Components/CaseTabTable"

interface DevelopmentWellCostsProps {
    tableYears: [number, number]
    developmentWellsGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}

const DevelopmentWellCosts: React.FC<DevelopmentWellCostsProps> = ({
    tableYears,
    developmentWellsGridRef,
    alignedGridsRef,
}) => {
    const { project } = useProjectContext()
    const { wellProject, setWellProject } = useModalContext()
    const {
        activeTabCase,
        projectCase,
    } = useCaseContext()

    const [wellProjectOilProducerCost, setWellProjectOilProducerCost] = useState<Components.Schemas.OilProducerCostProfileDto>()
    const [wellProjectOilProducerCostOverride, setWellProjectOilProducerCostOverride] = useState<Components.Schemas.OilProducerCostProfileOverrideDto>()

    const [wellProjectGasProducerCost, setWellProjectGasProducerCost] = useState<Components.Schemas.GasProducerCostProfileDto>()
    const [wellProjectGasProducerCostOverride, setWellProjectGasProducerCostOverride] = useState<Components.Schemas.GasProducerCostProfileOverrideDto>()

    const [wellProjectWaterInjectorCost, setWellProjectWaterInjectorCost] = useState<Components.Schemas.WaterInjectorCostProfileDto>()
    const [wellProjectWaterInjectorCostOverride, setWellProjectWaterInjectorCostOverride] = useState<Components.Schemas.WaterInjectorCostProfileOverrideDto>()

    const [wellProjectGasInjectorCost, setWellProjectGasInjectorCost] = useState<Components.Schemas.GasInjectorCostProfileDto>()
    const [wellProjectGasInjectorCostOverride, setWellProjectGasInjectorCostOverride] = useState<Components.Schemas.GasInjectorCostProfileOverrideDto>()

    const developmentTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil producer",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectOilProducerCost,
            overridable: true,
            overrideProfile: wellProjectOilProducerCostOverride,
            overrideProfileSet: setWellProjectOilProducerCostOverride,
        },
        {
            profileName: "Gas producer",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectGasProducerCost,
            overridable: true,
            overrideProfile: wellProjectGasProducerCostOverride,
            overrideProfileSet: setWellProjectGasProducerCostOverride,
        },
        {
            profileName: "Water injector",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectWaterInjectorCost,
            overridable: true,
            overrideProfile: wellProjectWaterInjectorCostOverride,
            overrideProfileSet: setWellProjectWaterInjectorCostOverride,
        },
        {
            profileName: "Gas injector",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectGasInjectorCost,
            overridable: true,
            overrideProfile: wellProjectGasInjectorCostOverride,
            overrideProfileSet: setWellProjectGasInjectorCostOverride,
        },
    ]

    useEffect(() => {
        if (wellProject && wellProjectOilProducerCost) {
            updateObject(wellProject, setWellProject, "oilProducerCostProfile", wellProjectOilProducerCost)
        }
    }, [wellProjectOilProducerCost])

    useEffect(() => {
        if (wellProject && wellProjectOilProducerCostOverride) {
            updateObject(wellProject, setWellProject, "oilProducerCostProfileOverride", wellProjectOilProducerCostOverride)
        }
    }, [wellProjectOilProducerCostOverride])

    useEffect(() => {
        if (wellProject && wellProjectGasProducerCost) {
            updateObject(wellProject, setWellProject, "gasProducerCostProfile", wellProjectGasProducerCost)
        }
    }, [wellProjectGasProducerCost])

    useEffect(() => {
        if (wellProject && wellProjectGasProducerCostOverride) {
            updateObject(wellProject, setWellProject, "gasProducerCostProfileOverride", wellProjectGasProducerCostOverride)
        }
    }, [wellProjectGasProducerCostOverride])

    useEffect(() => {
        if (wellProject && wellProjectWaterInjectorCost) {
            updateObject(wellProject, setWellProject, "waterInjectorCostProfile", wellProjectWaterInjectorCost)
        }
    }, [wellProjectWaterInjectorCost])

    useEffect(() => {
        if (wellProject && wellProjectWaterInjectorCostOverride) {
            updateObject(wellProject, setWellProject, "waterInjectorCostProfileOverride", wellProjectWaterInjectorCostOverride)
        }
    }, [wellProjectWaterInjectorCostOverride])

    useEffect(() => {
        if (wellProject && wellProjectGasInjectorCost) {
            updateObject(wellProject, setWellProject, "gasInjectorCostProfile", wellProjectGasInjectorCost)
        }
    }, [wellProjectGasInjectorCost])

    useEffect(() => {
        if (wellProject && wellProjectGasInjectorCostOverride) {
            updateObject(wellProject, setWellProject, "gasInjectorCostProfileOverride", wellProjectGasInjectorCostOverride)
        }
    }, [wellProjectGasInjectorCostOverride])

    useEffect(() => {
        if (activeTabCase === 5 && wellProject) {
            setWellProjectOilProducerCost(wellProject.oilProducerCostProfile)
            setWellProjectOilProducerCostOverride(wellProject.oilProducerCostProfileOverride)

            setWellProjectGasProducerCost(wellProject.gasProducerCostProfile)
            setWellProjectGasProducerCostOverride(wellProject.gasProducerCostProfileOverride)

            setWellProjectWaterInjectorCost(wellProject.waterInjectorCostProfile)
            setWellProjectWaterInjectorCostOverride(wellProject.waterInjectorCostProfileOverride)

            setWellProjectGasInjectorCost(wellProject.gasInjectorCostProfile)
            setWellProjectGasInjectorCostOverride(wellProject.gasInjectorCostProfileOverride)
        }
    }, [activeTabCase])

    return (
        <CaseTabTable
            timeSeriesData={developmentTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Development well cost"
            gridRef={developmentWellsGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default DevelopmentWellCosts
