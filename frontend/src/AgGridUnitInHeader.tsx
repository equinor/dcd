export const customUnitHeaderTemplate = (title: string, unit?: string): string => "<div class=\"ag-cell-label-container custom-ag-header\" role=\"presentation\">"
        + "  <span ref=\"eMenu\" class=\"ag-header-icon ag-header-cell-menu-button\"></span>"
        + "  <div ref=\"eLabel\" class=\"ag-header-cell-label\" role=\"presentation\">"
        + "    <span ref=\"eText\" class=\"ag-header-cell-text\" role=\"columnheader\"></span>"
        + "    <span ref=\"eSortOrder\" class=\"ag-header-icon ag-sort-order\"></span>"
        + "    <span ref=\"eSortAsc\" class=\"ag-header-icon ag-sort-ascending-icon\"></span>"
        + "    <span ref=\"eSortDesc\" class=\"ag-header-icon ag-sort-descending-icon\"></span>"
        + "    <span ref=\"eSortNone\" class=\"ag-header-icon ag-sort-none-icon\"></span>"
        + "    <span ref=\"eFilter\" class=\"ag-header-icon ag-filter-icon\"></span>"
        + "    <div class=\"abc\">"
        + `    ${title}`
        + "    <div class=\"abc\" style=\"font-size: 10px; font-weight: normal; margin-top: 4px; color: #6F6F6F\">"
        + `    ${unit}`
        + "</div>"
