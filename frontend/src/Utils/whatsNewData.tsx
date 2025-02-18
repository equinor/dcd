import { Version, WhatsNewUpdates } from "@/Models/Interfaces"
import { sortVersions } from "./common"

export const whatsNewUpdates: WhatsNewUpdates = {
    "2.0.0": {
        "New Functionalities": [
            { description: "All input fields Auto-save on change" },
            { description: "Upload photos in the case description tab" },
            { description: "Set the classification level of the project in the settings tab" },
            { description: "View the projects and cases in either edit or view mode" },
            { description: "The current case tab is saved in the URL for sharing" },
            { description: "Allow inserting 4 decimals instead of 2 in production profile table" },
            { description: "Navigate the project and add new cases in the sidebar" },
            { description: "A modal box appears with information about all changes whenever there is a new release" },
            { description: "View 'Export to stea' data in each case summary tab" },
            { description: "Added new production profile 'Deferred oil production' & 'Deferred gas production'" },
            { description: "Added input validation with descriptive error messages" },
        ],
        "UI Improvements": [
            { description: "Case and project description input text can now have formatting" },
            { description: "Improved grid and responsiveness" },
            { description: "Improved project selection landing page" },
            { description: "Improved loading indicators" },
            { description: "Improved sidebar styling" },
            { description: "Added case deletion confirmation prompt" },
            { description: "Added table description to Well costs" },
            { description: "When creating a new case, user is no longer automatically redireted to it" },
            { description: "Removed send feedback button" },
            { description: "Open cases directly from the Case overview table" },
            { description: "Combined title and unit into a single column to save space" },
            { description: "Show only relevant DG dates in Scehdule tab" },
        ],
        Bugfixes: [
            { description: "Technical input window does not appear in project 'chloris'" },
            { description: "Removed 'Expected Ream MNOK'21 from STEA export" },
            { description: "Pasting numbers from Excel to tables puts all inserted values into a single cell" },
            { description: "Unable to add numbers to tables if they have custom format from Excel" },
        ],
    },
    "2.1.0": {
        "New Functionalities": [
            { description: "Undo/redo edits" },
            { description: "Edit history overview in the sidebar displays all changes made to each case in the past hour" },
            { description: "Improve autosave functionality" },
            { description: "Show cash flow in cost tab" },
        ],
        "UI Improvements": [
            { description: "Added loading indicators" },
            { description: "Removed modal for overriding time series" },
        ],
        Bugfixes: [
            { description: "Misc. Sharepoint PROSP import fixes" },
            { description: "Set table year ranges from from time series data" },
            { description: "Fix calucalations always using manual input if manual input was ever used" },
            { description: "Fix navigating to project view when changing project context" },
        ],
    },
    "2.2.0": {
        "New Functionalities": [
            { description: "Show manual input for facility cost profiles before PROSP import" },
            { description: "Add loading indicator to calculated time series when calculations are running" },
            { description: "Disable/enable inputs based on access" },
            { description: "Add calculations for NPV and Break even" },
            { description: "Add additional fields for oil and gas production in production profiles and STEA export" },
            { description: "Implemented archived cases" },
            { description: "Projet phase pre dg0 can be manually changed. After dg0, phase is set from project master" },
            { description: "Add cashflow graph to cost tab" },
        ],
        "UI Improvements": [
            { description: "Rename \"Gas production\" to \"Rich gas production\"" },
            { description: "Always show a minimum number of columns in tables" },
            { description: "Change phase names APx, APy and APz to APbo, BOR and VPbo" },
            { description: "Added tooltips to certain buttons" },
        ],
        Bugfixes: [
            { description: "Fixed issue with deleting case with an image" },
            { description: "Remove archived cases from case comparison graph" },
            { description: "Add units to facilities tab in view mode" },
            { description: "Fix study cost calculation in cases where DG2 and DG3 were both set to Jan 1st of the same year" },
            { description: "Fix updating case from the cases table in project view" },
            { description: "Fix so the user can manually type dates in schedule tab" },
            { description: "Fix issue with case comparison graph showing wrong data for the cases" },
            { description: "Fix so that undo/redo on pasted range should update all values" },
        ],
    },
    "2.2.1": {
        "New Functionalities": [
            { description: "Add, limit or remove access to a project in the new Access Management tab" },
        ],
        "UI Improvements": [
            { description: "Minor UI Improvements" },
        ],
        Bugfixes: [
            { description: "Minor bugfixes and performance improvements" },
        ],
    },
    "2.3.0": {
        "New Functionalities": [
            { description: "Create project revisions - Snapshots of projects and cases that preserve the state at time of creation" },
            { description: "Access control enforcement based on project classification (Internal/Restricted/Confidential)" },
            { description: "Add caption to images" },
            { description: "Add selected project tab to the URL" },
        ],
        "UI Improvements": [
            { description: "Minor UI Improvements" },
        ],
        Bugfixes: [
            { description: "Fix navigation between case and project with the browser's back button" },
            { description: "Fix issue where the CO2 tab on project was reset when entering edit mode" },
            { description: "Minor bugfixes and performance improvements" },
        ],
    },
    "2.3.1": {
        "New Functionalities": [
            { description: "Access Management: add, limit or remove access to a project in the new Access Management tab" },
            { description: "Autosave on CO2 and Well Cost in Technical Input" },
            { description: "Calculate break even" },
            { description: "Calculate net present value" },
        ],
        "UI Improvements": [
            { description: "Added a new homepage, for when a project is not selected" },
            { description: "Minor UI Improvements" },
        ],
        Bugfixes: [
            { description: "Paste multiple rows in tables" },
            { description: "Major performance improvements for table input, autosave and some endpoints" },
            { description: "Minor bugfixes" },
        ],
    },
}

export const versions = sortVersions(Object.keys(whatsNewUpdates)) as Version[]
