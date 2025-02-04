import WellBase from "./WellBase"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const GasProducerWell: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <WellBase
        developerMode={developerMode}
        hasOverride={hasOverride}
        wellType="Gas Producer"
        wellCategory="Gas_Producer"
    />
)

export default GasProducerWell
