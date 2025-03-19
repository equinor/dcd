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

interface ProjectContextType {
    activeTabProject: number | boolean;
    setActiveTabProject: Dispatch<SetStateAction<number | boolean>>
    projectId: string
    setProjectId: Dispatch<SetStateAction<string>>
    isRevision: boolean
    setIsRevision: Dispatch<SetStateAction<boolean>>
    isCreateRevisionModalOpen: boolean
    setIsCreateRevisionModalOpen: Dispatch<SetStateAction<boolean>>
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined)

const ProjectContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [activeTabProject, setActiveTabProject] = useState<number | boolean>(0)
    const [isCreateRevisionModalOpen, setIsCreateRevisionModalOpen] = useState<boolean>(false)
    const [projectId, setProjectId] = useState<string>("")
    const [isRevision, setIsRevision] = useState<boolean>(false)

    const value = useMemo(() => ({
        activeTabProject,
        setActiveTabProject,
        projectId,
        setProjectId,
        isRevision,
        setIsRevision,
        isCreateRevisionModalOpen,
        setIsCreateRevisionModalOpen,
    }), [
        activeTabProject,
        setActiveTabProject,
        projectId,
        setProjectId,
        isRevision,
        setIsRevision,
        isCreateRevisionModalOpen,
        setIsCreateRevisionModalOpen,
    ])

    return (
        <ProjectContext.Provider value={value}>
            {children}
        </ProjectContext.Provider>
    )
}

export const useProjectContext = (): ProjectContextType => {
    const context = useContext(ProjectContext)

    if (context === undefined) {
        throw new Error("useProjectContext must be used within an ProjectContextProvider")
    }

    return context
}

export { ProjectContextProvider }
