import { tokens } from "@equinor/eds-tokens"
import { Button, Table } from "@equinor/eds-core-react"
import { MouseEventHandler, ReactElement } from "react"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"
import { GetWellCaseService } from "../../Services/WellCaseService"
import { Case } from "../../models/Case"

interface Props {
    wells: Well[]
    wellCases: WellCase[] | undefined | null
    caseItem: Case
}

function WellTableRow({ wells, wellCases, caseItem }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    const IncreaseWellCase = (well: Well, wellCase: WellCase | undefined) => {
        if (!wellCase) {
            const newWellCase = new WellCase()
            newWellCase.wellId = well.id
            newWellCase.caseId = caseItem.id
            newWellCase.count = 1
            const newProject = GetWellCaseService().createWellCase(newWellCase)
        } else {
            const newWellCase = { ...wellCase }
            newWellCase.count! += 1
            const newProject = GetWellCaseService().updateWellCase(newWellCase)
        }
    }

    const DecreaseWellCase = (well: Well, wellCase: WellCase | undefined) => {
        if (!wellCase) {
            const newWellCase = new WellCase()
            newWellCase.wellId = well.id
            newWellCase.caseId = caseItem.id
            newWellCase.count = 1
            const newProject = GetWellCaseService().createWellCase(newWellCase)
        } else {
            const newWellCase = { ...wellCase }
            newWellCase.count! -= 1
            const newProject = GetWellCaseService().updateWellCase(newWellCase)
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
                        <Button onClick={() => IncreaseWellCase(w, wc)}>Increase</Button>
                        <Button onClick={() => DecreaseWellCase(w, wc)}>Decrease</Button>
                        {wc?.wellId}
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
