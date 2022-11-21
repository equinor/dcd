import styled from "styled-components"
import React, {
    useEffect, useMemo, useRef, useState,
} from "react"
import {
    Typography,
} from "@equinor/eds-core-react"
import { AgGridReact } from "ag-grid-react"
import LinearDataTable from "../../Components/LinearDataTable"
import { customUnitHeaderTemplate } from "../../AgGridUnitInHeader"
import { Project } from "../../models/Project"

interface Props {
    project: Project
    capexYearX: number[]
    capexYearY: number[][]
    caseTitles: string[]
}

interface TableCompareCase {
    id: string,
    cases: string,
    description: string,
    npv: number,
    breakEven: number,
    irr: number,
    oilProduction: number,
    gasProduction: number,
    nglProduction: number,
    totalExportedVolumes: number,
    studyCostsPlusOpex: number,
    cessationCosts: number,
    offshorePlusOnshoreFacilityCosts: number,
    developmentCosts: number,
    explorationWellCosts: number,
    totalCO2Emissions: number,
    cO2Intensity: number,
}

// const Wrapper = styled.div`
//     display: flex;
//     flex-direction: column;
// `

// const ChartsContainer = styled.div`
//     display: flex;
// `

function ProjectCompareCasesTab({
    project,
    capexYearX,
    capexYearY,
    caseTitles,
}: Props) {
    // <Wrapper>
    //     <ChartsContainer>
    //         {capexYearX.length !== 0
    //             ? (
    //                 <LinearDataTable
    //                     capexYearX={capexYearX}
    //                     capexYearY={capexYearY}
    //                     caseTitles={caseTitles}
    //                 />
    //             )
    //             : <Typography> No cases containing CapEx to display data for</Typography> }
    //     </ChartsContainer>
    // </Wrapper>

    const gridRef = useRef(null)

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
    }), [])

    const [rowData, setRowData] = useState<TableCompareCase[]>()

    const casesToRowData = () => {
        if (project) {
            const tableCompareCases: TableCompareCase[] = []
            project.cases.forEach((c) => {
                const tableCase: TableCompareCase = {
                    id: c.id!,
                    cases: c.name ?? "",
                    description: c.description ?? "",
                    npv: c.npv ?? 0,
                    breakEven: c.breakEven ?? 0,
                    irr: 0,
                    oilProduction: 0,
                    gasProduction: 0,
                    nglProduction: 0,
                    totalExportedVolumes: 0,
                    studyCostsPlusOpex: 0,
                    cessationCosts: 0,
                    offshorePlusOnshoreFacilityCosts: 0,
                    developmentCosts: 0,
                    explorationWellCosts: 0,
                    totalCO2Emissions: 0,
                    cO2Intensity: 0,
                }
                tableCompareCases.push(tableCase)
            })
            setRowData(tableCompareCases)
        }
    }

    useEffect(() => {
        casesToRowData()
    }, [project.cases])

    const [columnDefs] = useState([
        {
            field: "cases", width: 400,
        },
        {
            headerName: "Economic KPIs (pre-tax)",
            children: [
                {
                    field: "npv",
                    headerName: "",
                    width: 175,
                    editable: false,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("NPV", "mill USD per 2020"),
                    },
                },
                {
                    field: "breakEven",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template:
                        customUnitHeaderTemplate("Break even", "USD/bbl"),
                    },
                },
                {
                    field: "irr",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("IRR", "%"),
                    },
                },
            ],
        },
        {
            headerName: "Production profiles",
            children: [
                {
                    field: "oilProduction",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Oil production", "MSm3"),
                    },
                },
                {
                    field: "gasProduction",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Gas production", "GSm3"),
                    },
                },
                {
                    field: "nglProduction",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("NGL production", "mill tonnes"),
                    },
                },
                {
                    field: "totalExportedVolumes",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Total exported volumes", "mill boe"),
                    },
                },
            ],
        },
        {
            headerName: "Cost profiles",
            children: [
                {
                    field: "studyCostsPlusOpex",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Study costs + OPEX", "mill USD per 2020"),
                    },
                },
                {
                    field: "cessationCosts",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Cessation costs", "USD/bbl"),
                    },
                },
            ],
        },
        {
            headerName: "Investment profiles",
            children: [
                {
                    field: "offshorePlusOnshoreFacilityCosts",
                    headerName: "",
                    width: 225,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Offshore + Onshore facility costs", "mill USD per 2020"),
                    },
                },
                {
                    field: "developmentCosts",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Development well costs", "mill USD per 2020"),
                    },
                },
                {
                    field: "explorationWellCosts",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Exploration well costs", "mill USD per 2020"),
                    },
                },
            ],
        },
        {
            headerName: "CO2 emissions",
            children: [
                {
                    field: "totalCO2Emissions",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Total CO2 emissions", "mill tonnes"),
                    },
                },
                {
                    field: "cO2Intensity",
                    headerName: "",
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("CO2 intensity", "kg CO2/boe"),
                    },
                },
            ],
        },
    ])

    return (
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
            />
        </div>
    )
}

export default ProjectCompareCasesTab
