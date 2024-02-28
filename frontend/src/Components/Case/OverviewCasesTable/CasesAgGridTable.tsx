import {
    Button,
    Icon,
    Tooltip,
    Typography,
} from "@equinor/eds-core-react"
import {
    useState,
    useEffect,
    useMemo,
    useRef,
} from "react"
import { Link } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgGridReact } from "@ag-grid-community/react"
import { bookmark_filled, more_vertical } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"
import { casePath, productionStrategyOverviewToString } from "../../../Utils/common"
import { useAppContext } from "../../../Context/AppContext"

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

interface CasesAgGridTableProps {
    setSelectedCaseId: (id: string) => void
    setMenuAnchorEl: (el: HTMLElement | null) => void
    setIsMenuOpen: (isMenuOpen: boolean) => void
    isMenuOpen: boolean

}

const CasesAgGridTable = ({
    setSelectedCaseId,
    setMenuAnchorEl,
    setIsMenuOpen,
    isMenuOpen,
}: CasesAgGridTableProps): JSX.Element => {
    const gridRef = useRef<AgGridReact>(null)
    const { project } = useAppContext()
    const [rowData, setRowData] = useState<TableCase[]>()
    const { currentContext } = useModuleCurrentContext()

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressMenu: true,
    }), [])

    if (!project) return <p>project not found</p>

    const productionStrategyToString = (p: any) => {
        const stringValue = productionStrategyOverviewToString(p.value)
        return <div>{stringValue}</div>
    }

    const onMoreClick = (data: TableCase, target: HTMLElement) => {
        console.log("onMoreClick", data)
        setSelectedCaseId(data.id)
        setMenuAnchorEl(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const menuButton = (p: any) => (
        <Button
            variant="ghost"
            onClick={(e) => onMoreClick(p.node.data, e.currentTarget)}

        >
            <Icon data={more_vertical} />
        </Button>
    )

    const nameWithReferenceCase = (p: any) => {
        if (!currentContext || !p.node.data) { return null }
        const caseDetailPath = casePath(currentContext.id, p.node.data.id)

        return (
            <span>
                {p.node.data.referenceCaseId === p.node.data.id && (
                    <Tooltip title="Reference case">
                        <MenuIcon data={bookmark_filled} size={16} />
                    </Tooltip>
                )}
                <Typography as={Link} to={caseDetailPath} link>{p.value}</Typography>
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

    return (
        <div>
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
        </div>
    )
}

export default CasesAgGridTable
