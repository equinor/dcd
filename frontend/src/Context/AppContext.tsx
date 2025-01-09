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
import { EditInstance } from "../Models/Interfaces"

interface AppContextType {
    showEditHistory: boolean,
    setShowEditHistory: Dispatch<SetStateAction<boolean>>,
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
    showRevisionReminder: boolean,
    setShowRevisionReminder: Dispatch<SetStateAction<boolean>>,
    snackBarMessage: string | undefined;
    setSnackBarMessage: Dispatch<SetStateAction<string | undefined>>;
    isCalculatingProductionOverrides: boolean,
    setIsCalculatingProductionOverrides: Dispatch<SetStateAction<boolean>>,
    isCalculatingTotalStudyCostOverrides: boolean,
    setIsCalculatingTotalStudyCostOverrides: Dispatch<SetStateAction<boolean>>,
    apiQueue: EditInstance[],
    setApiQueue: Dispatch<SetStateAction<EditInstance[]>>,

}

const AppContext = createContext<AppContextType | undefined>(undefined)

const AppContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [showEditHistory, setShowEditHistory] = useState<boolean>(false)

    const [isCreating, setIsCreating] = useState<boolean>(false)
    const [isLoading, setIsLoading] = useState<boolean>(false)
    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [editMode, setEditMode] = useState<boolean>(false)
    const [sidebarOpen, setSidebarOpen] = useState<boolean>(true)
    const [showRevisionReminder, setShowRevisionReminder] = useState<boolean>(false)
    const [snackBarMessage, setSnackBarMessage] = useState<string | undefined>(undefined)
    const [isCalculatingProductionOverrides, setIsCalculatingProductionOverrides] = useState<boolean>(false)
    const [isCalculatingTotalStudyCostOverrides, setIsCalculatingTotalStudyCostOverrides] = useState<boolean>(false)
    const [apiQueue, setApiQueue] = useState<EditInstance[]>([])

    const value = useMemo(() => ({
        showEditHistory,
        setShowEditHistory,
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
        showRevisionReminder,
        setShowRevisionReminder,
        snackBarMessage,
        setSnackBarMessage,
        isCalculatingProductionOverrides,
        setIsCalculatingProductionOverrides,
        isCalculatingTotalStudyCostOverrides,
        setIsCalculatingTotalStudyCostOverrides,
        apiQueue,
        setApiQueue,
    }), [
        showEditHistory,
        setShowEditHistory,
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
        showRevisionReminder,
        setShowRevisionReminder,
        snackBarMessage,
        setSnackBarMessage,
        isCalculatingProductionOverrides,
        setIsCalculatingProductionOverrides,
        isCalculatingTotalStudyCostOverrides,
        setIsCalculatingTotalStudyCostOverrides,
        apiQueue,
        setApiQueue,
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
