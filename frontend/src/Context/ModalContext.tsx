import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
} from "react"

interface ModalContextType {
    createCaseModalIsOpen: boolean;
    setCreateCaseModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
    modalEditMode: boolean;
    setModalEditMode: React.Dispatch<React.SetStateAction<boolean>>;
    modalShouldNavigate: boolean;
    setModalShouldNavigate: React.Dispatch<React.SetStateAction<boolean>>;
    modalCaseId: string | undefined;
    setModalCaseId: React.Dispatch<React.SetStateAction<string | undefined>>;
    addNewCase: () => void;
    editCase: (caseId: string) => void;
    editMode: boolean;
    setEditMode: React.Dispatch<React.SetStateAction<boolean>>;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined)

const ModalContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [editModalIsOpen, setEditModalIsOpen] = useState<boolean>(false)
    const [modalEditMode, setModalEditMode] = useState<boolean>(false)
    const [modalShouldNavigate, setModalShouldNavigate] = useState<boolean>(false)
    const [modalCaseId, setModalCaseId] = useState<string | undefined>(undefined)
    const [editMode, setEditMode] = useState<boolean>(false)

    const editCase = (caseId: string) => {
        setModalShouldNavigate(true)
        setModalEditMode(true)
        setModalCaseId(caseId)
        setCreateCaseModalIsOpen(true)
    }

    const addNewCase = () => {
        setModalCaseId(undefined)
        setModalShouldNavigate(false)
        setModalEditMode(false)
        setCreateCaseModalIsOpen(true)
    }

    const value = useMemo(() => ({
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
        editCase,
        addNewCase,
    }), [
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
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
