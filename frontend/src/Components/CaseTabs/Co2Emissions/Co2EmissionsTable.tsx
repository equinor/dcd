import { ColDef } from "@ag-grid-community/core"
import { AgGridReact } from "@ag-grid-community/react"
import { Switch, Tooltip, Typography } from "@equinor/eds-core-react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import Grid from "@mui/material/Grid2"
import React, {
    useEffect,
    useState,
    useRef,
    useMemo,
    useCallback,
    ChangeEvent,
} from "react"
import styled from "styled-components"

import { useCaseApiData, useCaseMutation, useTopsideMutation } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { parseDecimalInput, roundToDecimals } from "@/Utils/FormatingUtils"
import { cellStyleRightAlign, getCustomContextMenuItems } from "@/Utils/TableUtils"

const StyledContainer = styled.div`
    display: flex;
    flex-direction: row;
    width: 100%;
    justify-content: space-between;
    align-items: center;
`

interface CO2Data {
    profile: string
    unit: string
    set: React.Dispatch<React.SetStateAction<number | undefined>>
    value: number | undefined
}

const Header = styled(Grid)`
    margin-bottom: 44px;
    gap: 16px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
`

const Co2EmissionsTable = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { canEdit, isEditDisabled, getEditDisabledText } = useCanUserEdit()
    const { apiData } = useCaseApiData()

    const {
        updateCo2RemovedFromGas,
        updateCo2EmissionFromFuelGas,
        updateFlaredGasPerProducedVolume,
        updateCo2EmissionsFromFlaredGas,
        updateCo2Vented,
        updateAverageDevelopmentDrillingDays,
        updateDailyEmissionFromDrillingRig,
    } = useCaseMutation()

    const {
        updateFuelConsumption,
    } = useTopsideMutation()

    const [isCo2Reinjected, setIsCo2Reinjected] = useState(false)

    const [fuelConsumption, setFuelConsumption] = useState<number>()
    const [co2RemovedFromGas, setCo2RemovedFromGas] = useState<number>()
    const [co2EmissionsFromFuelGas, setCo2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [co2EmissionsFromFlaredGas, setCo2EmissionsFromFlaredGas] = useState<number>()
    const [co2Vented, setCo2Vented] = useState<number>()
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>()
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>()

    const { editMode } = useAppStore()

    let co2VentedRow = true

    const columnDefs = useMemo(() => [
        {
            field: "profile",
            headerName: "CO2 emission",
            flex: 2,
            editable: false,
        },
        {
            field: "unit",
            headerName: "Unit",
            flex: 1,
            editable: false,
        },
        {
            field: "value",
            headerName: "Value",
            flex: 1,
            editable: canEdit(),
            cellClass: canEdit() ? "editableCell" : undefined,
            cellStyle: cellStyleRightAlign,
        },
    ] as ColDef[], [editMode, isEditDisabled])

    useEffect(() => {
        if (apiData) {
            setFuelConsumption(apiData.topside.fuelConsumption)
            setCo2RemovedFromGas(apiData.case.co2RemovedFromGas)
            setCo2EmissionsFromFuelGas(apiData.case.co2EmissionFromFuelGas)
            setFlaredGasPerProducedVolume(apiData.case.flaredGasPerProducedVolume)
            setCo2EmissionsFromFlaredGas(apiData.case.co2EmissionsFromFlaredGas)
            setCo2Vented(apiData.case.co2Vented)
            setIsCo2Reinjected(apiData.case.co2Vented === 0)
            setAverageDevelopmentWellDrillingDays(apiData.case.averageDevelopmentDrillingDays)
            setDailyEmissionsFromDrillingRig(apiData.case.dailyEmissionFromDrillingRig)
        }
    }, [apiData])

    const handleSwitchChange = (e: ChangeEvent<HTMLInputElement>) => {
        const isChecked = e.target.checked

        setIsCo2Reinjected(isChecked)
        setCo2Vented(isChecked ? 0 : 1.96)
    }

    const toRowValue = (value: number | undefined) => {
        if (value !== undefined) {
            return roundToDecimals(value, 2)
        }

        return value
    }

    const rowData = useMemo(() => [
        {
            profile: "Fuel gas consumption from Prosp (Scope 1)",
            unit: "million Sm³ gas/sd",
            set: setFuelConsumption,
            value: toRowValue(fuelConsumption),
        },
        {
            profile: "CO2 removed from the gas",
            unit: "% of design gas rate",
            set: setCo2RemovedFromGas,
            value: toRowValue(co2RemovedFromGas),
        },
        {
            profile: "CO2-emissions from fuel gas",
            unit: "kg CO2/Sm³",
            set: setCo2EmissionsFromFuelGas,
            value: toRowValue(co2EmissionsFromFuelGas),
        },
        {
            profile: "Flared gas per produced volume",
            unit: "Sm³/Sm³",
            set: setFlaredGasPerProducedVolume,
            value: toRowValue(flaredGasPerProducedVolume),
        },
        {
            profile: "CO2-emissions from flared gas",
            unit: "kg CO2/Sm³",
            set: setCo2EmissionsFromFlaredGas,
            value: toRowValue(co2EmissionsFromFlaredGas),
        },
        {
            profile: "CO2 vented",
            unit: "kg CO2/Sm³",
            set: setCo2Vented,
            value: toRowValue(co2Vented),
        },
        {
            profile: "Average development well drilling days",
            unit: "days/wells",
            set: setAverageDevelopmentWellDrillingDays,
            value: toRowValue(averageDevelopmentWellDrillingDays),
        },
        {
            profile: "Daily emissions from drilling rig",
            unit: "tonnes CO2/day",
            set: setDailyEmissionsFromDrillingRig,
            value: toRowValue(dailyEmissionsFromDrillingRig),
        },
    ] as CO2Data[], [
        fuelConsumption,
        co2RemovedFromGas,
        co2EmissionsFromFuelGas,
        flaredGasPerProducedVolume,
        co2EmissionsFromFlaredGas,
        co2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
    ])

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const externalFilterChanged = useCallback((newCO2VentedRow: boolean) => {
        co2VentedRow = newCO2VentedRow
        gridRef.current.onFilterChanged()
    }, [])

    const isExternalFilterPresent = useCallback(
        (): boolean => co2VentedRow === false,
        [],
    )

    const doesExternalFilterPass = useCallback(
        (node: any): boolean => {
            if (!node.data) { return true }

            if (co2VentedRow) {
                return node.data.profile === "CO2 vented"
            }

            return node.data.profile !== "CO2 vented"
        },
        [co2VentedRow],
    )

    const handleCellValueChange = (p: any) => {
        const value = p.data.value === null ? 0 : p.data.value

        p.data.set(parseDecimalInput(value))
    }

    const defaultColDef = useMemo(() => ({
        sortable: false,
        filter: false,
        resizable: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
        suppressKeyboardEvent: (params: any) => {
            if (params.event.key === "Enter") {
                gridRef.current?.stopEditing()

                return true
            }

            return false
        },
    }), [])

    const switchRow = () => {
        if (isCo2Reinjected) {
            return externalFilterChanged(true)
        }

        return externalFilterChanged(false)
    }

    useEffect(() => {
        if (apiData && canEdit() && fuelConsumption !== undefined) {
            if (fuelConsumption !== apiData.topside.fuelConsumption) {
                updateFuelConsumption(apiData.topside.id, fuelConsumption)
            }
        }
    }, [fuelConsumption])

    useEffect(() => {
        if (apiData && canEdit() && co2RemovedFromGas !== undefined) {
            if (co2RemovedFromGas !== apiData.case.co2RemovedFromGas) {
                updateCo2RemovedFromGas(co2RemovedFromGas)
            }
        }
    }, [co2RemovedFromGas])

    useEffect(() => {
        if (apiData && canEdit() && co2EmissionsFromFlaredGas !== undefined) {
            if (co2EmissionsFromFlaredGas !== apiData.case.co2EmissionsFromFlaredGas) {
                updateCo2EmissionsFromFlaredGas(co2EmissionsFromFlaredGas)
            }
        }
    }, [co2EmissionsFromFlaredGas])

    useEffect(() => {
        if (apiData && canEdit() && co2EmissionsFromFuelGas !== undefined) {
            if (co2EmissionsFromFuelGas !== apiData.case.co2EmissionFromFuelGas) {
                updateCo2EmissionFromFuelGas(co2EmissionsFromFuelGas)
            }
        }
    }, [co2EmissionsFromFuelGas])

    useEffect(() => {
        if (apiData && canEdit() && co2Vented !== undefined) {
            if (co2Vented !== apiData.case.co2Vented) {
                updateCo2Vented(co2Vented)
            }
        }
    }, [co2Vented])

    useEffect(() => {
        if (apiData && canEdit() && averageDevelopmentWellDrillingDays !== undefined) {
            if (averageDevelopmentWellDrillingDays !== apiData.case.averageDevelopmentDrillingDays) {
                updateAverageDevelopmentDrillingDays(averageDevelopmentWellDrillingDays)
            }
        }
    }, [averageDevelopmentWellDrillingDays])

    useEffect(() => {
        if (apiData && canEdit() && dailyEmissionsFromDrillingRig !== undefined) {
            if (dailyEmissionsFromDrillingRig !== apiData.case.dailyEmissionFromDrillingRig) {
                updateDailyEmissionFromDrillingRig(dailyEmissionsFromDrillingRig)
            }
        }
    }, [dailyEmissionsFromDrillingRig])

    useEffect(() => {
        if (apiData && canEdit() && flaredGasPerProducedVolume !== undefined) {
            if (flaredGasPerProducedVolume !== apiData.case.flaredGasPerProducedVolume) {
                updateFlaredGasPerProducedVolume(flaredGasPerProducedVolume)
            }
        }
    }, [flaredGasPerProducedVolume])

    return (
        <Grid container spacing={1} justifyContent="flex-end">
            <StyledContainer>
                <Header>
                    <Typography variant="h3">CO2 emissions</Typography>
                    <Typography>
                        You can override these default assumption to customize the calculation made in CO2 emissions.
                    </Typography>
                </Header>
                <Tooltip title={getEditDisabledText()}>
                    <Switch
                        disabled={!canEdit()}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            handleSwitchChange(e)
                        }}
                        onClick={switchRow}
                        checked={isCo2Reinjected}
                        label={isCo2Reinjected ? "CO2 re-injected" : "CO2 vented"}
                    />
                </Tooltip>
            </StyledContainer>
            <Grid size={12} className={styles.root}>
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
                        getContextMenuItems={getCustomContextMenuItems}
                        animateRows
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        isExternalFilterPresent={isExternalFilterPresent}
                        doesExternalFilterPass={doesExternalFilterPass}
                        singleClickEdit={canEdit()}
                        stopEditingWhenCellsLoseFocus
                    />
                </div>
            </Grid>
        </Grid>
    )
}

export default Co2EmissionsTable
