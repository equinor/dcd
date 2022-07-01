import { tokens } from "@equinor/eds-tokens"
import { Table } from "@equinor/eds-core-react"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"

interface Props {
    well: Well
    wellCase: WellCase | undefined
}

function WellTableRow({ well, wellCase }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    return (

        <Table.Row>
            <Table.Cell>
                {wellCase?.count ?? 0}
            </Table.Cell>
            <Table.Cell>
                {well.name}
            </Table.Cell>
            <Table.Cell>
                {well.category}
            </Table.Cell>
            <Table.Cell>
                {well.drillingDays}
            </Table.Cell>
            <Table.Cell>
                {well.wellCost}
            </Table.Cell>
        </Table.Row>

    )
}

export default WellTableRow
