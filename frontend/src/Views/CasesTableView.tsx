/* eslint-disable no-nested-ternary */
/* eslint-disable no-unused-expressions */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable react/no-unstable-nested-components */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-use-before-define */
/* Implementation inspired from https://blog.logrocket.com/creating-react-sortable-table/ */
import { useState } from "react"
import { Project } from "../models/Project"
import "../casesTableViewStyles.css"

interface Props {
    project: Project
}

const columns = [
    { label: "Name", accessor: "name", sortable: true },
    { label: "Description", accessor: "description", sortable: true },
    { label: "Production Strategy Overview", accessor: "productionStrategyOverview", sortable: true },
    { label: "Producers", accessor: "producers", sortable: true },
    { label: "Gas Injectors", accessor: "gasInjectors", sortable: true },
    { label: "Water Injectors", accessor: "waterInjectors", sortable: true },
    { label: "Created", accessor: "created", sortable: true },
]

const createDataTable = (project: Project) => {
    const dataArray: any[] = []
    project.cases.forEach((casee) => {
        const object: any = {
            id: casee.id,
            name: casee.name,
            description: casee.description,
            productionStrategyOverview: casee.productionStrategyOverview,
            producers: casee.producerCount,
            gasInjectors: casee.gasInjectorCount,
            waterInjectors: casee.waterInjectorCount,
            created: casee.createdAt?.toLocaleDateString(),
        }
        dataArray.push(object)
    })
    return dataArray
}

const CasesTableView = ({
    project,
}: Props) => {
    const [tableData, setTableData] = useState(createDataTable(project))
    const [sortField, setSortField] = useState("")
    const [order, setOrder] = useState("asc")

    const TableHead = ({ columns, handleSorting }: any) => {
        const handleSortingChange = (accessor: any) => {
            const sortOrder: string = accessor === sortField && order === "asc" ? "desc" : "asc"
            setSortField(accessor)
            setOrder(sortOrder)
            handleSorting(accessor, sortOrder)
        }

        return (
            <thead>
                <tr>
                    {columns.map(({ label, accessor, sortable }: any) => {
                        const cl = sortable
                            ? sortField === accessor && order === "asc"
                                ? "up"
                                : sortField === accessor && order === "desc"
                                    ? "down"
                                    : "default"
                            : ""
                        return (
                            <th
                                key={accessor}
                                onClick={() => handleSortingChange(accessor)}
                                className={cl}
                            >
                                {label}
                            </th>
                        )
                    })}
                </tr>
            </thead>
        )
    }
    const TableBody = ({ tableData, columns }: any) => (
        <tbody>
            {tableData.map((data: any) => (
                <tr key={data.id}>
                    {columns.map(({ accessor }: any) => {
                        const tData: string = data[accessor] ? data[accessor] : "0"
                        return <td key={accessor}>{tData}</td>
                    })}
                </tr>
            ))}
        </tbody>
    )

    const handleSorting = (sortField: any, sortOrder: any) => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1
                if (b[sortField] === null) return -1
                if (a[sortField] === null && b[sortField] === null) return 0
                return (
                    a[sortField].toString().localeCompare(b[sortField].toString(), "en", {
                        numeric: true,
                    }) * (sortOrder === "asc" ? 1 : -1)
                )
            })
            setTableData(sorted)
        }
    }
    return (
        <table className="table">
            <TableHead columns={columns} handleSorting={handleSorting} />
            <TableBody columns={columns} tableData={tableData} />
        </table>
    )
}

export default CasesTableView
