import React from "react"
import Plot from "react-plotly.js"
import { tokens } from "@equinor/eds-tokens"
import { Table, Typography } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { WellCase } from "../../models/WellCase"

interface Props {
    well: Well
    wellCase: WellCase
}

function WellTableRow({ well, wellCase }: Props) {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    return (

        <Table.Row>
            <Table.Cell>
                {wellCase.count}
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
