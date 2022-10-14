/* eslint-disable camelcase */
import {
    Button,
    Menu,
    Icon,
    Typography,
} from "@equinor/eds-core-react"
import {
    useState,
    Dispatch,
    SetStateAction,
    useEffect,
    useMemo,
    useRef,
} from "react"
import { useHistory, useParams } from "react-router-dom"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import {
    delete_to_trash, edit, folder, library_add, more_vertical,
} from "@equinor/eds-icons"
import { Project } from "../../models/Project"
import { CasePath, ProductionStrategyOverviewToString } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"
import "ag-grid-enterprise"
import EditCaseModal from "./EditCaseModal"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

interface TableCase {
    id: string,
    name: string,
    description: string,
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview,
    producerCount: number,
    gasInjectorCount: number,
    waterInjectorCount: number,
    createdAt?: string
}

const CasesTable = ({ project, setProject }: Props) => {
    const gridRef = useRef(null)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [selectedCaseId, setSelectedCaseId] = useState<string>()

    const history = useHistory()
    const { fusionContextId } = useParams<Record<string, string | undefined>>()

    useAgGridStyles()

    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    const productionStrategyToString = (p: any) => {
        const stringValue = ProductionStrategyOverviewToString(p.value)
        return <div>{stringValue}</div>
    }

    const onMoreClick = (data: any, target: HTMLElement) => {
        setSelectedCaseId(data.id)
        setMenuAnchorEl(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const menuButton = (p: any) => (
        <Button
            color="primary"
            onClick={(e) => { if (e.target instanceof HTMLElement) { onMoreClick(p.data, e.target) } }}
        >
            <Icon data={more_vertical} />
        </Button>
    )

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const [columnDefs] = useState([
        { field: "name", sort: order },
        {
            field: "productionStrategyOverview",
            cellRenderer: productionStrategyToString,
            autoHeight: true,
            wrapText: true,
            width: 205,
        },
        { field: "producerCount", headerName: "Producers", width: 90 },
        { field: "gasInjectorCount", headerName: "Gas injectors", width: 110 },
        { field: "waterInjectorCount", headerName: "Water injectors", width: 120 },
        { field: "createdAt", headerName: "Created", width: 130 },
        { field: "", cellRenderer: menuButton, width: 95 },
    ])

    const [rowData, setRowData] = useState<TableCase[]>()

    const casesToRowData = () => {
        if (project.cases) {
            const tableCases: TableCase[] = []
            project.cases.forEach((c) => {
                const tableCase: TableCase = {
                    id: c.id!,
                    name: c.name ?? "",
                    description: c.description ?? "",
                    productionStrategyOverview: c.productionStrategyOverview,
                    producerCount: c.producerCount ?? 0,
                    waterInjectorCount: c.waterInjectorCount ?? 0,
                    gasInjectorCount: c.gasInjectorCount ?? 0,
                    createdAt: c.createdAt?.toISOString().substring(0, 10),
                }
                tableCases.push(tableCase)
            })
            setRowData(tableCases)
        }
    }

    useEffect(() => {
        casesToRowData()
    }, project.cases)

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
    }), [])

    const duplicateCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).duplicateCase(selectedCaseId, {})
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const deleteCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).deleteCase(selectedCaseId)
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                history.push(CasePath(fusionContextId!, selectedCaseId))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", width: "65%",
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
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
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
                    onClick={duplicateCase}
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
                    onClick={deleteCase}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
            </Menu>
            <EditCaseModal
                setProject={setProject}
                project={project}
                caseId={selectedCaseId}
                isOpen={editCaseModalIsOpen}
                toggleModal={toggleEditCaseModal}
                editMode
            />
        </div>
    )
}

export default CasesTable
