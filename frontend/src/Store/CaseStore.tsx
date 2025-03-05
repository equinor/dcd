import { create } from "zustand"
import { SetStateAction } from "react"

interface CaseState {
    activeTabCase: number;
    setActiveTabCase: (value: SetStateAction<number>) => void;
}

export const useCaseStore = create<CaseState>((set, get) => ({
    activeTabCase: 0,
    setActiveTabCase: (value) => set({
        activeTabCase: typeof value === "function"
            ? value(get().activeTabCase)
            : value,
    }),
}))
