import React from 'react'
import { Link } from 'react-router-dom'
import styled from 'styled-components'
import { Table, Typography } from '@equinor/eds-core-react'

import { sort } from './helpers'
import SortableTable, { Column, SortDirection } from './SortableTable'
import { Case } from '../SideMenu/SideMenu'

const { Row, Cell } = Table

const CellWithBorder = styled(Cell)`
    border-right: 1px solid lightgrey;
`

const columns: Column[] = [
    { name: 'Title', accessor: 'title', sortable: true },
    { name: 'Capex', accessor: 'capex', sortable: true },
    { name: 'Drillex', accessor: 'drillex', sortable: true },
    { name: 'UR', accessor: 'ur', sortable: true },
]

interface Props {
    projectId: string | undefined
    cases: Case[]
}

const CasesTable = ({ cases, projectId }: Props) => {
    const sortOnAccessor = (a: Case, b: Case, accessor: string, sortDirection: SortDirection) => {
        switch (accessor) {
            case 'title': {
                return sort(a.title.toLowerCase(), b.title.toLowerCase(), sortDirection)
            }
            case 'capex': {
                return sort(a.capex, b.capex, sortDirection)
            }
            case 'drillex': {
                return sort(a.drillex, b.drillex, sortDirection)
            }
            case 'ur': {
                return sort(a.ur, b.ur, sortDirection)
            }
            default:
                return sort(a.title.toLowerCase(), b.title.toLowerCase(), sortDirection)
        }
    }

    const renderRow = (caseItem: Case, index: number) => {
        return (
            <Row key={index}>
                <CellWithBorder>
                    <Link to={`/project/${projectId}/case/${caseItem.id}`} style={{ textDecoration: 'none' }}>
                        <Typography
                            color="primary"
                            variant="body_short"
                            token={{
                                fontSize: '1.2rem',
                            }}
                        >
                            {caseItem.title}
                        </Typography>
                    </Link>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{caseItem.capex} USD</Typography>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{caseItem.drillex} USD</Typography>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{caseItem.ur} Mbbl</Typography>
                </CellWithBorder>
            </Row>
        )
    }

    return <SortableTable columns={columns} data={cases} sortOnAccessor={sortOnAccessor} renderRow={renderRow} style={{ width: '40rem' }} />
}

export default CasesTable
