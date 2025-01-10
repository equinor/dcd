import React, {
    useEffect,
    useState,
    useRef,
    useMemo,
    useCallback,
    ChangeEvent,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { Switch, Tooltip } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"

import { useAppContext } from "@/Context/AppContext"
import { cellStyleRightAlign } from "@/Utils/common"
import useEditProject from "@/Hooks/useEditProject"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"

interface CO2Data {
    profile: string
    unit: string
    set: React.Dispatch<React.SetStateAction<number | undefined>>
    value: number | undefined
}

const CO2ListTechnicalInput = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()

    const [check, setCheck] = useState(false)

    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>()
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>()
    const [cO2Vented, setCO2Vented] = useState<number>()
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>()
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>()

    const [rowData, setRowData] = useState([{}])
    const { editMode } = useAppContext()
    const { addProjectEdit } = useEditProject()

    let cO2VentedRow = true

    const getColumnDefs = useCallback((edit: boolean): ColDef[] => ([
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
            editable: edit,
            cellClass: edit ? "editableCell" : undefined,
            cellStyle: cellStyleRightAlign,
        },
    ]), [editMode])

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(getColumnDefs(editMode))

    useEffect(() => {
        setColumnDefs(getColumnDefs(editMode))
    }, [editMode])

    useEffect(() => {
        if (revisionAndProjectData) {
            setCO2RemovedFromGas(revisionAndProjectData.commonProjectAndRevisionData.cO2RemovedFromGas)
            setCO2EmissionsFromFuelGas(revisionAndProjectData.commonProjectAndRevisionData.cO2EmissionFromFuelGas)
            setFlaredGasPerProducedVolume(revisionAndProjectData.commonProjectAndRevisionData.flaredGasPerProducedVolume)
            setCO2EmissionsFromFlaredGas(revisionAndProjectData.commonProjectAndRevisionData.cO2EmissionsFromFlaredGas)
            setCO2Vented(revisionAndProjectData.commonProjectAndRevisionData.cO2Vented)
            setAverageDevelopmentWellDrillingDays(revisionAndProjectData.commonProjectAndRevisionData.averageDevelopmentDrillingDays)
            setDailyEmissionsFromDrillingRig(revisionAndProjectData.commonProjectAndRevisionData.dailyEmissionFromDrillingRig)
        }
    }, [revisionAndProjectData])

    const toRowValue = (value: number | undefined) => {
        if (value !== undefined) {
            return Math.round(value * 100) / 100
        }
        return value
    }

    useEffect(() => {
        const co2Data: CO2Data[] = [
            {
                profile: "CO2 removed from the gas",
                unit: "% of design gas rate",
                set: setCO2RemovedFromGas,
                value: toRowValue(cO2RemovedFromGas),
            },
            {
                profile: "CO2-emissions from fuel gas",
                unit: "kg CO2/Sm³",
                set: setCO2EmissionsFromFuelGas,
                value: toRowValue(cO2EmissionsFromFuelGas),
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
                set: setCO2EmissionsFromFlaredGas,
                value: toRowValue(cO2EmissionsFromFlaredGas),
            },
            {
                profile: "CO2 vented",
                unit: "kg CO2/Sm³",
                set: setCO2Vented,
                value: toRowValue(cO2Vented),
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
        ]
        setRowData(co2Data)
    }, [
        cO2RemovedFromGas,
        cO2EmissionsFromFuelGas,
        flaredGasPerProducedVolume,
        cO2EmissionsFromFlaredGas,
        cO2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
    ])

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
            if (!node.data) { return true }

            if (cO2VentedRow) {
                return node.data.profile === "CO2 vented"
            }
            return node.data.profile !== "CO2 vented"
        },
        [cO2VentedRow],
    )

    const handleCellValueChange = (p: any) => {
        const newValue = Number(p.data.value.toString().replace(/,/g, "."))
        p.data.set(newValue)

        if (revisionAndProjectData && !Number.isNaN(newValue)) {
            const newProject: Components.Schemas.UpdateProjectDto = {
                ...revisionAndProjectData.commonProjectAndRevisionData,
            }

            switch (p.data.profile) {
            case "CO2 removed from the gas":
                newProject.cO2RemovedFromGas = newValue
                break
            case "CO2-emissions from fuel gas":
                newProject.cO2EmissionFromFuelGas = newValue
                break
            case "Flared gas per produced volume":
                newProject.flaredGasPerProducedVolume = newValue
                break
            case "CO2-emissions from flared gas":
                newProject.cO2EmissionsFromFlaredGas = newValue
                break
            case "CO2 vented":
                newProject.cO2Vented = newValue
                break
            case "Average development well drilling days":
                newProject.averageDevelopmentDrillingDays = newValue
                break
            case "Daily emissions from drilling rig":
                newProject.dailyEmissionFromDrillingRig = newValue
                break
            default:
                break
            }

            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }

    const defaultColDef = useMemo(() => ({
        sortable: false,
        filter: false,
        resizable: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
    }), [])

    const switchRow = () => {
        if (check) {
            return externalFilterChanged(true)
        }
        return externalFilterChanged(false)
    }

    if (!revisionAndProjectData) { return null }

    return (
        <Grid container spacing={1} justifyContent="flex-end">
            <Grid item>
                <Tooltip title={getEditDisabledText()}>
                    <Switch
                        disabled={isEditDisabled || !editMode}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            setCheck(e.target.checked)
                        }}
                        onClick={switchRow}
                        checked={check}
                        label={check ? "CO2 re-injected" : "CO2 vented"}
                    />
                </Tooltip>
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
                        singleClickEdit={editMode}
                    />
                </div>
            </Grid>
        </Grid>
    )
}

export default CO2ListTechnicalInput
