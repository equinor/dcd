export type SortDirection = 'ascending' | 'descending' | 'none'

export type Column = {
    name: string
    accessor: string
    sortable: boolean
}
