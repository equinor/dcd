import React, { useState } from "react"
import RevisionsDropMenu from "./Dropdown/RevisionsDropMenu"
import RevisionDetailsModal from "./RevisionDetailsModal"

interface RevisionsControlProps {
    isMenuOpen: boolean
    setIsMenuOpen: (isOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    isCaseMenu: boolean
}

const RevisionsControl: React.FC<RevisionsControlProps> = (props) => {
    const [isRevisionDetailsModalOpen, setIsRevisionDetailsModalOpen] = useState(false)

    return (
        <>
            <RevisionsDropMenu
                {...props}
                onOpenRevisionDetails={() => setIsRevisionDetailsModalOpen(true)}
            />
            <RevisionDetailsModal
                isMenuOpen={isRevisionDetailsModalOpen}
                setIsMenuOpen={setIsRevisionDetailsModalOpen}
            />
        </>
    )
}

export default RevisionsControl
