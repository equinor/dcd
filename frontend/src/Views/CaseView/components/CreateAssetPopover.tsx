import { Button, EdsProvider, Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { VoidFunctionComponent } from "react"
import styled from "styled-components"
import { AssetTypes } from "../../../Components/CreateAssetForm/types"

import { Popover } from "../../../Components/Popover"

const StyledPopover = styled(Popover)`
    overflow: hidden;
`

const PopoverContent = styled.div`
    display: flex;
    flex-direction: column;
`

const PopoverButton = styled(Button)`
    padding-left: 8px;

    &,
    &:hover {
        border-radius: 0;
    }

    span {
        justify-content: flex-start;
    }
`

type Props = {
    popoverAnchor: HTMLElement | null
    onDismiss: (e: Event) => void
    onAssetTypeClick: (assetType: AssetTypes) => void
}

export const CreateAssetPopover: VoidFunctionComponent<Props> = ({ popoverAnchor, onDismiss, onAssetTypeClick }) => (
    <EdsProvider density="comfortable">
        <StyledPopover anchor={popoverAnchor} onDismiss={onDismiss}>
            <PopoverContent>
                <PopoverButton
                    variant="ghost"
                    color="secondary"
                    onClick={() => onAssetTypeClick(AssetTypes.DrainageStrategy)}
                >
                    <Icon data={add} />
                    Drainage strategy
                </PopoverButton>
                <PopoverButton
                    variant="ghost"
                    color="secondary"
                    onClick={() => onAssetTypeClick(AssetTypes.Exploration)}
                >
                    <Icon data={add} />
                    Exploration
                </PopoverButton>
                <PopoverButton
                    variant="ghost"
                    color="secondary"
                    onClick={() => onAssetTypeClick(AssetTypes.WellProject)}
                >
                    <Icon data={add} />
                    Well project
                </PopoverButton>
                <PopoverButton
                    variant="ghost"
                    color="secondary"
                    onClick={() => onAssetTypeClick(AssetTypes.SURF)}
                >
                    <Icon data={add} />
                    SURF
                </PopoverButton>
            </PopoverContent>
        </StyledPopover>
    </EdsProvider>
)
