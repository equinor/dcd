import {
    FC,
    Dispatch,
    SetStateAction,
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
} from "react"

interface AppContextType {
    isCreating: boolean,
    setIsCreating: Dispatch<SetStateAction<boolean>>,
    isLoading: boolean,
    setIsLoading: Dispatch<SetStateAction<boolean>>,
    isSaving: boolean,
    setIsSaving: Dispatch<SetStateAction<boolean>>,
    editMode: boolean,
    setEditMode: Dispatch<SetStateAction<boolean>>,
    updateFromServer: boolean,
    setUpdateFromServer: Dispatch<SetStateAction<boolean>>,
    sidebarOpen: boolean,
    setSidebarOpen: Dispatch<SetStateAction<boolean>>,
    editHistoryIsActive: boolean;
    setEditHistoryIsActive: Dispatch<SetStateAction<boolean>>;
}

const AppContext = createContext<AppContextType | undefined>(undefined)

const AppContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [isCreating, setIsCreating] = useState<boolean>(false)
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [editMode, setEditMode] = useState<boolean>(false)
    const [updateFromServer, setUpdateFromServer] = useState<boolean>(true)
    const [sidebarOpen, setSidebarOpen] = useState<boolean>(true)
    const [editHistoryIsActive, setEditHistoryIsActive] = useState(true)

    const value = useMemo(() => ({
        isCreating,
        setIsCreating,
        isLoading,
        setIsLoading,
        isSaving,
        setIsSaving,
        editMode,
        setEditMode,
        updateFromServer,
        setUpdateFromServer,
        sidebarOpen,
        setSidebarOpen,
        editHistoryIsActive,
        setEditHistoryIsActive,
    }), [
        isCreating,
        setIsCreating,
        isLoading,
        setIsLoading,
        isSaving,
        setIsSaving,
        editMode,
        setEditMode,
        updateFromServer,
        setUpdateFromServer,
        sidebarOpen,
        setSidebarOpen,
        editHistoryIsActive,
        setEditHistoryIsActive,
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
