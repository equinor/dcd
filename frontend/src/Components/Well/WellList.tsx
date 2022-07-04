import { tokens } from "@equinor/eds-tokens"
import { Button, Table, Typography } from "@equinor/eds-core-react"
import { well } from "@equinor/eds-icons"
import { useState } from "react"
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
    const [wells, setWells] = useState<Well[]>(project.wells ?? [])
    const [wellCases, setWellCases] = useState<WellCase[]>(caseItem.wellCases ?? [])

    const CreateWell = () => {
        const newWell = new Well()
        newWell.category = 0
        newWell.name = "New well"
        const existingWells = [...wells]
        if (existingWells !== undefined && existingWells !== null) {
            existingWells.push(newWell)
            setWells(existingWells)
        }

        console.log(wells?.length)
    }

    return (
        <>
            <Button onClick={CreateWell} variant="outlined">Add new well</Button>
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
                    <WellTableRow wellCases={wellCases} wells={wells!} caseItem={caseItem} />
                </Table.Body>
            </Table>
        </>
    )
}

export default WellList
