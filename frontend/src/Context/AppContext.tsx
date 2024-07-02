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
    sidebarOpen: boolean,
    setSidebarOpen: Dispatch<SetStateAction<boolean>>,
    snackBarMessage: string | undefined;
    setSnackBarMessage: Dispatch<SetStateAction<string | undefined>>;
}

const AppContext = createContext<AppContextType | undefined>(undefined)

const AppContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [isCreating, setIsCreating] = useState<boolean>(false)
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [editMode, setEditMode] = useState<boolean>(false)
    const [sidebarOpen, setSidebarOpen] = useState<boolean>(true)
    const [snackBarMessage, setSnackBarMessage] = useState<string | undefined>(undefined)

    const value = useMemo(() => ({
        isCreating,
        setIsCreating,
        isLoading,
        setIsLoading,
        isSaving,
        setIsSaving,
        editMode,
        setEditMode,
        sidebarOpen,
        setSidebarOpen,
        snackBarMessage,
        setSnackBarMessage,
    }), [
        isCreating,
        setIsCreating,
        isLoading,
        setIsLoading,
        isSaving,
        setIsSaving,
        editMode,
        setEditMode,
        sidebarOpen,
        setSidebarOpen,
        snackBarMessage,
        setSnackBarMessage,
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
