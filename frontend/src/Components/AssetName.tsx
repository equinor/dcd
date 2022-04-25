import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler, Dispatch, SetStateAction } from "react"
import {
    AssetHeader, WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setName: Dispatch<SetStateAction<string>>
    name: string
    setHasChanges: Dispatch<SetStateAction<boolean>>
}

const AssetName = ({
    setName,
    name,
    setHasChanges,
}: Props) => {
    const onNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetHeader>
            <WrapperColumn>
                <Label htmlFor="name" label="Name" />
                <Input
                    id="name"
                    name="name"
                    placeholder="Enter name"
                    value={name}
                    onChange={onNameChange}
                />
            </WrapperColumn>
        </AssetHeader>
    )
}

export default AssetName
