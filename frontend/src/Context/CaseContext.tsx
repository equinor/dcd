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
import { useAppContext } from "./AppContext"

interface CaseContextType {
    projectCase: Components.Schemas.CaseDto | undefined;
    setProjectCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    renameProjectCase: boolean,
    setRenameProjectCase: Dispatch<SetStateAction<boolean>>,
    projectCaseEdited: Components.Schemas.CaseDto | undefined;
    setProjectCaseEdited: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    saveProjectCase: boolean,
    setSaveProjectCase: Dispatch<SetStateAction<boolean>>,
    projectCaseNew: Components.Schemas.CreateCaseDto | undefined;
    setProjectCaseNew: Dispatch<SetStateAction<Components.Schemas.CreateCaseDto | undefined>>,
    activeTabCase: number;
    setActiveTabCase: Dispatch<SetStateAction<number>>,
}

const CaseContext = createContext<CaseContextType | undefined>(undefined)

const CaseContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const { editMode, setEditMode } = useAppContext()
    const [projectCase, setProjectCase] = useState<Components.Schemas.CaseDto | undefined>()
    const [renameProjectCase, setRenameProjectCase] = useState<boolean>(false)
    const [projectCaseEdited, setProjectCaseEdited] = useState<Components.Schemas.CaseDto | undefined>()
    const [saveProjectCase, setSaveProjectCase] = useState<boolean>(false)
    const [projectCaseNew, setProjectCaseNew] = useState<Components.Schemas.CreateCaseDto | undefined>()
    const [activeTabCase, setActiveTabCase] = useState<number>(0)

    const value = useMemo(() => ({
        projectCase,
        setProjectCase,
        renameProjectCase,
        setRenameProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        projectCaseNew,
        setProjectCaseNew,
        activeTabCase,
        setActiveTabCase,
    }), [
        projectCase,
        setProjectCase,
        renameProjectCase,
        setRenameProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        projectCaseNew,
        setProjectCaseNew,
        activeTabCase,
        setActiveTabCase,
    ])

    useEffect(() => {
        if (editMode && projectCase && !projectCaseEdited) {
            setProjectCaseEdited(projectCase)
            setRenameProjectCase(editMode)
        }
    }, [editMode, projectCaseEdited])

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
