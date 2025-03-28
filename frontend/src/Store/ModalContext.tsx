import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    useEffect,
} from "react"

import { useAppStore } from "./AppStore"

interface ModalContextType {
    caseModalIsOpen: boolean;
    setCaseModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>,
    caseModalEditMode: boolean;
    setCaseModalEditMode: React.Dispatch<React.SetStateAction<boolean>>,
    caseModalShouldNavigate: boolean;
    setCaseModalShouldNavigate: React.Dispatch<React.SetStateAction<boolean>>,
    modalCaseId: string | undefined;
    setModalCaseId: React.Dispatch<React.SetStateAction<string | undefined>>,
    addNewCase: () => void;
    editCase: (caseId: string) => void;

    technicalModalIsOpen: boolean;
    setTechnicalModalIsOpen: React.Dispatch<React.SetStateAction<boolean | undefined>>,
    editTechnicalInput: boolean;
    setEditTechnicalInput: React.Dispatch<React.SetStateAction<boolean | undefined>>,

    featuresModalIsOpen: boolean;
    setFeaturesModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>,
}

const ModalContext = createContext<ModalContextType | undefined>(undefined)

const ModalContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const { editMode } = useAppStore()
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
    const [editTechnicalInput, setEditTechnicalInput] = useState<boolean | undefined>(undefined)
    const [featuresModalIsOpen, setFeaturesModalIsOpen] = useState<boolean>(false)

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
        editTechnicalInput,
        setEditTechnicalInput,

        featuresModalIsOpen,
        setFeaturesModalIsOpen,
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
        editTechnicalInput,
        setEditTechnicalInput,

        featuresModalIsOpen,
        setFeaturesModalIsOpen,
    ])

    useEffect(() => {
        if (editTechnicalInput === undefined && editMode) { setEditTechnicalInput(true) }
    }, [editMode])

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
