import {
    Button,
    Icon,
    Tooltip,
} from "@equinor/eds-core-react"
import {
    useState,
    useEffect,
    useMemo,
    useRef,
} from "react"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgGridReact } from "@ag-grid-community/react"
import { bookmark_filled, more_vertical } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"
import { casePath, productionStrategyOverviewToString } from "../../../Utils/common"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"

const StyledIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-left: 0.5rem;
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
    const { project } = useProjectContext()
    const { setProjectCase } = useCaseContext()
    const [rowData, setRowData] = useState<TableCase[]>()
    const { currentContext } = useModuleCurrentContext()
    const navigate = useNavigate()

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

    const selectCase = (p: any) => {
        if (!currentContext || !p.node.data) { return null }
        const caseResult = project.cases.find((o) => o.id === p.node.data.id)
        setProjectCase(caseResult)
        navigate(casePath(currentContext.id, p.node.data.id))
        return null
    }

    const nameWithReferenceCase = (p: any) => (
        <>
            <Button as="span" variant="ghost" className="GhostButton" onClick={() => selectCase(p)}>{p.value}</Button>
            {p.node.data.referenceCaseId === p.node.data.id && (
                <Tooltip title="Reference case">
                    <StyledIcon data={bookmark_filled} size={16} />
                </Tooltip>
            )}
        </>
    )

    const [columnDefs] = useState<ColDef[]>([
        { field: "name", cellRenderer: nameWithReferenceCase, flex: 1 },
        {
            field: "productionStrategyOverview",
            headerName: "Production Strategy Overview",
            cellRenderer: productionStrategyToString,
        },
        { field: "producerCount", headerName: "Producers" },
        { field: "gasInjectorCount", headerName: "Gas injectors" },
        { field: "waterInjectorCount", headerName: "Water injectors" },
        { field: "createdAt", headerName: "Created" },
        { field: "Options", cellRenderer: menuButton, width: 100 },
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
            <AgTableContainer>
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
