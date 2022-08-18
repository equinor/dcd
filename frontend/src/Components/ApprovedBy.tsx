import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import {
    AssetHeader, WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setApprovedBy: Dispatch<SetStateAction<string>>
    approvedBy: string
    setHasChanges: Dispatch<SetStateAction<boolean>>
}

const ApprovedBy = ({
    setApprovedBy,
    approvedBy,
    setHasChanges,
}: Props) => {
    const onApprovedByChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setApprovedBy(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetHeader>
            <WrapperColumn>
                <Label htmlFor="approvedBy" label="Approved by" />
                <Input
                    id="approvedBy"
                    name="approvedBy"
                    placeholder="Enter approvers name"
                    value={approvedBy}
                    onChange={onApprovedByChange}
                />
            </WrapperColumn>
        </AssetHeader>
    )
}

export default ApprovedBy
