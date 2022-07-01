import { tokens } from "@equinor/eds-tokens"
import { Table, Typography } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import WellTableRow from "./WellTableRow"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"
import { Case } from "../../models/Case"

interface Props {
    project: Project
    caseItem: Case
}

function WellList({ project, caseItem }: Props) {
    const { wells } = project
    const { wellCases } = caseItem

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        wells?.forEach((w) => {
            const wc = wellCases?.find((x) => x.wellId === w.id)
            tableRows.push((<WellTableRow key={w.id} well={w} wellCase={wc} />))
        })
        return tableRows
    }

    return (
        <Table>
            <Table.Caption>
                <Typography variant="h2">
                    Wells
                </Typography>
            </Table.Caption>
            <Table.Head>
                <Table.Row>
                    <Table.Cell>
                        Count
                    </Table.Cell>
                    <Table.Cell>
                        Well name
                    </Table.Cell>
                    <Table.Cell>
                        Well type
                    </Table.Cell>
                    <Table.Cell>
                        Drilling days
                    </Table.Cell>
                    <Table.Cell>
                        Well cost
                    </Table.Cell>
                </Table.Row>
            </Table.Head>
            <Table.Body>
                {GenerateWellTableRows()}
            </Table.Body>
        </Table>
    )
}

export default WellList
