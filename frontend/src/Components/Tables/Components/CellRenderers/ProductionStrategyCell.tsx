import { PRODUCTION_STRATEGY } from "@/Utils/Config/constants"

interface ProductionStrategyCellProps {
    value: Components.Schemas.ProductionStrategyOverview
}

export const ProductionStrategyCell = ({ value }: ProductionStrategyCellProps) => {
    const strategyString = PRODUCTION_STRATEGY[value] ?? "Unknown"

    return <div>{strategyString}</div>
}
