import React from "react"
import { Button, Icon, Tooltip } from "@equinor/eds-core-react"
import { history } from "@equinor/eds-icons"
import { useParams } from "react-router-dom"
import { useAppContext } from "../../Context/AppContext"

interface props {
    size?: 16 | 24 | 32 | 48 | 18 | 40
}

const HistoryButton: React.FC<props> = ({ size }) => {
    const { setSidebarOpen } = useAppContext()
    const { caseId } = useParams()

    if (!caseId) { return null }

    const openHistory = () => {
        setSidebarOpen(true)
    }

    return (
        <Tooltip title="See edit history">
            <Button variant="ghost_icon" onClick={() => openHistory()}>
                <Icon size={size} data={history} />
            </Button>
        </Tooltip>
    )
}

export default HistoryButton
