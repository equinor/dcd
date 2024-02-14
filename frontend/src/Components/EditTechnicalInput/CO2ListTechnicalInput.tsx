import {
    ChangeEvent,
    Dispatch,
    SetStateAction,
    useCallback,
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { Switch } from "@equinor/eds-core-react"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"

const SwitchWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    margin-left: auto;
    z-index: 110;
`
const ColumnWrapper = styled.div`
    margin-top: 1rem;
    margin-left: 45rem;
    flex-direction: column;
    margin-bottom: 0.5rem;
`

interface Props {
    project: Components.Schemas.ProjectDto
    setProject: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>
}

const CO2ListTechnicalInput = ({
    project, setProject,
}: Props) => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()

    const [check, setCheck] = useState(false)

    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>(project.cO2RemovedFromGas ?? 0)
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>(project.cO2EmissionFromFuelGas ?? 0)
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>(
        project.flaredGasPerProducedVolume ?? 0,
    )
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>(
        project.cO2EmissionsFromFlaredGas ?? 0,
    )
    const [cO2Vented, setCO2Vented] = useState<number>(project.cO2Vented ?? 0)
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>(
        project.averageDevelopmentDrillingDays ?? 0,
    )
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>(
        project.dailyEmissionFromDrillingRig ?? 0,
    )

    let cO2VentedRow = true

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

    const [rowData, setRowData] = useState([{}])

    const handleCellValueChange = (p: any) => {
        p.data.set(Number(p.data.value.toString().replace(/,/g, ".")))
    }

    useEffect(() => {
        setRowData(co2Data)
    }, [cO2RemovedFromGas, cO2EmissionsFromFuelGas, flaredGasPerProducedVolume,
        cO2EmissionsFromFlaredGas, cO2Vented, averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig])

    useEffect(() => {
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
    }, [cO2RemovedFromGas, cO2EmissionsFromFuelGas, flaredGasPerProducedVolume,
        cO2EmissionsFromFuelGas, cO2Vented, averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        onCellValueChanged: handleCellValueChange,
        suppressMenu: true,
    }), [])

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

    const switchRow = () => {
        if (check) {
            return externalFilterChanged(true)
        }
        return externalFilterChanged(false)
    }

    const switchLabel = () => {
        if (check) {
            return "CO2 re-injected"
        }
        return "CO2 vented"
    }

    return (
        <>
            <ColumnWrapper>
                <SwitchWrapper>
                    <Switch
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            setCheck(e.target.checked)
                        }}
                        onClick={switchRow}
                        checked={check}
                        label={switchLabel()}
                    />
                </SwitchWrapper>
            </ColumnWrapper>
            <div className={styles.root}>
                <div
                    style={{
                        display: "flex", flexDirection: "column", width: "54rem",
                    }}
                    className="ag-theme-alpine-fusion"
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
            </div>
        </>
    )
}

export default CO2ListTechnicalInput
