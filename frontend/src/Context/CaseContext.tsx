import {
    FC,
    Dispatch,
    SetStateAction,
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    useEffect,
} from "react"
import { EditInstance } from "../Models/Interfaces"
import { useLocalStorage } from "../Hooks/useLocalStorage"

interface CaseContextType {
    caseEdits: EditInstance[];
    setCaseEdits: Dispatch<SetStateAction<EditInstance[]>>,
    activeTabCase: number;
    setActiveTabCase: Dispatch<SetStateAction<number>>,
    editIndexes: any[]
    setEditIndexes: Dispatch<SetStateAction<any[]>>
    caseEditsBelongingToCurrentCase: EditInstance[]
    setCaseEditsBelongingToCurrentCase: Dispatch<SetStateAction<EditInstance[]>>
}

const CaseContext = createContext<CaseContextType | undefined>(undefined)

const CaseContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [storedCaseEdits, setStoredCaseEdits] = useLocalStorage<EditInstance[]>("caseEdits", [])
    const [storedEditIndexes, setStoredEditIndexes] = useLocalStorage<any[]>("editIndexes", [])
    const [activeTabCase, setActiveTabCase] = useState<number>(0)
    const [editIndexes, setEditIndexes] = useState<any[]>(storedEditIndexes)
    const [caseEditsBelongingToCurrentCase, setCaseEditsBelongingToCurrentCase] = useState<EditInstance[]>([])

    // Filter out edits older than 1 hour and update both state and storage
    const filterAndUpdateEdits = (edits: EditInstance[]) => {
        const oneHourAgo = new Date().getTime() - (60 * 60 * 1000)
        const recentEdits = edits.filter((edit) => edit.timeStamp > oneHourAgo)
        setStoredCaseEdits(recentEdits)
        return recentEdits
    }

    // Initialize caseEdits with filtered stored edits
    const [caseEdits, setCaseEdits] = useState<EditInstance[]>(() => filterAndUpdateEdits(storedCaseEdits))

    // Update stored edits whenever caseEdits changes
    useEffect(() => {
        setStoredCaseEdits(caseEdits)
    }, [caseEdits])

    // Update stored editIndexes whenever editIndexes changes
    useEffect(() => {
        setStoredEditIndexes(editIndexes)
    }, [editIndexes])

    // Clean up old edits periodically
    useEffect(() => {
        const cleanup = () => {
            setCaseEdits((currentEdits) => filterAndUpdateEdits(currentEdits))
        }

        // Run cleanup every 5 minutes
        const interval = setInterval(cleanup, 5 * 60 * 1000)

        // Run cleanup on mount
        cleanup()

        return () => clearInterval(interval)
    }, [])

    // Reset editIndexes if there are no case edits
    useEffect(() => {
        if (caseEdits.length === 0) {
            setEditIndexes([])
            setStoredEditIndexes([])
        }
    }, [caseEdits])

    const value = useMemo(() => ({
        caseEdits,
        setCaseEdits,
        activeTabCase,
        setActiveTabCase,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
        setCaseEditsBelongingToCurrentCase,
    }), [
        caseEdits,
        setCaseEdits,
        activeTabCase,
        setActiveTabCase,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
        setCaseEditsBelongingToCurrentCase,
    ])

    return (
        <CaseContext.Provider value={value}>
            {children}
        </CaseContext.Provider>
    )
}

export const useCaseContext = (): CaseContextType => {
    const context = useContext(CaseContext)
    if (context === undefined) {
        throw new Error("useCaseContext must be used within an CaseContextProvider")
    }
    return context
}

export { CaseContextProvider }
