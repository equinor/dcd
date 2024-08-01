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

const getFilteredEdits = () => {
    const savedEdits = JSON.parse(localStorage.getItem("caseEdits") || "[]")
    const oneHourAgo = new Date().getTime() - (60 * 60 * 1000)

    // localStorage cleanup to remove edits older than 1 hour
    const recentEdits = savedEdits.filter((edit: EditInstance) => edit.timeStamp > oneHourAgo)
    localStorage.setItem("caseEdits", JSON.stringify(recentEdits))

    return recentEdits
}

const CaseContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [caseEdits, setCaseEdits] = useState<EditInstance[]>(getFilteredEdits())
    const [activeTabCase, setActiveTabCase] = useState<number>(0)
    const [editIndexes, setEditIndexes] = useState<any[]>([])
    const [caseEditsBelongingToCurrentCase, setCaseEditsBelongingToCurrentCase] = useState<EditInstance[]>([])

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

    useEffect(() => {
        const storedCaseEdits = localStorage.getItem("caseEdits")
        const caseEditsArray = storedCaseEdits ? JSON.parse(storedCaseEdits) : []

        if (caseEditsArray.length === 0) {
            // reset editIndexes if there are no recent edits
            localStorage.setItem("editIndexes", JSON.stringify([]))
            setEditIndexes([])
        } else {
            // otherwise, load the editIndexes from localStorage
            const storedEditIndexes = localStorage.getItem("editIndexes")
            const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
            setEditIndexes(editIndexesArray)
        }
    }, [])

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
