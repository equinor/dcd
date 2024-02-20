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
}

const AppContext = createContext<AppContextType | undefined>(undefined)

const AppContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto | undefined>()
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)

    const value = useMemo(() => ({
        project,
        setProject,
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
    }), [
        project,
        setProject,
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
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
