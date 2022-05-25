import {
    Typography,
} from "@equinor/eds-core-react"
import {
    useState,
    useEffect,
} from "react"
import MetadataTypeEnum from "../models/MetadataTypeEnum"

interface Props {
    caseMetadataValue: Number | undefined,
    assetMetadataValue: Number | undefined
    metaData: MetadataTypeEnum
}

const MetadataMismatchWarning = ({
    caseMetadataValue,
    assetMetadataValue,
    metaData,
}: Props) => {
    const [warning, setWarning] = useState<string>("")

    const resetWarning = () => {
        setWarning("")
    }

    useEffect(() => {
        (async () => {
            if (caseMetadataValue !== assetMetadataValue) {
                return setWarning(`${metaData} does not match case ${metaData}`)
            }
            return resetWarning()
        })()
    })

    return (
        <Typography variant="h6" color="red">{warning}</Typography>
    )
}

export default MetadataMismatchWarning
