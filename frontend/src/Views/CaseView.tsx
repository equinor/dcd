import { useEffect } from "react"
import { useParams } from "react-router-dom"
import Grid2 from "@mui/material/Grid2"
import styled from "styled-components"
import CaseDrillingScheduleTab from "@/Components/Case/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseProductionProfilesTab from "@/Components/Case/CaseProductionProfilesTab"
import CaseDescriptionTab from "@/Components/Case/CaseDescriptionTab"
import CaseFacilitiesTab from "@/Components/Case/CaseFacilitiesTab"
import CaseCO2Tab from "@/Components/Case/Co2Emissions/CaseCO2Tab"
import CaseCostTab from "@/Components/Case/CaseCost/CaseCostTab"
import CaseScheduleTab from "@/Components/Case/CaseScheduleTab/CaseScheduleTab"
import CaseSummaryTab from "@/Components/Case/CaseSummaryTab/CaseSummaryTab"
import { useCaseStore } from "@/Store/CaseStore"
import { useDataFetch } from "@/Hooks"
import { caseTabNames } from "@/Utils/constants"
import { useAppNavigation } from "@/Hooks/useNavigate"

const Wrapper = styled(Grid2)`
    padding: 0 16px 16px;
`
const CaseView = () => {
    const { caseId, tab } = useParams()
    const revisionAndProjectData = useDataFetch()
    const {
        activeTabCase,
        setActiveTabCase,
        caseEdits,
        setCaseEditsBelongingToCurrentCase,
    } = useCaseStore()
    const { navigateToCase, navigateToProject } = useAppNavigation()

    // syncs the active tab with the url
    useEffect(() => {
        if (tab) {
            const tabIndex = caseTabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        } else if (caseId) {
            // If no tab is specified, navigate to default tab using replace
            navigateToCase(caseId, caseTabNames[0], { replace: true })
        }
    }, [tab, caseId])

    // navigates to the project page if the case is not found in the revision
    useEffect(() => {
        if (revisionAndProjectData
            && !revisionAndProjectData?.commonProjectAndRevisionData.cases
                .find((c: Components.Schemas.CaseOverviewDto) => c.caseId === caseId)) {
            navigateToProject()
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        if (caseId) {
            setCaseEditsBelongingToCurrentCase(caseEdits.filter((edit) => edit.caseId === caseId))
        }
    }, [caseId, caseEdits])

    return (
        <Wrapper size={{ xs: 12 }}>
            <div role="tabpanel" hidden={activeTabCase !== 0}>
                <CaseDescriptionTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 1}>
                <CaseProductionProfilesTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 2}>
                <CaseScheduleTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 3}>
                <CaseDrillingScheduleTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 4}>
                <CaseFacilitiesTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 5}>
                <CaseCostTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 6}>
                <CaseCO2Tab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 7}>
                <CaseSummaryTab />
            </div>
        </Wrapper>
    )
}

export default CaseView
