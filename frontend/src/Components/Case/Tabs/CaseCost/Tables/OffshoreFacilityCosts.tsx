import React, { useState, useEffect } from "react"
import { ITimeSeriesData } from "../../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../../Context/CaseContext"
import CaseTabTable from "../../../Components/CaseTabTable"
import { updateObject } from "../../../../../Utils/common"

interface OffshoreFacillityCostsProps {
    tableYears: [number, number]
    capexGridRef: React.MutableRefObject<any>
    alignedGridsRef: any[]
}
const OffshoreFacillityCosts: React.FC<OffshoreFacillityCostsProps> = ({
    tableYears,
    capexGridRef,
    alignedGridsRef,
}) => {
    const { project } = useProjectContext()
    const {
        projectCase,
        activeTabCase,

        surfCost,
        setSurfCost,

        topsideCost,
        setTopsideCost,

        substructureCost,
        setSubstructureCost,

        transportCost,
        setTransportCost,

        surf,
        setSurf,

        topside,
        setTopside,

        substructure,
        setSubstructure,

        transport,
        setTransport,

    } = useCaseContext()

    // CAPEX
    const [topsideCostOverride, setTopsideCostOverride] = useState<Components.Schemas.TopsideCostProfileOverrideDto>()
    const [surfCostOverride, setSurfCostOverride] = useState<Components.Schemas.SurfCostProfileOverrideDto>()
    const [substructureCostOverride, setSubstructureCostOverride] = useState<Components.Schemas.SubstructureCostProfileOverrideDto>()
    const [transportCostOverride, setTransportCostOverride] = useState<Components.Schemas.TransportCostProfileOverrideDto>()

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Subsea production system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: surfCost,
            overridable: true,
            overrideProfile: surfCostOverride,
            overrideProfileSet: setSurfCostOverride,
        },
        {
            profileName: "Topside",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: topsideCost,
            overridable: true,
            overrideProfile: topsideCostOverride,
            overrideProfileSet: setTopsideCostOverride,
        },
        {
            profileName: "Substructure",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: substructureCost,
            overridable: true,
            overrideProfile: substructureCostOverride,
            overrideProfileSet: setSubstructureCostOverride,
        },
        {
            profileName: "Transport system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: transportCost,
            overridable: true,
            overrideProfile: transportCostOverride,
            overrideProfileSet: setTransportCostOverride,
        },
    ]

    useEffect(() => {
        if (surf && surfCostOverride) {
            updateObject(surf, setSurf, "costProfileOverride", surfCostOverride)
        }
    }, [surfCostOverride])

    useEffect(() => {
        if (topside && topsideCostOverride) {
            updateObject(topside, setTopside, "costProfileOverride", topsideCostOverride)
        }
    }, [topsideCostOverride])

    useEffect(() => {
        if (substructure && substructureCostOverride) {
            updateObject(substructure, setSubstructure, "costProfileOverride", substructureCostOverride)
        }
    }, [substructureCostOverride])

    useEffect(() => {
        if (transport && transportCostOverride) {
            updateObject(transport, setTransport, "costProfileOverride", transportCostOverride)
        }
    }, [transportCostOverride])

    useEffect(() => {
        if (surf && surfCost) {
            updateObject(surf, setSurf, "costProfile", surfCost)
        }
    }, [surfCost])

    useEffect(() => {
        if (topside && topsideCost) {
            updateObject(topside, setTopside, "costProfile", topsideCost)
        }
    }, [topsideCost])

    useEffect(() => {
        if (substructure && substructureCost) {
            updateObject(substructure, setSubstructure, "costProfile", substructureCost)
        }
    }, [substructureCost])

    useEffect(() => {
        if (transport && transportCost) {
            updateObject(transport, setTransport, "costProfile", transportCost)
        }
    }, [transportCost])

    useEffect(() => {
        if (activeTabCase === 5) {
            if (topside) {
                setTopsideCost(topside.costProfile)
                setTopsideCostOverride(topside.costProfileOverride)
            }

            if (surf) {
                setSurfCost(surf.costProfile)
                setSurfCostOverride(surf.costProfileOverride)
            }

            if (substructure) {
                setSubstructureCost(substructure.costProfile)
                setSubstructureCostOverride(substructure.costProfileOverride)
            }

            if (transport) {
                setTransportCost(transport.costProfile)
                setTransportCostOverride(transport.costProfileOverride)
            }
        }
    }, [activeTabCase])

    return (
        <CaseTabTable
            timeSeriesData={capexTimeSeriesData}
            dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Offshore facilitiy cost"
            gridRef={capexGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
        />
    )
}

export default OffshoreFacillityCosts
