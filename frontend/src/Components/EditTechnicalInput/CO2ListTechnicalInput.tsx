import {
    useCallback,
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { Button } from "@equinor/eds-core-react"
import styled from "styled-components"
import { Project } from "../../models/Project"

const TransparentButton = styled(Button)`
    color: #007079;
    background-color: white;
    border: 1px solid #007079;
`
const CaseButtonsWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    margin-left: auto;
    z-index: 110;
`
const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

interface Props {
    project: Project
}

function CO2ListTechnicalInput({
    project,
}: Props) {
    const gridRef = useRef<any>(null)

    const [fuelGasConsumptionFromProsp, setFuelGasConsumptionFromProsp] = useState<number>()
    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>()
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>()
    const [cO2Vented, setCO2Vented] = useState<number>()
    const [cO2ReInjected, setCO2ReInjected] = useState<number>()
    const [averageDevelopmentWellDrillingDays, setAverageDevelopmentWellDrillingDays] = useState<number>()
    const [dailyEmissionsFromDrillingRig, setDailyEmissionsFromDrillingRig] = useState<number>()

    // eslint-disable-next-line no-var
    var cO2VentedRow = "CO2 vented"

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const externalFilterChanged = useCallback((newValue: string) => {
        cO2VentedRow = newValue
        gridRef.current.onFilterChanged()
    }, [])

    const isExternalFilterPresent = useCallback(
        (): boolean => cO2VentedRow !== "CO2 vented",
        [],
    )

    const doesExternalFilterPass = useCallback(
        (node: any): boolean => {
            if (node.data) {
                switch (cO2VentedRow) {
                case "CO2 vented":
                    return node.data.profile === "CO2 vented"
                case "":
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
            profile: "Fuel gas consumption from PROSP (Scope 1)",
            unit: "MSm³/sd",
        },
        {
            profile: "CO2 removed from the gas",
            unit: "% of design gas rate",
        },
        {
            profile: "CO2-emissions from fuel gas",
            unit: "kg CO2/Sm³",
        },
        {
            profile: "Flared gas per produced volume",
            unit: "Sm³/boe",
        },
        {
            profile: "CO2-emissions from flared gas",
            unit: "kg CO2/Sm³",
        },
        {
            profile: "CO2 vented",
            unit: "kg CO2/Sm³",
        },
        {
            profile: "CO2 re-injected",
            unit: "kg CO2/Sm³",
        },
        {
            profile: "Average development well drilling days",
            unit: "days/wells",
        },
        {
            profile: "Daily emissions from drilling rig",
            unit: "tonnes CO2/day",
        },
    ]

    const [rowData, setRowData] = useState([{}])

    useEffect(() => {
        setRowData(co2Data)
    }, [project])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
    }), [])

    const [columnDefs] = useState([
        {
            field: "profile",
            headerName: "CO2 emission",
            width: 400,
        },
        {
            field: "unit",
            headerName: "Unit",
            width: 200,
        },
        {
            field: "value",
            headerName: "Value",
            width: 500,
            flex: 1,
        },
    ])

    return (
        <>
            <ColumnWrapper>
                <CaseButtonsWrapper>
                    <Button onClick={() => externalFilterChanged("CO2 vented")}>
                        CO2 vented
                    </Button>
                    <TransparentButton onClick={() => externalFilterChanged("")}>
                        CO2 re-injected
                    </TransparentButton>
                </CaseButtonsWrapper>
            </ColumnWrapper>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
                className="ag-theme-alpine"
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

        </>
    )
}

export default CO2ListTechnicalInput
