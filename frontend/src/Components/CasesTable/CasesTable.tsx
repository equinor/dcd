import { useCallback } from 'react'
import { Link } from 'react-router-dom'
import styled from 'styled-components'
import { Table, Typography } from '@equinor/eds-core-react'

import { sort } from './helpers'
import SortableTable, { Column, SortDirection } from './SortableTable'
import { GetDrainageStrategy } from '../../Utils/common'


const { Row, Cell } = Table

const CellWithBorder = styled(Cell)`
    border-right: 1px solid lightgrey;
`

const LinkWithoutStyle = styled(Link)`
    text-decoration: none;
`

const columns: Column[] = [
    { name: 'Name', accessor: 'name', sortable: true },
    { name: 'PPG', accessor: 'ppg', sortable: true },
    { name: 'PPO', accessor: 'ppo', sortable: true },
    { name: 'NGL Yield', accessor: 'nglYield', sortable: true },
]

interface Props {
    project: Components.Schemas.ProjectDto
    cases: Components.Schemas.CaseDto[]
}

const CasesTable = ({ cases, project }: Props) => {
    const getPpg = useCallback((c: Components.Schemas.CaseDto) => {
        const drainageStrategy = GetDrainageStrategy(project, c.drainageStrategyLink);
        return drainageStrategy?.productionProfileGas?.values?.reduce((sum, value) => sum + value, 0) ?? 0
    }, [])

    const getPpo = useCallback((c: Components.Schemas.CaseDto) => {
        const drainageStrategy = GetDrainageStrategy(project, c.drainageStrategyLink);
        return drainageStrategy?.productionProfileOil?.values?.reduce((sum, value) => sum + value, 0) ?? 0
    }, [])

    const sortOnAccessor = (a: Components.Schemas.CaseDto, b: Components.Schemas.CaseDto, accessor: string, sortDirection: SortDirection) => {
        switch (accessor) {
            case 'name': {
                return sort(a.name!.toLowerCase(), b.name!.toLowerCase(), sortDirection)
            }
            case 'ppg': {
                return sort(getPpg(a), getPpg(b), sortDirection)
            }
            case 'ppo': {
                return sort(getPpo(a), getPpo(b), sortDirection)
            }
            case 'nglYield': {
                const drainageStrategyA = GetDrainageStrategy(project, a.drainageStrategyLink);
                const drainageStrategyB = GetDrainageStrategy(project, b.drainageStrategyLink);
                return sort(drainageStrategyA!.nglYield!, drainageStrategyB!.nglYield!, sortDirection)
            }
            default:
                return sort(a.name!.toLowerCase(), b.name!.toLowerCase(), sortDirection)
        }
    }

    const renderRow = (caseItem: Components.Schemas.CaseDto, index: number) => {
        return (
            <Row key={index}>
                <CellWithBorder>
                    <LinkWithoutStyle to={`/project/${project.projectId}/case/${caseItem.id}`}>
                        <Typography
                            color="primary"
                            variant="body_short"
                            token={{
                                fontSize: '1.2rem',
                            }}
                        >
                            {caseItem.name}
                        </Typography>
                    </LinkWithoutStyle>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{getPpg(caseItem)} USD</Typography>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{getPpo(caseItem)} USD</Typography>
                </CellWithBorder>
                <CellWithBorder>
                    <Typography>{GetDrainageStrategy(project, caseItem.drainageStrategyLink)?.nglYield}</Typography>
                </CellWithBorder>
            </Row>
        )
    }

    return <SortableTable columns={columns} data={cases} sortOnAccessor={sortOnAccessor} renderRow={renderRow} />
}

export default CasesTable
