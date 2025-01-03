import { productionStrategyOverviewToString } from "@/Utils/common"

interface ProductionStrategyCellProps {
    value: Components.Schemas.ProductionStrategyOverview
}

export const ProductionStrategyCell = ({ value }: ProductionStrategyCellProps) => {
    const strategyString = productionStrategyOverviewToString(value)
    return <div>{strategyString}</div>
}
