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
}

const ModalContext = createContext<ModalContextType | undefined>(undefined)

const ModalContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
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
    ])

    return (
        <ModalContext.Provider value={value}>
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
