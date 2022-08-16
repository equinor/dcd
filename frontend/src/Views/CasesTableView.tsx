/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable camelcase */
/* Implementation inspired from https://blog.logrocket.com/creating-react-sortable-table/ */
import { Dispatch, SetStateAction, useState } from "react"
import { Button } from "@material-ui/core"
import "../casesTableViewStyles.css"
import { Icon, Menu, Typography } from "@equinor/eds-core-react"
import {
    delete_to_trash, edit, folder, library_add, more_vertical,
} from "@equinor/eds-icons"
import { useParams } from "react-router"
import { Project } from "../models/Project"
import { GetCaseService } from "../Services/CaseService"
import { GetProjectService } from "../Services/ProjectService"

const columnsForTable = [
    { label: "Name", accessor: "name", sortable: true },
    { label: "Description", accessor: "description", sortable: true },
    { label: "Production Strategy Overview", accessor: "productionStrategyOverview", sortable: true },
    { label: "Producers", accessor: "producers", sortable: true },
    { label: "Gas Injectors", accessor: "gasInjectors", sortable: true },
    { label: "Water Injectors", accessor: "waterInjectors", sortable: true },
    { label: "Created", accessor: "created", sortable: true },
    { label: "", accessor: "kebab", sortable: false },
]

enum productionStrategyOverviewEnum {
    "Depletion" = 0,
    "Water injection" = 1,
    "Gas injection" = 2,
    "WAG" = 3,
    "Mixed" = 4
}

const createDataTable = (project: Project) => {
    const dataArray: any[] = []
    project.cases.forEach((casee) => {
        const object: any = {
            id: casee.id,
            name: casee.name,
            description: casee.description,
            productionStrategyOverview: productionStrategyOverviewEnum[casee.productionStrategyOverview!],
            producers: casee.producerCount,
            gasInjectors: casee.gasInjectorCount,
            waterInjectors: casee.waterInjectorCount,
            created: casee.createdAt?.toLocaleDateString(),
            kebab: "",
        }
        dataArray.push(object)
    })
    return dataArray
}

interface TableHeadProps {
    columns: any,
    handleSorting: any,
    sortField: string,
    setSortField: any,
    order: string,
    setOrder: any
}

const TableHead = ({
    columns,
    handleSorting,
    sortField, setSortField,
    order, setOrder,
}: TableHeadProps) => {
    const handleSortingChange = (accessor: string) => {
        const sortOrder: string = accessor === sortField && order === "asc" ? "desc" : "asc"
        setSortField(accessor)
        setOrder(sortOrder)
        handleSorting(accessor, sortOrder)
    }

    return (
        <thead>
            <tr>
                {columns.map(({ label, accessor, sortable }: any) => {
                    const cl = sortable ? sortField === accessor && order === "asc" ? "up" : sortField === accessor && order === "desc" ? "down" : "default" : ""
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

interface TableBodyProps {
    setCaseRowDataSelected: any,
    columns: any,
    tableData: any,
    setElement: any,
    isMenuOpen: boolean,
    setIsMenuOpen: any
}

const TableBody = ({
    setCaseRowDataSelected,
    columns,
    tableData,
    setElement,
    isMenuOpen, setIsMenuOpen,
}: TableBodyProps) => {
    const onMoreClick = (data: any, target: EventTarget) => {
        setCaseRowDataSelected(data)
        setElement(target)
        setIsMenuOpen(!isMenuOpen)
    }
    return (
        <tbody>
            {tableData.map((data: any) => (
                <tr key={data.id}>
                    {columns.map(({ accessor }: any) => {
                        // For kebaben
                        if (accessor === "kebab") {
                            return (
                                <td key="hehe">
                                    <Button
                                        color="primary"
                                        onClick={(e) => onMoreClick(data, e.target)}
                                    >
                                        <Icon data={more_vertical} />
                                    </Button>

                                </td>
                            )
                        }

                        const tData: string = data[accessor] ? data[accessor] : "0"
                        return <td key={accessor}>{tData}</td>
                    })}
                </tr>
            ))}

        </tbody>
    )
}

interface CasesTableViewProps {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

const CasesTableView = ({
    project, setProject,
}: CasesTableViewProps) => {
    const [tableData, setTableData] = useState(createDataTable(project))
    const [sortField, setSortField] = useState("")
    const [order, setOrder] = useState("asc")

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [element, setElement] = useState<HTMLButtonElement>()

    const [caseRowDataSelected, setCaseRowDataSelected] = useState<any>()

    const handleSorting = (sortFieldIndex: any, sortOrder: any) => {
        if (sortFieldIndex) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortFieldIndex] === null) return 1
                if (b[sortFieldIndex] === null) return -1
                if (a[sortFieldIndex] === null && b[sortFieldIndex] === null) return 0
                return (
                    a[sortFieldIndex].toString().localeCompare(b[sortFieldIndex].toString(), "en", {
                        numeric: true,
                    }) * (sortOrder === "asc" ? 1 : -1)
                )
            })
            setTableData(sorted)
        }
    }

    const duplicateCase = async () => {
        try {
            if (caseRowDataSelected != null) {
                const newProject = await (await GetCaseService()).duplicateCase(caseRowDataSelected.id, {})
                setProject(newProject)
                setTableData(createDataTable(newProject))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <div>
            <table className="table">
                <TableHead
                    columns={columnsForTable}
                    handleSorting={handleSorting}
                    sortField={sortField}
                    setSortField={setSortField}
                    order={order}
                    setOrder={setOrder}
                />
                <TableBody
                    setElement={setElement}
                    setIsMenuOpen={setIsMenuOpen}
                    setCaseRowDataSelected={setCaseRowDataSelected}
                    columns={columnsForTable}
                    tableData={tableData}
                    isMenuOpen={isMenuOpen}
                />
            </table>
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={element}
                onClose={() => setIsMenuOpen(false)}
                placement="right"
            >
                <Menu.Item
                    onClick={() => console.log(caseRowDataSelected)}
                >
                    <Icon data={folder} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Open
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={duplicateCase}
                >
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => console.log(caseRowDataSelected)}
                >
                    <Icon data={edit} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Rename
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => console.log(caseRowDataSelected)}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
            </Menu>
        </div>
    )
}

export default CasesTableView
