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
import { cellStyleRightAlign } from "@/Utils/commonUtils"
import useEditProject from "@/Hooks/useEditProject"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useDataFetch } from "@/Hooks"

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

const CO2Tab = () => {
    const gridRef = useRef<any>(null)
    const styles = useStyles()
    const { isRevision } = useProjectContext()
    const { canEdit, isEditDisabled, getEditDisabledText } = useCanUserEdit()
    const revisionAndProjectData = useDataFetch()

    const [check, setCheck] = useState(false)

    const [co2RemovedFromGas, setCo2RemovedFromGas] = useState<number>()
    const [co2EmissionsFromFuelGas, setCo2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [co2EmissionsFromFlaredGas, setCo2EmissionsFromFlaredGas] = useState<number>()
    const [co2Vented, setCo2Vented] = useState<number>()
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>()
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>()

    const { editMode } = useAppStore()
    const { addProjectEdit } = useEditProject()

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
        if (revisionAndProjectData) {
            setCo2RemovedFromGas(revisionAndProjectData.commonProjectAndRevisionData.co2RemovedFromGas)
            setCo2EmissionsFromFuelGas(revisionAndProjectData.commonProjectAndRevisionData.co2EmissionFromFuelGas)
            setFlaredGasPerProducedVolume(revisionAndProjectData.commonProjectAndRevisionData.flaredGasPerProducedVolume)
            setCo2EmissionsFromFlaredGas(revisionAndProjectData.commonProjectAndRevisionData.co2EmissionsFromFlaredGas)
            setCo2Vented(revisionAndProjectData.commonProjectAndRevisionData.co2Vented)
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
            && co2RemovedFromGas !== undefined
            && co2EmissionsFromFlaredGas !== undefined
            && co2EmissionsFromFuelGas !== undefined
            && co2Vented !== undefined
            && averageDevelopmentWellDrillingDays !== undefined
            && dailyEmissionsFromDrillingRig !== undefined
            && flaredGasPerProducedVolume !== undefined
            && (
                co2RemovedFromGas !== revisionAndProjectData.commonProjectAndRevisionData.co2RemovedFromGas
                || co2EmissionsFromFlaredGas !== revisionAndProjectData.commonProjectAndRevisionData.co2EmissionsFromFlaredGas
                || co2EmissionsFromFuelGas !== revisionAndProjectData.commonProjectAndRevisionData.co2EmissionFromFuelGas
                || co2Vented !== revisionAndProjectData.commonProjectAndRevisionData.co2Vented
                || averageDevelopmentWellDrillingDays !== revisionAndProjectData.commonProjectAndRevisionData.averageDevelopmentDrillingDays
                || dailyEmissionsFromDrillingRig !== revisionAndProjectData.commonProjectAndRevisionData.dailyEmissionFromDrillingRig
                || flaredGasPerProducedVolume !== revisionAndProjectData.commonProjectAndRevisionData.flaredGasPerProducedVolume
            )
        ) {
            const newProject: Components.Schemas.UpdateProjectDto = { ...revisionAndProjectData.commonProjectAndRevisionData }
            newProject.co2RemovedFromGas = co2RemovedFromGas
            newProject.co2EmissionFromFuelGas = co2EmissionsFromFuelGas
            newProject.flaredGasPerProducedVolume = flaredGasPerProducedVolume
            newProject.co2EmissionsFromFlaredGas = co2EmissionsFromFlaredGas
            newProject.co2Vented = co2Vented
            newProject.averageDevelopmentDrillingDays = averageDevelopmentWellDrillingDays
            newProject.dailyEmissionFromDrillingRig = dailyEmissionsFromDrillingRig
            addProjectEdit(revisionAndProjectData.projectId, newProject)
        }
    }, [
        co2RemovedFromGas,
        co2EmissionsFromFlaredGas,
        flaredGasPerProducedVolume,
        co2EmissionsFromFuelGas,
        co2Vented,
        averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig,
        isRevision,
    ])

    if (!revisionAndProjectData) { return null }

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
