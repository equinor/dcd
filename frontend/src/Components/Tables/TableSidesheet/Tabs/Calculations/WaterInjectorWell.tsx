import WellBase from "./WellBase"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const WaterInjectorWell: React.FC<Props> = ({ developerMode, hasOverride }) => {
    return (
        <WellBase
            developerMode={developerMode}
            hasOverride={hasOverride}
            wellType="Water Injector"
            wellCategory="Water_Injector"
        />
    )
}

export default WaterInjectorWell 