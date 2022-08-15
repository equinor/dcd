/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable camelcase */
/* Implementation inspired from https://blog.logrocket.com/creating-react-sortable-table/ */
import {
    ChangeEventHandler, MouseEventHandler, useState,
} from "react"
import "../casesTableViewStyles.css"
import {
    Icon, Menu, TextField, Typography, Button,
} from "@equinor/eds-core-react"
import {
    delete_to_trash, edit, folder, library_add, more_vertical, navigation,
} from "@equinor/eds-icons"
import styled from "styled-components"
import { useHistory } from "react-router"
import { Project } from "../models/Project"
import { GetCaseService } from "../Services/CaseService"
import { CasePath } from "../Utils/common"
import { ModalNoFocus } from "../Components/ModalNoFocus"

const EditCaseForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

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
}

function CasesTableView({
    project,
}: CasesTableViewProps) {
    const [tableData, setTableData] = useState(createDataTable(project))
    const [sortField, setSortField] = useState("")
    const [order, setOrder] = useState("asc")

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [element, setElement] = useState<HTMLButtonElement>()

    const [caseRowDataSelected, setCaseRowDataSelected] = useState<any>()

    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [caseName, setCaseName] = useState<string>("")
    const [caseDescription, setCaseDescription] = useState<string>("")
    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    const history = useHistory()

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

    const handleCaseNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setCaseName(value)
    }

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        const { value } = e.target
        setCaseDescription(value)
    }

    const submitEditCaseForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            const projectResult: Project = await (await GetCaseService()).updateCase({
                description: caseDescription,
                name: caseName,
                projectId: project.projectId,
                id: caseRowDataSelected.id,
            })
            toggleEditCaseModal()
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const openCase = async () => {
        try {
            if (caseRowDataSelected != null) {
                history.push(CasePath(project.id, caseRowDataSelected.id))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <>
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
                        onClick={openCase}
                    >
                        <Icon data={folder} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Open
                        </Typography>
                    </Menu.Item>
                    <Menu.Item
                        onClick={() => console.log(caseRowDataSelected)}
                    >
                        <Icon data={library_add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Duplicate
                        </Typography>
                    </Menu.Item>
                    <Menu.Item
                        onClick={toggleEditCaseModal}
                    >
                        <Icon data={edit} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Edit
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
            <ModalNoFocus isOpen={editCaseModalIsOpen} title={`Edit case name and description for ${caseRowDataSelected?.name}`}>
                <EditCaseForm>
                    <TextField
                        label="Name"
                        id="name"
                        name="name"
                        placeholder={caseRowDataSelected?.name ?? "Enter a name"}
                        onChange={handleCaseNameChange}
                    />

                    <TextField
                        label="Description"
                        id="description"
                        name="description"
                        placeholder={caseRowDataSelected?.description ?? "Enter a description"}
                        onChange={handleDescriptionChange}
                    />
                    <div>
                        <Button
                            type="submit"
                            onClick={submitEditCaseForm}
                            disabled={caseName === "" || caseDescription === ""}
                        >
                            Update case
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleEditCaseModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </EditCaseForm>
            </ModalNoFocus>
        </>
    )
}

export default CasesTableView
