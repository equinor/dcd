import WellBase from "./WellBase"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const OilProducerWell: React.FC<Props> = ({ developerMode, hasOverride }) => {
    return (
        <WellBase
            developerMode={developerMode}
            hasOverride={hasOverride}
            wellType="Oil Producer"
            wellCategory="Oil_Producer"
        />
    )
}

export default OilProducerWell 