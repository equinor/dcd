import React, {
    createContext,
    useContext,
    useState,
    useCallback,
    useMemo,
} from "react"
import { EditInstance } from "@/Models/Interfaces"

interface EditQueueContextProps {
    editQueue: EditInstance[]
    addToQueue: (edit: EditInstance) => void
    clearQueue: () => void
    timer: NodeJS.Timeout | null
    setTimer: (timer: NodeJS.Timeout | null) => void
    clearTimer: () => void
}

const EditQueueContext = createContext<EditQueueContextProps | undefined>(undefined)

/**
* Provider for the edit queue context for tables
*/
export const EditQueueProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [timer, setTimerState] = useState<NodeJS.Timeout | null>(null)

    const addToQueue = useCallback((edit: EditInstance) => {
        setEditQueue((prev) => [...prev, edit])
    }, [])

    const clearQueue = useCallback(() => {
        setEditQueue([])
    }, [])

    const setTimer = useCallback((newTimer: NodeJS.Timeout | null) => {
        setTimerState((prevTimer) => {
            if (prevTimer) {
                clearTimeout(prevTimer)
            }
            return newTimer
        })
    }, [])

    const clearTimer = useCallback(() => {
        setTimerState((prevTimer) => {
            if (prevTimer) {
                clearTimeout(prevTimer)
            }
            return null
        })
    }, [])

    const value = useMemo(() => ({
        editQueue,
        addToQueue,
        clearQueue,
        timer,
        setTimer,
        clearTimer,
    }), [editQueue, addToQueue, clearQueue, timer, setTimer, clearTimer])

    return (
        <EditQueueContext.Provider value={value}>
            {children}
        </EditQueueContext.Provider>
    )
}

export const useEditQueue = (): EditQueueContextProps => {
    const context = useContext(EditQueueContext)
    if (!context) {
        throw new Error("useEditQueue must be used within an EditQueueProvider")
    }
    return context
}
