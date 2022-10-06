import {
    Button, Checkbox, NativeSelect, Table,
} from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import "ag-grid-enterprise"
import { Project } from "../../models/Project"
import PROSPTableRow from "./PROSPTableRow"
import SharePointImport from "./SharePointImport"
import { GetProspService } from "../../Services/ProspService"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import { ImportStatusEnum } from "./ImportStatusEnum"

interface Props {
    project: Project
    driveItems: DriveItem[] | undefined
}

interface RowData {
    id: string,
    name: string,
    surfState: ImportStatusEnum
    substructureState: ImportStatusEnum
    topsideState: ImportStatusEnum
    transportState: ImportStatusEnum
    sharePointFileName?: string
    sharePointFileId?: string
    sharePointSiteUrl?: string
}

function PROSPCaseListNew({
    project, driveItems,
}: Props) {
    const gridRef = useRef(null)
    const [prospCases, setProspCases] = useState<SharePointImport[]>()
    const [rowData, setRowData] = useState<RowData[]>()

    const changeStatus = (p: any, value: ImportStatusEnum) => {
        p.setValue(value)
    }

    const checkBoxStatus = (
        p: any,
    ) => {
        if (p.value === ImportStatusEnum.PROSP) {
            return <Checkbox disabled checked />
        }
        if (p.value === ImportStatusEnum.Selected) {
            return <Checkbox checked onChange={() => changeStatus(p, ImportStatusEnum.NotSelected)} />
        }
        if (p.value === ImportStatusEnum.NotSelected) {
            return <Checkbox onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
        }
        return <Checkbox onChange={() => changeStatus(p, ImportStatusEnum.Selected)} />
    }

    const casesToRowData = () => {
        if (project.cases) {
            const tableCases: RowData[] = []
            project.cases.forEach((c) => {
                const tableCase: RowData = {
                    id: c.id!,
                    name: c.name ?? "",
                    surfState: SharePointImport.surfStatus(c, project),
                    substructureState: SharePointImport.substructureStatus(c, project),
                    topsideState: SharePointImport.topsideStatus(c, project),
                    transportState: SharePointImport.transportStatus(c, project),
                    sharePointFileId: c.sharepointFileId,
                }
                tableCases.push(tableCase)
            })
            setRowData(tableCases)
        }
    }

    // Create SharePointImport[] from cases
    useEffect(() => {
        const newProspCases: SharePointImport[] = []
        console.log("DriveItems in useEffect: ", driveItems)
        if (project.cases) {
            project.cases.forEach((c) => {
                const newProspCase = new SharePointImport(c, project, undefined)
                newProspCases.push(newProspCase)
            })
        }
        setProspCases(newProspCases)
        casesToRowData()
    }, [project, driveItems])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
    }), [])

    const a = 10

    const sharePointFileDropdownOptions = () => {
        if (!driveItems) {
            return null
        }
        const options: JSX.Element[] = []

        driveItems.forEach((item) => {
            options.push((<option key={item.id} value={item.id!}>{item.name}</option>))
        })
        return options
    }

    const fileIdDropDown = (p: any) => {
        const a = 10
        return (
            <NativeSelect
                id="sharePointFile"
                label=""
                value={p.value}
                onChange={() => console.log("yee")}
            >
                {sharePointFileDropdownOptions()}
                <option aria-label="empty value" key="" value="" />
            </NativeSelect>
        )
    }

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "desc"

    const [columnDefs] = useState([
        { field: "name", sort: order },
        { field: "surfState", cellRenderer: checkBoxStatus },
        { field: "substructureState", cellRenderer: checkBoxStatus },
        { field: "topsideState", cellRenderer: checkBoxStatus },
        { field: "transportState", cellRenderer: checkBoxStatus },
        { field: "sharePointFileId", cellRenderer: fileIdDropDown },

    ])

    return (

        <div
            style={{
                display: "flex", flexDirection: "column", width: "100%",
            }}
            className="ag-theme-alpine"
        >
            {driveItems ? (
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                />
            ) : (
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                />
            )}
        </div>

    )
}

export default PROSPCaseListNew
