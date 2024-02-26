import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
} from "react"

interface AppContextType {
    project: Components.Schemas.ProjectDto | undefined;
    setProject: React.Dispatch<React.SetStateAction<Components.Schemas.ProjectDto | undefined>>;
    createCaseModalIsOpen: boolean;
    setCreateCaseModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
    modalEditMode: boolean;
    setModalEditMode: React.Dispatch<React.SetStateAction<boolean>>;
    modalShouldNavigate: boolean;
    setModalShouldNavigate: React.Dispatch<React.SetStateAction<boolean>>;
    modalCaseId: string | undefined;
    setModalCaseId: React.Dispatch<React.SetStateAction<string | undefined>>;
    addNewCase: () => void;
    editCase: (caseId: string) => void;
    editMode: boolean;
    setEditMode: React.Dispatch<React.SetStateAction<boolean>>;
}

const AppContext = createContext<AppContextType | undefined>(undefined)

const AppContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto | undefined>()
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [editModalIsOpen, setEditModalIsOpen] = useState<boolean>(false)
    const [modalEditMode, setModalEditMode] = useState<boolean>(false)
    const [modalShouldNavigate, setModalShouldNavigate] = useState<boolean>(false)
    const [modalCaseId, setModalCaseId] = useState<string | undefined>(undefined)
    const [editMode, setEditMode] = useState<boolean>(false)

    const editCase = (caseId: string) => {
        setModalShouldNavigate(true)
        setModalEditMode(true)
        setModalCaseId(caseId)
        setCreateCaseModalIsOpen(true)
    }

    const addNewCase = () => {
        setModalCaseId(undefined)
        setModalShouldNavigate(false)
        setModalEditMode(false)
        setCreateCaseModalIsOpen(true)
    }

    const value = useMemo(() => ({
        project,
        setProject,
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
        editCase,
        addNewCase,
    }), [
        project,
        setProject,
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
        editCase,
        addNewCase,
    ])

    return (
        <AppContext.Provider value={value}>
            {children}
        </AppContext.Provider>
    )
}

export const useAppContext = (): AppContextType => {
    const context = useContext(AppContext)
    if (context === undefined) {
        throw new Error("useAppContext must be used within an AppContextProvider")
    }
    return context
}

export { AppContextProvider }
