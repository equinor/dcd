import { chevron_down, chevron_up } from '@equinor/eds-icons'
import { Icon, Table } from '@equinor/eds-core-react'
import { useMemo, useState, ReactElement } from 'react'
import styled from 'styled-components'
import { Column, SortDirection } from './types'

const { Body, Row, Cell, Head } = Table

const Wrapper = styled.div`
    width: 40rem;
`

const StyledTable = styled(Table)`
    width: 100%;
`

const StyledCell = styled(Cell)<{ sortable?: boolean }>`
    cursor: ${props => (props.sortable ? 'pointer' : 'default')};
`

Icon.add({ chevron_down, chevron_up })

const SortIcon = styled(Icon)<{
    name: string
    isSelected: boolean
}>`
    visibility: ${({ isSelected }) => (isSelected ? 'visible' : 'hidden')};
`

type Props<T> = {
    columns: Column[]
    data: T[]
    sortOnAccessor: (a: T, b: T, accessor: string, sortDirection: SortDirection) => number
    renderRow: (dataObject: T, index: number) => React.ReactChild
}

const SortableTable = <T extends unknown>({ columns, data, sortOnAccessor, renderRow }: Props<T>): ReactElement | null => {
    const [sortDirection, setSortDirection] = useState<SortDirection>('none')
    const [columnToSortBy, setColumnToSortBy] = useState<Column>()

    const sortedData = useMemo(() => {
        if (columnToSortBy) {
            return [...data].sort((a: T, b: T) => {
                const { accessor } = columnToSortBy

                return sortOnAccessor(a, b, accessor, sortDirection)
            })
        }
        return data
    }, [columnToSortBy, sortDirection, data, sortOnAccessor])

    const setSortOn = (selectedColumn: Column) => {
        if (columnToSortBy && selectedColumn.name === columnToSortBy.name) {
            if (sortDirection === 'ascending') {
                setSortDirection('descending')
            } else {
                setSortDirection('none')
                setColumnToSortBy(undefined)
            }
        } else {
            setColumnToSortBy(selectedColumn)
            setSortDirection('ascending')
        }
    }

    return (
        <Wrapper>
            <StyledTable>
                <Head>
                    <Row>
                        {columns.map(column => {
                            const isSelected = columnToSortBy ? column.name === columnToSortBy.name : false
                            return (
                                <StyledCell
                                    key={column.name}
                                    onClick={column.sortable ? () => setSortOn(column) : undefined}
                                    sort={isSelected ? sortDirection : 'none'}
                                    sortable={column.sortable}
                                >
                                    {column.name}
                                    {column.sortable && (
                                        <SortIcon
                                            name={sortDirection === 'descending' ? 'chevron_up' : 'chevron_down'}
                                            isSelected={isSelected}
                                        />
                                    )}
                                </StyledCell>
                            )
                        })}
                    </Row>
                </Head>
                <Body>{sortedData.map((dataObject, index) => renderRow(dataObject, index))}</Body>
            </StyledTable>
        </Wrapper>
    )
}

export default SortableTable
