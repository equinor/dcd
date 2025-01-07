import { Button, Icon } from "@equinor/eds-core-react"
import { more_vertical } from "@equinor/eds-icons"

interface MenuButtonCellProps {
    data: {
        id: string
    }
    onMenuClick: (id: string, target: HTMLElement) => void
}

export const MenuButtonCell = ({ data, onMenuClick }: MenuButtonCellProps) => (
    <Button
        variant="ghost"
        onClick={(e) => onMenuClick(data.id, e.currentTarget)}
    >
        <Icon data={more_vertical} />
    </Button>
)
