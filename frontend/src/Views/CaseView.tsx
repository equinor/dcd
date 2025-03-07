import { useEffect } from "react"
import { useParams } from "react-router-dom"
import Grid2 from "@mui/material/Grid2"
import styled from "styled-components"
import CaseDrillingScheduleTab from "@/Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseProductionProfilesTab from "@/Components/Case/Tabs/CaseProductionProfilesTab"
import CaseDescriptionTab from "@/Components/Case/Tabs/CaseDescriptionTab"
import CaseFacilitiesTab from "@/Components/Case/Tabs/CaseFacilitiesTab"
import CaseCO2Tab from "@/Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import CaseCostTab from "@/Components/Case/Tabs/CaseCost/CaseCostTab"
import CaseScheduleTab from "@/Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "@/Components/Case/Tabs/CaseSummaryTab"
import { useCaseStore } from "@/Store/CaseStore"
import { useDataFetch } from "@/Hooks"
import { caseTabNames } from "@/Utils/constants"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { EditQueueProvider } from "@/Store/EditQueueContext"

const Wrapper = styled(Grid2)`
    padding: 0 16px 16px;
`

const CaseView = () => {
    const { caseId, tab } = useParams()
    const revisionAndProjectData = useDataFetch()
    const {
        activeTabCase,
        setActiveTabCase,
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

    return (
        <Wrapper size={{ xs: 12 }}>
            <div role="tabpanel" hidden={activeTabCase !== 0}>
                <CaseDescriptionTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 1}>
                <EditQueueProvider>
                    <CaseProductionProfilesTab />
                </EditQueueProvider>
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 2}>
                <CaseScheduleTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 3}>
                <EditQueueProvider>
                    <CaseDrillingScheduleTab />
                </EditQueueProvider>
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 4}>
                <CaseFacilitiesTab />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 5}>
                <EditQueueProvider>
                    <CaseCostTab />
                </EditQueueProvider>
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 6}>
                <EditQueueProvider>
                    <CaseCO2Tab />
                </EditQueueProvider>
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 7}>
                <CaseSummaryTab />
            </div>
        </Wrapper>
    )
}

export default CaseView
