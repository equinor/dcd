import { useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { useDataFetch } from "@/Hooks"
import { ActiveCasesTable } from "./Components/ActiveCasesTable"
import { ArchivedCasesTable } from "./Components/ArchivedCasesTable"
import { STEAExportButton } from "./Components/STEAExportButton"
import TableCasesDropMenu from "./Components/TableCasesDropMenu"
import { useModalContext } from "@/Store/ModalContext"

const CasesTable = () => {
    const { isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const { setShowRevisionReminder } = useAppStore()
    const revisionAndProjectData = useDataFetch()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
    const [selectedCaseId, setSelectedCaseId] = useState<string>()

    const { editCase } = useModalContext()

    const handleMenuClick = (caseId: string, target: HTMLElement) => {
        setSelectedCaseId(caseId)
        setMenuAnchorEl(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const mapCasesToTableData = (isArchived: boolean) => {
        if (!revisionAndProjectData?.commonProjectAndRevisionData?.cases) { return [] }
        return revisionAndProjectData.commonProjectAndRevisionData.cases
            .filter((c) => c.archived === isArchived)
            .map((c) => ({
                id: c.caseId!,
                name: c.name ?? "",
                description: c.description ?? "",
                productionStrategyOverview: c.productionStrategyOverview,
                producerCount: c.producerCount ?? 0,
                waterInjectorCount: c.waterInjectorCount ?? 0,
                gasInjectorCount: c.gasInjectorCount ?? 0,
                createdAt: c.createTime?.substring(0, 10),
                referenceCaseId: revisionAndProjectData.commonProjectAndRevisionData.referenceCaseId,
            }))
    }

    const activeCases = useMemo(
        () => mapCasesToTableData(false),
        [revisionAndProjectData],
    )

    const archivedCases = useMemo(
        () => mapCasesToTableData(true),
        [revisionAndProjectData],
    )

    if (!revisionAndProjectData && !isRevision) { return <p>Project not found</p> }
    if (!revisionAndProjectData && isRevision) { return <p>Revision not found</p> }

    const projectOrRevisionId = isRevision
        ? (revisionAndProjectData as { revisionId: string }).revisionId
        : (revisionAndProjectData as { projectId: string }).projectId

    return (
        <div>
            <ActiveCasesTable
                cases={activeCases}
                isRevision={isRevision}
                revisionId={revisionId}
                onMenuClick={handleMenuClick}
            />

            <STEAExportButton
                projectOrRevisionId={projectOrRevisionId}
                setShowRevisionReminder={setShowRevisionReminder}
            />

            <ArchivedCasesTable
                cases={archivedCases}
                isRevision={isRevision}
                revisionId={revisionId}
                onMenuClick={handleMenuClick}
            />

            <TableCasesDropMenu
                isMenuOpen={isMenuOpen}
                setIsMenuOpen={setIsMenuOpen}
                menuAnchorEl={menuAnchorEl}
                selectedCaseId={selectedCaseId}
                editCase={() => selectedCaseId && editCase(selectedCaseId)}
            />
        </div>
    )
}

export default CasesTable
