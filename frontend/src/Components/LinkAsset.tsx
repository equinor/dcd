import {
    NativeSelect,
} from "@equinor/eds-core-react"
import React, {
    ChangeEvent,
} from "react"
import styled from "styled-components"

const AssetDropdown = styled(NativeSelect)`
    width: 20rem;
    padding-bottom: 2em;
`

interface Props {
    assetName: string,
    linkAsset: (event: React.ChangeEvent<HTMLSelectElement>, link: any) => void,
    link: string,
    currentValue: string | undefined
    values: JSX.Element[]
}

const LinkAsset = ({
    assetName,
    linkAsset,
    link,
    currentValue,
    values,
}: Props) => (
    <AssetDropdown
        label={assetName}
        id="asset"
        placeholder="Choose an asset"
        onChange={(event: ChangeEvent<HTMLSelectElement>) => linkAsset(event, link)}
        value={currentValue}
    >
        {values}
        <option key="00000000-0000-0000-0000-000000000000" value="00000000-0000-0000-0000-000000000000"> </option>
    </AssetDropdown>
)

export default LinkAsset
