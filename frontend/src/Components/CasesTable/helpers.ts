import { SortDirection } from "../SortableTable/types"

export const sort = (a: string | boolean | number, b: string | boolean | number, sortDirection: SortDirection) => {
    if (a < b) {
        return sortDirection === 'ascending' ? -1 : 1
    }

    if (a > b) {
        return sortDirection === 'ascending' ? 1 : -1
    }

    return 0
}
