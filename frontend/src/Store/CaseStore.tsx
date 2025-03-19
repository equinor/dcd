import { SetStateAction } from "react"
import { create } from "zustand"
import { persist } from "zustand/middleware"

import { EditInstance } from "../Models/Interfaces"

interface CaseState {
    caseEdits: EditInstance[];
    setCaseEdits: (value: SetStateAction<EditInstance[]>) => void;
    activeTabCase: number;
    setActiveTabCase: (value: SetStateAction<number>) => void;
    editIndexes: any[];
    setEditIndexes: (value: SetStateAction<any[]>) => void;
    caseEditsBelongingToCurrentCase: EditInstance[];
    setCaseEditsBelongingToCurrentCase: (value: SetStateAction<EditInstance[]>) => void;
}

export const useCaseStore = create<CaseState>()(
    persist(
        (set, get) => ({
            caseEdits: [],
            setCaseEdits: (value) => set({
                caseEdits: typeof value === "function"
                    ? value(get().caseEdits)
                    : value,
            }),

            activeTabCase: 0,
            setActiveTabCase: (value) => set({
                activeTabCase: typeof value === "function"
                    ? value(get().activeTabCase)
                    : value,
            }),
            editIndexes: [],
            setEditIndexes: (value) => set({
                editIndexes: typeof value === "function"
                    ? value(get().editIndexes)
                    : value,
            }),

            caseEditsBelongingToCurrentCase: [],
            setCaseEditsBelongingToCurrentCase: (value) => set({
                caseEditsBelongingToCurrentCase: typeof value === "function"
                    ? value(get().caseEditsBelongingToCurrentCase)
                    : value,
            }),
        }),
        {
            name: "case-storage",
            partialize: (state) => ({
                caseEdits: state.caseEdits,
                editIndexes: state.editIndexes,
            }),
        },
    ),
)
