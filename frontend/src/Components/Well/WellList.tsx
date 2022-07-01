import React from "react"
import Plot from "react-plotly.js"
import { tokens } from "@equinor/eds-tokens"
import { Table, Typography } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import WellTableRow from "./WellTableRow"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"

interface Props {
    project: Project
}

function WellList({ project }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba
    const well = new Well()
    well.name = "Oil producer"
    well.category = 2
    well.drillingDays = 30
    well.wellCost = 299
    const wellCase = new WellCase()
    wellCase.count = 10

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

                <WellTableRow well={well} wellCase={wellCase} />
                <WellTableRow well={well} wellCase={wellCase} />
            </Table.Body>
        </Table>
    )
}

export default WellList
