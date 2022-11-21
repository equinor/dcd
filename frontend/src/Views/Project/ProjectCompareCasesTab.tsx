import styled from "styled-components"
import React, { useMemo, useRef, useState } from "react"
import {
    Typography,
} from "@equinor/eds-core-react"
import LinearDataTable from "../../Components/LinearDataTable"
import { AgGridReact } from "ag-grid-react"
import { customUnitHeaderTemplate } from "../../AgGridUnitInHeader"
import { Project } from "../../models/Project"

interface Props {
    project: Project
    capexYearX: number[]
    capexYearY: number[][]
    caseTitles: string[]
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

    const [columnDefs] = useState([
        {
            field: "cases", width: 400,
        },
        {
            headerName: "Economic KPIs (pre-tax)",
            children: [
                {
                    width: 175,
                    editable: false,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("NPV", "mill USD per 2020"),
                    },
                },
                {
                    width: 175,
                    headerComponentParams: {
                        template:
                        customUnitHeaderTemplate("Break even", "USD/bbl"),
                    },
                },
                {
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
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Oil production", "MSm3"),
                    },
                },
                {
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Gas production", "GSm3"),
                    },
                },
                {
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("NGL production", "mill tonnes"),
                    },
                },
                {
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
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Study costs + OPEX", "mill USD per 2020"),
                    },
                },
                {
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
                    width: 225,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Offshore + Onshore facility costs", "mill USD per 2020"),
                    },
                },
                {
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Development well costs", "mill USD per 2020"),
                    },
                },
                {
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
                    width: 175,
                    headerComponentParams: {
                        template: customUnitHeaderTemplate("Total CO2 emissions", "mill tonnes"),
                    },
                },
                {
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
                rowData={[]}
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
