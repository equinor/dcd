import { createGlobalStyle } from "styled-components"
import { EnvironmentVariables } from "./environmentVariables"

const env = EnvironmentVariables.ENVIRONMENT

export default createGlobalStyle`
  body {
    --header-height: ${env === "dev" ? "48px" : "calc((var(--grid-unit)* 10px) + 2px)"};
    margin: 0;
  }

  * {
    scrollbar-width: thin;
  }

  .App {
    font-family: Equinor;
  }

  .ConceptApp.MainGrid,
  .ContentOverview,
  .ContentOverview .ContentPanels,
  .ContentOverview .ContentPanels > .MuiGrid-root,
  .ContentOverview .ContentPanels > .MuiGrid-root > .MuiGrid-root,
  .ContentOverview .ContentPanels > .MuiGrid-root > .MuiGrid-root > div {
    height: -webkit-fill-available;
    overflow: hidden;
  }

  div[class*="TabList"][role="tablist"] {
    position: relative;
    z-index: 1;
    scrollbar-width: none;
    overflow-y: none;
  }

  div[class*="TabList"][role="tablist"]:after {
    content: '';
    display: block;
    position: absolute;
    bottom: 0;
    width: 100%;
    height: 2px;
    background-color: var(--eds_ui_background__medium,rgba(220,220,220,1));
    z-index: -1;
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

  .ConceptApp .MuiDialogActions-root {
    padding: 20px 24px;
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
`
