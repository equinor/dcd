import React from "react"
import {
    Button,
    Icon,
    Tooltip,
} from "@equinor/eds-core-react"
import { history } from "@equinor/eds-icons"
import { useAppContext } from "../../Context/AppContext"

interface props {
    size?: 16 | 24 | 32 | 48 | 18 | 40
}

const EditsSideBar: React.FC<props> = ({ size }) => {
    const { setEditHistoryIsActive, setSidebarOpen } = useAppContext()

    const openHistory = () => {
        setEditHistoryIsActive(true)
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

export default EditsSideBar
