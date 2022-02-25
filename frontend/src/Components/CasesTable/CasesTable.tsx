import { Table, Typography } from "@equinor/eds-core-react"
import { VoidFunctionComponent } from "react"
import styled from "styled-components"

import { SortableTable } from "../SortableTable"
import { Column, SortDirection } from "../SortableTable/types"

import { sort } from "./helpers"

import { Case } from "../../models/Case"

const CellWithBorder = styled(Table.Cell)`
    border-right: 1px solid lightgrey;
`

const columns: Column[] = [
    { name: "Case", accessor: "caseName", sortable: true },
    { name: "Gas injectors", accessor: "gasInjectorsCount", sortable: true },
    { name: "Well injectors", accessor: "wellInjectorsCount", sortable: true },
    { name: "Producers", accessor: "producersCount", sortable: true },
    { name: "Templates", accessor: "templatesCount", sortable: true },
    { name: "Gas capacity", accessor: "gasCapacity", sortable: true },
    { name: "Oil capacity", accessor: "oilCapacity", sortable: true },
    { name: "DG4 Date", accessor: "dg4Date", sortable: true },
]

type Props = {
    cases: Case[]
}

const CasesTable: VoidFunctionComponent<Props> = ({ cases }) => {
    const sortOnAccessor = (
        a: any,
        b: any,
        _accessor: string,
        sortDirection: SortDirection,
    ) => sort(a, b, sortDirection)

    const renderRow = (caseItem: Components.Schemas.CaseDto, index: number) => (
        <Table.Row key={index}>
            {columns.map((c) => (
                <CellWithBorder key={c.accessor}>
                    <Typography>{c.accessor}</Typography>
                </CellWithBorder>
            ))}
        </Table.Row>
    )

    return (
        <SortableTable
            columns={columns}
            data={cases}
            sortOnAccessor={sortOnAccessor}
            renderRow={renderRow}
        />
    )
}

export default CasesTable
