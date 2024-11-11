import {
    FC,
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    useEffect,
} from "react"
import { useQuery } from "@tanstack/react-query"
import { featureToggleQueryFn } from "@/Services/QueryFunctions"

interface FeatureContextType {
    Features: Components.Schemas.FeatureToggleDto,

}

const FeatureContext = createContext<FeatureContextType | undefined>(undefined)

const FeatureContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const { data: featureApiData } = useQuery({
        queryKey: ["featureApiData", "featureToggleApiData"],
        queryFn: () => featureToggleQueryFn(),
    })

    const [Features, setFeatures] = useState<Components.Schemas.FeatureToggleDto>({ })

    useEffect(() => {
        if (featureApiData) {
            setFeatures(featureApiData)
        }
    }, [featureApiData])

    const value = useMemo(() => ({
        Features,

    }), [
        Features,

    ])

    return (
        <FeatureContext.Provider value={value}>
            {children}
        </FeatureContext.Provider>
    )
}

export const useFeatureContext = (): FeatureContextType => {
    const context = useContext(FeatureContext)
    if (context === undefined) {
        throw new Error("useAppContext must be used within an FeatureContextProvider")
    }
    return context
}

export { FeatureContextProvider }
