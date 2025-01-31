import WellBase from "./WellBase"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const GasInjectorWell: React.FC<Props> = ({ developerMode, hasOverride }) => {
    return (
        <WellBase
            developerMode={developerMode}
            hasOverride={hasOverride}
            wellType="Gas Injector"
            wellCategory="Gas_Injector"
        />
    )
}

export default GasInjectorWell 