import {
    Dispatch,
    SetStateAction,
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
    margin-left: 39rem;
    flex-direction: column;
    margin-bottom: 0.5rem;
`

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function CO2ListTechnicalInput({
    project, setProject,
}: Props) {
    const gridRef = useRef<any>(null)

    const [cO2RemovedFromGas, setCO2RemovedFromGas] = useState<number>()
    const [cO2EmissionsFromFuelGas, setCO2EmissionsFromFuelGas] = useState<number>()
    const [flaredGasPerProducedVolume, setFlaredGasPerProducedVolume] = useState<number>()
    const [cO2EmissionsFromFlaredGas, setCO2EmissionsFromFlaredGas] = useState<number>()
    const [cO2Vented, setCO2Vented] = useState<number>()
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
            profile: "CO2 removed from the gas",
            unit: "% of design gas rate",
            set: setCO2RemovedFromGas,
            value: cO2RemovedFromGas,
        },
        {
            profile: "CO2-emissions from fuel gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFuelGas,
            value: cO2EmissionsFromFuelGas,
        },
        {
            profile: "Flared gas per produced volume",
            unit: "Sm³/boe",
            set: setFlaredGasPerProducedVolume,
            value: flaredGasPerProducedVolume,
        },
        {
            profile: "CO2-emissions from flared gas",
            unit: "kg CO2/Sm³",
            set: setCO2EmissionsFromFlaredGas,
            value: cO2EmissionsFromFlaredGas,
        },
        {
            profile: "CO2 vented",
            unit: "kg CO2/Sm³",
            set: setCO2Vented,
            value: cO2Vented,
        },
        {
            profile: "Average development well drilling days",
            unit: "days/wells",
            set: setAverageDevelopmentWellDrillingDays,
            value: averageDevelopmentWellDrillingDays,
        },
        {
            profile: "Daily emissions from drilling rig",
            unit: "tonnes CO2/day",
            set: setDailyEmissionsFromDrillingRig,
            value: dailyEmissionsFromDrillingRig,
        },
    ]

    const [rowData, setRowData] = useState([{}])

    const handleCellValueChange = (p: any) => {
        p.data.set(Number(p.data.value))
    }

    useEffect(() => {
        (async () => {
            try {
                setCO2RemovedFromGas(project.cO2RemovedFromGas)
                setCO2EmissionsFromFuelGas(project.cO2EmissionFromFuelGas)
                setFlaredGasPerProducedVolume(project.flaredGasPerProducedVolume)
                setCO2EmissionsFromFlaredGas(project.cO2EmissionsFromFlaredGas)
                setCO2Vented(project.cO2Vented)
                setAverageDevelopmentWellDrillingDays(project.averageDevelopmentDrillingDays)
                setDailyEmissionsFromDrillingRig(project.dailyEmissionFromDrillingRig)
            } catch (error) {
                console.error("[CO2Tab] error while submitting form data", error)
            }
        })()
    }, [])

    useEffect(() => {
        if (project) {
            const newProject: Project = { ...project }
            newProject.cO2RemovedFromGas = cO2RemovedFromGas
            newProject.cO2EmissionFromFuelGas = cO2EmissionsFromFuelGas
            newProject.flaredGasPerProducedVolume = flaredGasPerProducedVolume
            newProject.cO2EmissionsFromFlaredGas = cO2EmissionsFromFlaredGas
            newProject.cO2Vented = cO2Vented
            newProject.averageDevelopmentDrillingDays = averageDevelopmentWellDrillingDays
            newProject.dailyEmissionFromDrillingRig = dailyEmissionsFromDrillingRig
            setProject(newProject)
        }
        setRowData(co2Data)
    }, [cO2RemovedFromGas, cO2EmissionsFromFuelGas, flaredGasPerProducedVolume,
        cO2EmissionsFromFuelGas, cO2Vented, averageDevelopmentWellDrillingDays,
        dailyEmissionsFromDrillingRig])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        onCellValueChanged: handleCellValueChange,
    }), [])

    const [columnDefs] = useState([
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
                    display: "flex", flexDirection: "column", width: "54rem",
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
