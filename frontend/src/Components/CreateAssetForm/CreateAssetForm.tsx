import { MouseEventHandler, VoidFunctionComponent } from "react"
import { CreateDrainageStrategyForm } from "./forms/CreateDrainageStrategyForm"

import { AssetTypes } from "./types"

type Props = {
    asset: AssetTypes
    onCancel: MouseEventHandler<HTMLButtonElement>
}

export const CreateAssetForm: VoidFunctionComponent<Props> = ({ asset, onCancel }) => {
    switch (asset) {
    case AssetTypes.DrainageStrategy:
        return <CreateDrainageStrategyForm onCancel={onCancel} />
    default:
        return null
    }
}
