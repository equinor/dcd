import { Table, Typography } from '@equinor/eds-core-react'
import { VoidFunctionComponent } from 'react'
import styled from 'styled-components'
import { useTranslation } from "react-i18next";

import { SortableTable } from '../SortableTable'
import { Column, SortDirection } from '../SortableTable/types'

import { sort } from './helpers'

import { Case } from '../../models/Case'

const CellWithBorder = styled(Table.Cell)`
    border-right: 1px solid lightgrey;
`
const { t } = useTranslation();

const columns: Column[] = [
    { name: t('CasesTable.Case'), accessor: 'caseName', sortable: true },
    { name: t('CasesTable.GasInjectors'), accessor: 'gasInjectorsCount', sortable: true },
    { name: t('CasesTable.WellInjectors'), accessor: 'wellInjectorsCount', sortable: true },
    { name: t('CasesTable.Producers'), accessor: 'producersCount', sortable: true },
    { name: t('CasesTable.Templates'), accessor: 'templatesCount', sortable: true },
    { name: t('CasesTable.GasCapacity'), accessor: 'gasCapacity', sortable: true },
    { name: t('CasesTable.OilCapacity'), accessor: 'oilCapacity', sortable: true },
    { name: t('CasesTable.DG4Date'), accessor: 'dg4Date', sortable: true },
]

type Props = {
    projectId?: string
    cases: Case[]
}

const CasesTable: VoidFunctionComponent<Props> = ({ cases, projectId }) => {
    const sortOnAccessor = (a: any, b: any, _accessor: string, sortDirection: SortDirection) => {
        return sort(a, b, sortDirection)
    }

    const renderRow = (caseItem: Components.Schemas.CaseDto, index: number) => {
        return (
            <Table.Row key={index}>
                {columns.map((c) => (
                    <CellWithBorder key={c.accessor}>
                        <Typography>{c.accessor}</Typography>
                    </CellWithBorder>
                ))}
            </Table.Row>
        )
    }

    return <SortableTable columns={columns} data={cases} sortOnAccessor={sortOnAccessor} renderRow={renderRow} />
}

export default CasesTable
