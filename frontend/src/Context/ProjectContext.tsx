import {
    FC,
    Dispatch,
    SetStateAction,
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    useEffect
} from "react"

interface ProjectContextType {
    project: Components.Schemas.ProjectDto | undefined;
    setProject: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>,
    projectEdited: Components.Schemas.ProjectDto | undefined;
    setProjectEdited: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>,
    saveProject: boolean;
    setSaveProject: Dispatch<SetStateAction<boolean>>,
    activeTabProject: number;
    setActiveTabProject: Dispatch<SetStateAction<number>>,
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined)

const ProjectContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto | undefined>()
    const [projectEdited, setProjectEdited] = useState<Components.Schemas.ProjectDto | undefined>()
    const [saveProject, setSaveProject] = useState<boolean>(false)
    const [activeTabProject, setActiveTabProject] = useState<number>(0)

    const value = useMemo(() => ({
        project,
        setProject,
        projectEdited,
        setProjectEdited,
        saveProject,
        setSaveProject,
        activeTabProject,
        setActiveTabProject,
    }), [
        project,
        setProject,
        projectEdited,
        setProjectEdited,
        saveProject,
        setSaveProject,
        activeTabProject,
        setActiveTabProject,
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
