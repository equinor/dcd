import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
} from "react"

interface ModalContextType {
    caseModalIsOpen: boolean;
    setCaseModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
    caseModalEditMode: boolean;
    setCaseModalEditMode: React.Dispatch<React.SetStateAction<boolean>>;
    caseModalShouldNavigate: boolean;
    setCaseModalShouldNavigate: React.Dispatch<React.SetStateAction<boolean>>;
    modalCaseId: string | undefined;
    setModalCaseId: React.Dispatch<React.SetStateAction<string | undefined>>;
    addNewCase: () => void;
    editCase: (caseId: string) => void;

    technicalModalIsOpen: boolean;
    setTechnicalModalIsOpen: React.Dispatch<React.SetStateAction<boolean | undefined>>;
    wellProject: Components.Schemas.WellProjectDto | undefined;
    setWellProject: React.Dispatch<React.SetStateAction<Components.Schemas.WellProjectDto | undefined>>;
    exploration: Components.Schemas.ExplorationDto | undefined;
    setExploration: React.Dispatch<React.SetStateAction<Components.Schemas.ExplorationDto | undefined>>;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined)

const ModalContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    // Case modal
    const [caseModalIsOpen, setCaseModalIsOpen] = useState<boolean>(false)
    const [caseModalEditMode, setCaseModalEditMode] = useState<boolean>(false)
    const [caseModalShouldNavigate, setCaseModalShouldNavigate] = useState<boolean>(false)
    const [modalCaseId, setModalCaseId] = useState<string | undefined>(undefined)

    const editCase = (caseId: string) => {
        setCaseModalShouldNavigate(true)
        setCaseModalEditMode(true)
        setModalCaseId(caseId)
        setCaseModalIsOpen(true)
    }

    const addNewCase = () => {
        setModalCaseId(undefined)
        setCaseModalShouldNavigate(false)
        setCaseModalEditMode(false)
        setCaseModalIsOpen(true)
    }

    // Technical input modal
    const [technicalModalIsOpen, setTechnicalModalIsOpen] = useState<boolean>(false)
    const [technicalWellProject, setTechnicalWellProject] = useState<Components.Schemas.WellProjectDto | undefined>(undefined)
    const [technicalExploration, setTechnicalExploration] = useState<Components.Schemas.ExplorationDto | undefined>(undefined)

    const value = useMemo(() => ({
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        setCaseModalEditMode,
        caseModalShouldNavigate,
        setCaseModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editCase,
        addNewCase,

        technicalModalIsOpen,
        setTechnicalModalIsOpen,
        technicalWellProject,
        setTechnicalWellProject,
        technicalExploration,
        setTechnicalExploration,
    }), [
        caseModalIsOpen,
        setCaseModalIsOpen,
        caseModalEditMode,
        setCaseModalEditMode,
        caseModalShouldNavigate,
        setCaseModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editCase,
        addNewCase,

        technicalModalIsOpen,
        setTechnicalModalIsOpen,
        technicalWellProject,
        setTechnicalWellProject,
        technicalExploration,
        setTechnicalExploration,
    ])

    return (
        <ModalContext.Provider value={value as ModalContextType}>
            {children}
        </ModalContext.Provider>
    )
}

export const useModalContext = (): ModalContextType => {
    const context = useContext(ModalContext)
    if (context === undefined) {
        throw new Error("useModalContext must be used within an ModalContextProvider")
    }
    return context
}

export { ModalContextProvider }
