import {
    ChangeEvent,
    useCallback,
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { Switch, Tooltip } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useParams } from "react-router"

import { useAppContext } from "@/Context/AppContext"
import { cellStyleRightAlign } from "@/Utils/common"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useProjectContext } from "@/Context/ProjectContext"

const CO2ListTechnicalInput = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()


    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    console.log(apiRevisionData)

    const [check, setCheck] = useState(false)
    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>(apiData?.cO2RemovedFromGas ?? 0)
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>(apiData?.cO2EmissionFromFuelGas ?? 0)
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>(apiData?.flaredGasPerProducedVolume ?? 0)
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>(apiData?.cO2EmissionsFromFlaredGas ?? 0)
    const [cO2Vented, setCO2Vented] = useState<number>(apiData?.cO2Vented ?? 0)
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>(apiData?.averageDevelopmentDrillingDays ?? 0)
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>(apiData?.dailyEmissionFromDrillingRig ?? 0)
    const [rowData, setRowData] = useState([{}])
    const { editMode } = useAppContext()
    const { addProjectEdit } = useEditProject()

    let cO2VentedRow = true

    const getColumnDefs = (edit: boolean): ColDef[] => ([
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
    ])

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(getColumnDefs(editMode))

    useEffect(() => {
        setColumnDefs(getColumnDefs(editMode))
    }, [editMode])

    const co2Data = [
        {
            profile: "CO2 removed from the gas",
            unit: "% of design gas rate",
            set: setCO2RemovedFromGas,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.cO2RemovedFromGas * 100) / 100 : Math.round(cO2RemovedFromGas * 100) / 100,
        },
        {
            profile: "CO2-emissions from fuel gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFuelGas,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.cO2EmissionFromFuelGas * 100) / 100 : Math.round(cO2EmissionsFromFuelGas * 100) / 100,
        },
        {
            profile: "Flared gas per produced volume",
            unit: "Sm³/Sm³",
            set: setFlaredGasPerProducedVolume,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.flaredGasPerProducedVolume * 100) / 100 : Math.round(flaredGasPerProducedVolume * 100) / 100,
        },
        {
            profile: "CO2-emissions from flared gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFlaredGas,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.cO2EmissionsFromFlaredGas * 100) / 100 : Math.round(cO2EmissionsFromFlaredGas * 100) / 100,
        },
        {
            profile: "CO2 vented",
            unit: "kg CO2/Sm³",
            set: setCO2Vented,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.cO2Vented * 100) / 100 : Math.round(cO2Vented * 100) / 100,
        },
        {
            profile: "Average development well drilling days",
            unit: "days/wells",
            set: setAverageDevelopmentWellDrillingDays,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.averageDevelopmentDrillingDays * 100) / 100 : Math.round(averageDevelopmentWellDrillingDays * 100) / 100,
        },
        {
            profile: "Daily emissions from drilling rig",
            unit: "tonnes CO2/day",
            set: setDailyEmissionsFromDrillingRig,
            value: isRevision && apiRevisionData ? Math.round(apiRevisionData?.dailyEmissionFromDrillingRig * 100) / 100 : Math.round(dailyEmissionsFromDrillingRig * 100) / 100,
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
        suppressHeaderMenuButton: true,
    }), [])

    const switchRow = () => {
        if (check) {
            return externalFilterChanged(true)
        }
        return externalFilterChanged(false)
    }

    const switchLabel = () => {
        if (check) { return "CO2 re-injected" }

        return "CO2 vented"
    }

    useEffect(() => {
        setRowData(co2Data)

        if (apiData && editMode) {
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...apiData }
            newProject.cO2RemovedFromGas = cO2RemovedFromGas
            newProject.cO2EmissionFromFuelGas = cO2EmissionsFromFuelGas
            newProject.flaredGasPerProducedVolume = flaredGasPerProducedVolume
            newProject.cO2EmissionsFromFlaredGas = cO2EmissionsFromFlaredGas
            newProject.cO2Vented = cO2Vented
            newProject.averageDevelopmentDrillingDays = averageDevelopmentWellDrillingDays
            newProject.dailyEmissionFromDrillingRig = dailyEmissionsFromDrillingRig
            addProjectEdit(apiData.id, newProject)
        }
    }, [
        cO2RemovedFromGas,
        cO2EmissionsFromFlaredGas,
        flaredGasPerProducedVolume,
        cO2EmissionsFromFuelGas,
        cO2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
        editMode,
        isRevision
    ])

    if (!apiData) { return null }

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
                    label={switchLabel()}
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
