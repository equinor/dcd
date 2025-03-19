import { SetStateAction } from "react"
import { create } from "zustand"
import { persist } from "zustand/middleware"

import { EditInstance } from "../Models/Interfaces"

interface AppState {
    isCreating: boolean;
    setIsCreating: (value: boolean) => void;
    isLoading: boolean;
    setIsLoading: (value: boolean) => void;
    isSaving: boolean;
    setIsSaving: (value: boolean) => void;
    editMode: boolean;
    setEditMode: (value: boolean) => void;
    developerMode: boolean;
    setDeveloperMode: (value: boolean) => void;
    sidebarOpen: boolean;
    setSidebarOpen: (value: boolean) => void;
    showRevisionReminder: boolean;
    setShowRevisionReminder: (value: boolean) => void;
    snackBarMessage?: string;
    setSnackBarMessage: (value: SetStateAction<string | undefined>) => void;
    isCalculatingProductionOverrides: boolean;
    setIsCalculatingProductionOverrides: (value: boolean) => void;
    isCalculatingTotalStudyCostOverrides: boolean;
    setIsCalculatingTotalStudyCostOverrides: (value: boolean) => void;
    apiQueue: EditInstance[];
    setApiQueue: (value: SetStateAction<EditInstance[]>) => void;
}

export const useAppStore = create<AppState>()(
    persist(
        (set, get) => ({
            isCreating: false,
            setIsCreating: (value) => set({ isCreating: value }),
            isLoading: true,
            setIsLoading: (value) => set({ isLoading: value }),
            isSaving: false,
            setIsSaving: (value) => set({ isSaving: value }),
            editMode: false,
            setEditMode: (value) => set({ editMode: value }),
            developerMode: false,
            setDeveloperMode: (value) => set({ developerMode: value }),
            sidebarOpen: true,
            setSidebarOpen: (value) => set({ sidebarOpen: value }),
            showRevisionReminder: false,
            setShowRevisionReminder: (value) => set({ showRevisionReminder: value }),
            snackBarMessage: undefined,
            setSnackBarMessage: (value) => set({
                snackBarMessage: typeof value === "function"
                    ? value(get().snackBarMessage)
                    : value,
            }),
            isCalculatingProductionOverrides: false,
            setIsCalculatingProductionOverrides: (value) => set({ isCalculatingProductionOverrides: value }),
            isCalculatingTotalStudyCostOverrides: false,
            setIsCalculatingTotalStudyCostOverrides: (value) => set({ isCalculatingTotalStudyCostOverrides: value }),
            apiQueue: [],
            setApiQueue: (value) => set({
                apiQueue: typeof value === "function"
                    ? value(get().apiQueue)
                    : value,
            }),
        }),
        {
            name: "app-storage",
            partialize: (state) => ({
                editMode: state.editMode,
                developerMode: state.developerMode,
                sidebarOpen: state.sidebarOpen,
            }),
        },
    ),
)
