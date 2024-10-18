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
    accessRights: Components.Schemas.AccessRightsDto | undefined;
    setAccessRights: Dispatch<SetStateAction<Components.Schemas.AccessRightsDto | undefined>>
    activeTabProject: number;
    setActiveTabProject: Dispatch<SetStateAction<number>>
    projectId: string
    setProjectId: Dispatch<SetStateAction<string>>
    isRevision: boolean
    setIsRevision: Dispatch<SetStateAction<boolean>>
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined)

const ProjectContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [accessRights, setAccessRights] = useState<Components.Schemas.AccessRightsDto>()
    const [activeTabProject, setActiveTabProject] = useState<number>(0)
    const [projectId, setProjectId] = useState<string>("")
    const [isRevision, setIsRevision] = useState<boolean>(false)

    const value = useMemo(() => ({
        accessRights,
        setAccessRights,
        activeTabProject,
        setActiveTabProject,
        projectId,
        setProjectId,
        isRevision,
        setIsRevision,
    }), [
        accessRights,
        setAccessRights,
        activeTabProject,
        setActiveTabProject,
        projectId,
        setProjectId,
        isRevision,
        setIsRevision,
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
