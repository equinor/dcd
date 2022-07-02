import { tokens } from "@equinor/eds-tokens"
import { Table } from "@equinor/eds-core-react"
import { ReactElement } from "react"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"

interface Props {
    wells: Well[]
    wellCases: WellCase[] | undefined | null
}

function WellTableRow({ wells, wellCases }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    const GenerateWellTableRows = (): ReactElement[] => {
        const tableRows: JSX.Element[] = []
        wells?.forEach((w) => {
            const wc = wellCases?.find((x) => x.wellId === w.id)
            // tableRows.push((<WellTableRow key={w.id} well={w} wellCase={wc} />))
            tableRows.push((
                <Table.Row>
                    <Table.Cell>
                        {wc?.count ?? 0}
                    </Table.Cell>
                    <Table.Cell>
                        {w.name}
                    </Table.Cell>
                    <Table.Cell>
                        {w.category}
                    </Table.Cell>
                    <Table.Cell>
                        {w.drillingDays}
                    </Table.Cell>
                    <Table.Cell>
                        {w.wellCost}
                    </Table.Cell>
                </Table.Row>))
        })
        return tableRows
    }

    return (<>{GenerateWellTableRows()}</>)
}

export default WellTableRow
