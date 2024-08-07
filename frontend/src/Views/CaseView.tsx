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
import { tabNames } from "../Utils/constants"
import useDataEdits from "../Hooks/useDataEdits"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CaseView = () => {
    const { caseId, tab } = useParams()
    const { addEdit } = useDataEdits()

    const {
        project,
    } = useProjectContext()

    const {
        activeTabCase,
        setActiveTabCase,
        caseEdits,
        setCaseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const navigate = useNavigate()
    const location = useLocation()
    const queryClient = useQueryClient()
    const projectId = project?.id || null

    useEffect(() => {
        if (tab) {
            const tabIndex = tabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [tab])

    // navigates to the default tab (description) if none is provided in the url
    useEffect(() => {
        if (!tab && caseId) {
            const projectUrl = location.pathname.split("/case")[0]
            navigate(`${projectUrl}/case/${caseId}/${tabNames[0]}`, { replace: true })
        } else if (tab) {
            const tabIndex = tabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [caseId])

    useEffect(() => {
        localStorage.setItem("caseEdits", JSON.stringify(caseEdits))
    }, [caseEdits])

    useEffect(() => {
        if (caseId) {
            setCaseEditsBelongingToCurrentCase(caseEdits.filter((edit) => edit.caseId === caseId))
        }
    }, [caseId, caseEdits])

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
                            <CaseDescriptionTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseProductionProfilesTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseScheduleTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseDrillingScheduleTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseFacilitiesTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseCostTab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseCO2Tab addEdit={addEdit} />
                        </Panel>
                        <Panel>
                            <CaseSummaryTab addEdit={addEdit} />
                        </Panel>
                    </Panels>
                </Tabs>
            </Grid>
        </Grid>
    )
}

export default CaseView
