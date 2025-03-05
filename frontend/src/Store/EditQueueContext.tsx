import React, {
    createContext,
    useContext,
    useState,
    useCallback,
    useMemo,
} from "react"
import { EditInstance } from "@/Models/Interfaces"

interface EditQueueContextProps {
    editQueue: EditInstance[],
    lastEditTime: number,
    addToQueue: (edit: EditInstance) => void
    clearQueue: () => void
    updateLastEditTime: () => void
}

const EditQueueContext = createContext<EditQueueContextProps | undefined>(undefined)

export const EditQueueProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [lastEditTime, setLastEditTime] = useState<number>(new Date().getTime())

    const addToQueue = useCallback((edit: EditInstance) => {
        setEditQueue((prev) => [...prev, edit])
        setLastEditTime(new Date().getTime())
    }, [])

    const clearQueue = useCallback(() => {
        setEditQueue([])
    }, [])

    const updateLastEditTime = useCallback(() => {
        setLastEditTime(new Date().getTime())
    }, [])

    const value = useMemo(() => ({
        editQueue,
        lastEditTime,
        addToQueue,
        clearQueue,
        updateLastEditTime,
    }), [editQueue, lastEditTime, addToQueue, clearQueue])

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
