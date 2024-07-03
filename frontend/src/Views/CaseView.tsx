import { Tabs } from "@equinor/eds-core-react"
import { useEffect } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import CaseDescriptionTab from "../Components/Case/Tabs/CaseDescriptionTab"
import CaseCostTab from "../Components/Case/Tabs/CaseCost/CaseCostTab"
import CaseFacilitiesTab from "../Components/Case/Tabs/CaseFacilitiesTab"
import CaseProductionProfilesTab from "../Components/Case/Tabs/CaseProductionProfilesTab"
import CaseScheduleTab from "../Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "../Components/Case/Tabs/CaseSummaryTab"
import CaseDrillingScheduleTab from "../Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseCO2Tab from "../Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"
import CaseDescriptionTabSkeleton from "../Components/Case/Tabs/LoadingSkeletons/CaseDescriptionTabSkeleton"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CaseView = () => {
    const { caseId, tab } = useParams()

    const {
        project,
    } = useProjectContext()

    const {
        activeTabCase,
        setActiveTabCase,
    } = useCaseContext()

    const navigate = useNavigate()
    const location = useLocation()
    const queryClient = useQueryClient()
    const projectId = project?.id || null

    const tabNames = [
        "Description",
        "Production Profiles",
        "Schedule",
        "Drilling Schedule",
        "Facilities",
        "Cost",
        "CO2 Emissions",
        "Summary",
    ]

    useEffect(() => {
        if (tab) {
            const tabIndex = tabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [tab])

    const handleTabChange = (index: number) => {
        const projectUrl = location.pathname.split("/case")[0]
        navigate(`${projectUrl}/case/${caseId}/${tabNames[index]}`)
    }

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    if (!project || !apiData) {
        return <CaseDescriptionTabSkeleton />
    }

    return (
        <Grid container spacing={1} alignSelf="flex-start">
            <Grid item xs={12}>
                <Tabs activeTab={activeTabCase} onChange={handleTabChange} scrollable>
                    <List>
                        {tabNames.map((tabName) => <Tab key={tabName}>{tabName}</Tab>)}
                    </List>
                    <Panels>
                        <Panel>
                            <CaseDescriptionTab />
                        </Panel>
                        <Panel>
                            <CaseProductionProfilesTab />
                        </Panel>
                        <Panel>
                            <CaseScheduleTab />
                        </Panel>
                        <Panel>
                            <CaseDrillingScheduleTab />
                        </Panel>
                        <Panel>
                            <CaseFacilitiesTab />
                        </Panel>
                        <Panel>
                            <CaseCostTab />
                        </Panel>
                        <Panel>
                            <CaseCO2Tab />
                        </Panel>
                        <Panel>
                            <CaseSummaryTab />
                        </Panel>
                    </Panels>
                </Tabs>
            </Grid>
        </Grid>
    )
}

export default CaseView
