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
import { Switch, Tooltip, Typography } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid2"
import styled from "styled-components"

import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { cellStyleRightAlign } from "@/Utils/common"
import useEditProject from "@/Hooks/useEditProject"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useDataFetch } from "@/Hooks"

const StyledContainer = styled.div`
    display: flex;
    flex-direction: row;
    width: 100%;
    justify-content: space-between;
    align-items: center;
    margin-top: 20px;
    padding: 0 10px;
`

interface CO2Data {
    profile: string
    unit: string
    set: React.Dispatch<React.SetStateAction<number | undefined>>
    value: number | undefined
}

const CO2Tab = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { isRevision } = useProjectContext()
    const { canEdit, isEditDisabled, getEditDisabledText } = useCanUserEdit()
    const revisionAndProjectData = useDataFetch()

    const [check, setCheck] = useState(false)

    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>()
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>()
    const [cO2Vented, setCO2Vented] = useState<number>()
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>()
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>()

    const { editMode } = useAppStore()
    const { addProjectEdit } = useEditProject()

    let cO2VentedRow = true

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

    const rowData = useMemo(() => [
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
    ] as CO2Data[], [
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
        const value = p.data.value === null ? 0 : p.data.value
        p.data.set(Number(value.toString().replace(/,/g, ".")))
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
        if (check) {
            return externalFilterChanged(true)
        }
        return externalFilterChanged(false)
    }

    useEffect(() => {
        if (revisionAndProjectData
            && canEdit()
            && cO2RemovedFromGas !== undefined
            && cO2EmissionsFromFlaredGas !== undefined
            && cO2EmissionsFromFuelGas !== undefined
            && cO2Vented !== undefined
            && averageDevelopmentWellDrillingDays !== undefined
            && dailyEmissionsFromDrillingRig !== undefined
            && flaredGasPerProducedVolume !== undefined
        ) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
            newProject.cO2RemovedFromGas = cO2RemovedFromGas
            newProject.cO2EmissionFromFuelGas = cO2EmissionsFromFuelGas
            newProject.flaredGasPerProducedVolume = flaredGasPerProducedVolume
            newProject.cO2EmissionsFromFlaredGas = cO2EmissionsFromFlaredGas
            newProject.cO2Vented = cO2Vented
            newProject.averageDevelopmentDrillingDays = averageDevelopmentWellDrillingDays
            newProject.dailyEmissionFromDrillingRig = dailyEmissionsFromDrillingRig
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }, [
        cO2RemovedFromGas,
        cO2EmissionsFromFlaredGas,
        flaredGasPerProducedVolume,
        cO2EmissionsFromFuelGas,
        cO2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
        isRevision,
    ])

    if (!revisionAndProjectData) { return null }

    return (
        <Grid container spacing={1} justifyContent="flex-end">
            <StyledContainer>
                <Typography>
                    You can override these default assumption to customize the calculation made in CO2 emissions.
                </Typography>
                <Tooltip title={getEditDisabledText()}>
                    <Switch
                        disabled={!canEdit()}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => {
                            setCheck(e.target.checked)
                        }}
                        onClick={switchRow}
                        checked={check}
                        label={check ? "CO2 re-injected" : "CO2 vented"}
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

export default CO2Tab
