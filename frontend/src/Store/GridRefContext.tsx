import React, {
    createContext,
    useContext,
    useState,
    useCallback,
    useMemo,
} from "react"

interface GridRefContextProps {
    gridRefs: React.RefObject<any>[]
    addGridRef: (ref: React.RefObject<any>) => void
    clearGridRefs: () => void
}

const GridRefContext = createContext<GridRefContextProps | undefined>(undefined)

export const GridRefProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [gridRefs, setGridRefs] = useState<React.RefObject<any>[]>([])

    const addGridRef = useCallback((ref: React.RefObject<any>) => {
        setGridRefs((prev) => [...prev, ref])
    }, [])

    const clearGridRefs = useCallback(() => {
        setGridRefs([])
    }, [])

    const value = useMemo(() => ({
        gridRefs,
        addGridRef,
        clearGridRefs,
    }), [gridRefs, addGridRef, clearGridRefs])

    return (
        <GridRefContext.Provider value={value}>
            {children}
        </GridRefContext.Provider>
    )
}

export const useGridRef = () => {
    const context = useContext(GridRefContext)
    if (!context) {
        throw new Error("useGridRef must be used within a GridRefProvider")
    }
    return context
}
