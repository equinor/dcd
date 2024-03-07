/* eslint-disable indent */
import {
    ChangeEvent,
    useCallback,
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { Switch } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../Context/ProjectContext"

const CO2ListTechnicalInput = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { project, setProject } = useProjectContext()

    if (!project) return null

    const [check, setCheck] = useState(false)
    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>(project.cO2RemovedFromGas ?? 0)
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>(project.cO2EmissionFromFuelGas ?? 0)
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>(project.flaredGasPerProducedVolume ?? 0)
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>(project.cO2EmissionsFromFlaredGas ?? 0)
    const [cO2Vented, setCO2Vented] = useState<number>(project.cO2Vented ?? 0)
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>(project.averageDevelopmentDrillingDays ?? 0)
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>(project.dailyEmissionFromDrillingRig ?? 0)
    const [rowData, setRowData] = useState([{}])

    let cO2VentedRow = true

    const [columnDefs] = useState<ColDef[]>([
        {
            field: "profile",
            headerName: "CO2 emission",
            width: 400,
            editable: false,
        },
        {
            field: "unit",
            headerName: "Unit",
            width: 200,
            editable: false,
        },
        {
            field: "value",
            headerName: "Value",
            width: 500,
            flex: 1,
            editable: true,
        },
    ])

    const co2Data = [
        {
            profile: "CO2 removed from the gas",
            unit: "% of design gas rate",
            set: setCO2RemovedFromGas,
            value: Math.round(cO2RemovedFromGas * 100) / 100,
        },
        {
            profile: "CO2-emissions from fuel gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFuelGas,
            value: Math.round(cO2EmissionsFromFuelGas * 100) / 100,
        },
        {
            profile: "Flared gas per produced volume",
            unit: "Sm³/boe",
            set: setFlaredGasPerProducedVolume,
            value: Math.round(flaredGasPerProducedVolume * 100) / 100,
        },
        {
            profile: "CO2-emissions from flared gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFlaredGas,
            value: Math.round(cO2EmissionsFromFlaredGas * 100) / 100,
        },
        {
            profile: "CO2 vented",
            unit: "kg CO2/Sm³",
            set: setCO2Vented,
            value: Math.round(cO2Vented * 100) / 100,
        },
        {
            profile: "Average development well drilling days",
            unit: "days/wells",
            set: setAverageDevelopmentWellDrillingDays,
            value: Math.round(averageDevelopmentWellDrillingDays * 100) / 100,
        },
        {
            profile: "Daily emissions from drilling rig",
            unit: "tonnes CO2/day",
            set: setDailyEmissionsFromDrillingRig,
            value: Math.round(dailyEmissionsFromDrillingRig * 100) / 100,
        },
    ]

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const externalFilterChanged = useCallback((newCO2VentedRow: boolean) => {
        cO2VentedRow = newCO2VentedRow
        gridRef.current.onFilterChanged()
    }, [])

    const isExternalFilterPresent = useCallback(
        (): boolean => cO2VentedRow === false,
        [],
    )

    const doesExternalFilterPass = useCallback(
        (node: any): boolean => {
            if (node.data) {
                switch (cO2VentedRow) {
                    case true:
                        return node.data.profile === "CO2 vented"
                    case false:
                        return node.data.profile !== "CO2 vented"
                    default:
                        return true
                }
            }
            return true
        },
        [cO2VentedRow],
    )

    const handleCellValueChange = (p: any) => {
        p.data.set(Number(p.data.value.toString().replace(/,/g, ".")))
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        onCellValueChanged: handleCellValueChange,
        suppressMenu: true,
    }), [])

    const switchRow = () => {
        if (check) {
            return externalFilterChanged(true)
        }
        return externalFilterChanged(false)
    }

    const switchLabel = () => {
        if (check) return "CO2 re-injected"

        return "CO2 vented"
    }

    useEffect(() => {
        setRowData(co2Data)

        if (project) {
            const newProject: Components.Schemas.ProjectDto = { ...project }
            newProject.cO2RemovedFromGas = cO2RemovedFromGas
            newProject.cO2EmissionFromFuelGas = cO2EmissionsFromFuelGas
            newProject.flaredGasPerProducedVolume = flaredGasPerProducedVolume
            newProject.cO2EmissionsFromFlaredGas = cO2EmissionsFromFlaredGas
            newProject.cO2Vented = cO2Vented
            newProject.averageDevelopmentDrillingDays = averageDevelopmentWellDrillingDays
            newProject.dailyEmissionFromDrillingRig = dailyEmissionsFromDrillingRig
            setProject(newProject)
        }
    }, [
        cO2RemovedFromGas,
        cO2EmissionsFromFlaredGas,
        flaredGasPerProducedVolume,
        cO2EmissionsFromFuelGas,
        cO2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
    ])

    return (
        <Grid container spacing={1} justifyContent="flex-end">
            <Grid item>
                <Switch
                    onChange={(e: ChangeEvent<HTMLInputElement>) => {
                        setCheck(e.target.checked)
                    }}
                    onClick={switchRow}
                    checked={check}
                    label={switchLabel()}
                />
            </Grid>
            <Grid item xs={12} className={styles.root}>
                <div
                    style={{
                        display: "flex", flexDirection: "column", width: "100%",
                    }}
                >
                    <AgGridReact
                        ref={gridRef}
                        rowData={rowData}
                        columnDefs={columnDefs}
                        defaultColDef={defaultColDef}
                        animateRows
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        isExternalFilterPresent={isExternalFilterPresent}
                        doesExternalFilterPass={doesExternalFilterPass}
                    />
                </div>
            </Grid>
        </Grid>
    )
}

export default CO2ListTechnicalInput
