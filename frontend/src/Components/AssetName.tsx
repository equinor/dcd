import { Input, Label } from "@equinor/eds-core-react"
import { ChangeEventHandler } from "react"
import {
 AssetHeader, WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    setName: React.Dispatch<React.SetStateAction<string>>
    name: string
    setHasChanges: React.Dispatch<React.SetStateAction<boolean>>
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
