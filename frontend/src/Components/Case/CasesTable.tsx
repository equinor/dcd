/* eslint-disable max-len */
import {
    Button,
    TextField,
    Input,
    Label,
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    useState,
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    MouseEventHandler,
    useEffect,
    useMemo,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { ModalNoFocus } from "../ModalNoFocus"
import { ToMonthDate } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"
import "ag-grid-enterprise"

const CreateCaseForm = styled.form`
    width: 50rem;
`

interface Props {
    cases: Case[]
}

const CasesTable = ({ cases }: Props) => {
    useAgGridStyles()

    const casesToRowData = () => {
        if (cases) {
            
        }
    }

    const [rowData, setRowData] = useState([
        { name: "test", description: "testDesc", productionStrategy: 1 },
        { name: "test2", description: "testDesc2", productionStrategy: 2 },
        { name: "test3", description: "testDesc3", productionStrategy: 3 },
    ])

    const [columnDefs, setColumnDefs] = useState([
        { field: "name" },
        { field: "description" },
        { field: "productionStrategy" },
    ])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
    }), [])

    return (
        <div
            style={{ display: "flex", flexDirection: "column", height: 500 }}
            className="ag-theme-alpine"
        >
            <AgGridReact
                rowData={rowData}
                columnDefs={columnDefs}
                defaultColDef={defaultColDef}
                animateRows
            />
        </div>
    )
}

export default CasesTable
