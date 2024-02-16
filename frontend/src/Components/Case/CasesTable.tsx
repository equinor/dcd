import {
    Button,
    Menu,
    Icon,
    Typography,
    Tooltip,
} from "@equinor/eds-core-react"
import {
    useState,
    useEffect,
    useMemo,
    useRef,
} from "react"
import { Link, useNavigate } from "react-router-dom"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import {
    bookmark_filled,
    bookmark_outlined,
    delete_to_trash, edit, folder, library_add, more_vertical,
} from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"
import { casePath, productionStrategyOverviewToString } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"
import EditCaseModal from "./EditCaseModal"
import { EMPTY_GUID } from "../../Utils/constants"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../context/AppContext"

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
`
const AgTableContainer = styled.div`
    overflow: auto;
`

interface TableCase {
    id: string,
    name: string,
    description: string,
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview | undefined,
    producerCount: number,
    gasInjectorCount: number,
    waterInjectorCount: number,
    createdAt?: string
    referenceCaseId?: string
}

const CasesTable = () => {
    const gridRef = useRef<AgGridReact>(null)
    const styles = useStyles()
    const { project, setProject } = useAppContext()

    if (!project) return null

    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [selectedCaseId, setSelectedCaseId] = useState<string>()

    const navigate = useNavigate()

    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    const productionStrategyToString = (p: any) => {
        const stringValue = productionStrategyOverviewToString(p.value)
        return <div>{stringValue}</div>
    }

    const onMoreClick = (data: TableCase, target: HTMLElement) => {
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

    const nameWithReferenceCase = (p: any) => {
        const caseDetailPath = casePath(project.id, p.node.data.id)

        return (
            <span>
                {p.node.data.referenceCaseId === p.node.data.id && (
                    <Tooltip title="Reference case">
                        <MenuIcon data={bookmark_filled} size={16} />
                    </Tooltip>
                )}
                <Link to={caseDetailPath} style={{ textDecoration: "none", color: "inherit" }}>
                    {p.value}
                </Link>
            </span>
        )
    }

    const [columnDefs] = useState<ColDef[]>([
        { field: "name", cellRenderer: nameWithReferenceCase },
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
        { field: "Options", cellRenderer: menuButton, width: 95 },
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
                    createdAt: c.createTime?.substring(0, 10),
                    referenceCaseId: project.referenceCaseId,
                }
                tableCases.push(tableCase)
            })
            setRowData(tableCases)
        }
    }

    useEffect(() => {
        casesToRowData()
    }, [project.cases])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressMenu: true,
    }), [])

    const duplicateCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).duplicateCase(project.id, selectedCaseId)
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const deleteCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).deleteCase(project.id, selectedCaseId)
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                navigate(selectedCaseId)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const setCaseAsReference = async () => {
        try {
            const projectDto = { ...project }
            if (projectDto.referenceCaseId === selectedCaseId) {
                projectDto.referenceCaseId = EMPTY_GUID
            } else {
                projectDto.referenceCaseId = selectedCaseId ?? ""
            }
            const newProject = await (await GetProjectService()).updateProject(projectDto)
            setProject(newProject)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <div className={styles.root}>
            <EditCaseModal
                caseId={selectedCaseId}
                isOpen={editCaseModalIsOpen}
                toggleModal={toggleEditCaseModal}
                editMode
                shouldNavigate={false}
            />
            <AgTableContainer className="ag-theme-alpine-fusion">
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                />
            </AgTableContainer>
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
                {project.referenceCaseId === selectedCaseId
                    ? (
                        <Menu.Item
                            onClick={setCaseAsReference}
                        >
                            <Icon data={bookmark_outlined} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Remove as reference case
                            </Typography>
                        </Menu.Item>
                    )
                    : (
                        <Menu.Item
                            onClick={setCaseAsReference}
                        >
                            <Icon data={bookmark_filled} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Set as reference case
                            </Typography>
                        </Menu.Item>
                    )}
            </Menu>
        </div>
    )
}

export default CasesTable
