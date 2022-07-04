import { tokens } from "@equinor/eds-tokens"
import { Button, Table } from "@equinor/eds-core-react"
import { MouseEventHandler, ReactElement } from "react"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"

interface Props {
    wells: Well[]
    wellCases: WellCase[] | undefined | null
}

function WellTableRow({ wells, wellCases }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    const AddWell = (well: Well, wellCase: WellCase | undefined) => {
        if (!wellCase) {
            // Create wellCase
        } else {
            const newWellCase = { ...wellCase }
            newWellCase.count! += 1
            // setWellCases
        }
    }

    const GenerateWellTableRows = (): ReactElement[] => {
        const tableRows: JSX.Element[] = []
        wells?.forEach((w) => {
            const wc = wellCases?.find((x) => x.wellId === w.id)
            // tableRows.push((<WellTableRow key={w.id} well={w} wellCase={wc} />))
            tableRows.push((
                <Table.Row key={w.id}>
                    <Table.Cell>
                        {wc?.count ?? 0}
                        <Button onClick={() => AddWell(w, wc)}>Add well</Button>
                        <Button>Remove well</Button>
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
