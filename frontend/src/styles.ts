import { createGlobalStyle } from "styled-components"
import { EnvironmentVariables } from "./environmentVariables"

const env = EnvironmentVariables.ENVIRONMENT

export default createGlobalStyle`
    body {
        --header-height: ${env === "dev" ? "48px" : "calc((var(--grid-unit)* 10px) + 2px)"};
        --controls-height: 96px;
        margin: 0;
        @media (width >= 1064px) {
            --controls-height: 56px;
        }
    }

    * {
        scrollbar-width: thin;
        box-sizing: border-box!important;
    }

    .App {
        font-family: Equinor;
    }

    div[class*="TabList"][role="tablist"] {
        position: relative;
        z-index: 1;
        scrollbar-width: none;
    }

    div[role="rowgroup"] div[role="row"]:last-of-type {
        border-bottom: none;
    }

    div[class*="SideBarContent"] .MuiGrid-container .MuiGrid-item:last-child span[class*="SidebarLink"] {
        border-bottom: none;
    }

    label[class*="Label__LabelBase"] {
        margin-left: 0;
    }

    .GhostButton::after {
        display: none;
    }
    @media (hover: hover) and (pointer: fine) {
        .GhostButton:hover {
            background-color: transparent;
        }
    }

    .ag-theme-alpine-fusion .ag-row-odd {
        background-color: var(--ag-background-color);
    }

    .ag-theme-alpine-fusion .ag-cell-inline-editing {
        padding: 0;
    }

    .ag-theme-alpine-fusion [role="gridcell"].editableCell:not(.ag-cell-range-selected) {
        background-color: var(--UI-Background-Light, #F7F7F7);
        border-right: 1px solid #DBDBDB;
    }

    .ag-aria-description-container {
        display: none !important;
      }
    
      .red-cell {
        background-color: #FFC0C1 !important;
    }

    .ag-center-cols-viewport {
    min-height: 42px !important;
    }

    .highlighted {
    border: 2px solid rgb(0, 79, 85);
    transition: background-color 0.5s ease;
    }
`
