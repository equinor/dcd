import React, {
    createContext, useContext, useState, useCallback, ReactNode, useMemo,
} from "react"

import { EditInstance } from "@/Models/Interfaces"

interface EditQueueContextType {
    editQueue: EditInstance[]
    addToQueue: (edit: EditInstance) => void
    clearQueue: () => void
    lastEditTime: number
}

const EditQueueContext = createContext<EditQueueContextType | undefined>(undefined)

interface EditQueueProviderProps {
    children: ReactNode
}

export const EditQueueProvider: React.FC<EditQueueProviderProps> = ({ children }) => {
    const [editQueue, setEditQueue] = useState<EditInstance[]>([])
    const [lastEditTime, setLastEditTime] = useState<number>(Date.now())

    const addToQueue = useCallback((edit: EditInstance) => {
        setLastEditTime(Date.now())
        setEditQueue((prev) => [...prev, edit])
    }, [])

    const clearQueue = useCallback(() => {
        setEditQueue([])
    }, [])

    const value = useMemo(() => ({
        editQueue,
        addToQueue,
        clearQueue,
        lastEditTime,
    }), [editQueue, addToQueue, clearQueue, lastEditTime])

    return (
        <EditQueueContext.Provider value={value}>
            {children}
        </EditQueueContext.Provider>
    )
}

export const useEditQueue = (): EditQueueContextType => {
    const context = useContext(EditQueueContext)

    if (context === undefined) {
        throw new Error("useEditQueue must be used within an EditQueueProvider")
    }

    return context
}
