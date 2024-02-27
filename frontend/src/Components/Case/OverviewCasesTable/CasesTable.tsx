import React, { useState } from "react"
import CasesAgGridTable from "./CasesAgGridTable"
import CasesDropMenu from "./CasesDropMenu"
import { useModalContext } from "../../../Context/ModalContext"

const CasesTable = () => {
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement | null>(null)
    const [selectedCaseId, setSelectedCaseId] = useState<string | undefined>(undefined)

    const { editCase } = useModalContext()

    return (
        <div>

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
                editCase={() => selectedCaseId && editCase(selectedCaseId)}
            />
        </div>
    )
}
export default CasesTable
