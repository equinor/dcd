import React, { useState } from "react"
import CasesAgGridTable from "./CasesAgGridTable"
import CasesDropMenu from "./CasesDropMenu"
import EditCaseModal from "../EditCaseModal"

const CasesTable = () => {
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
    const [selectedCaseId, setSelectedCaseId] = useState<string | undefined>(undefined)
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)

    return (
        <div>
            <EditCaseModal
                caseId={selectedCaseId}
                isOpen={editCaseModalIsOpen}
                setIsOpen={setEditCaseModalIsOpen}
                editMode
                shouldNavigate={false}
            />
            <CasesAgGridTable
                setSelectedCaseId={setSelectedCaseId}
                setMenuAnchorEl={setMenuAnchorEl}
                setIsMenuOpen={setIsMenuOpen}
                isMenuOpen={isMenuOpen}
            />
            <CasesDropMenu
                isMenuOpen={isMenuOpen}
                setIsMenuOpen={setIsMenuOpen}
                menuAnchorEl={menuAnchorEl}
                selectedCaseId={selectedCaseId}
                setEditCaseModalIsOpen={setEditCaseModalIsOpen}
            />
        </div>
    )
}
export default CasesTable
