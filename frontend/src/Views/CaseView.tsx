import { useEffect } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import styled from "styled-components"
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
import CaseDescriptionTabSkeleton from "../Components/LoadingSkeletons/CaseDescriptionTabSkeleton"
import { caseTabNames } from "../Utils/constants"
import useDataEdits from "../Hooks/useDataEdits"

const Wrapper = styled(Grid)`
    padding: 0 16px;
`
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
            const tabIndex = caseTabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [tab])

    // navigates to the default tab (description) if none is provided in the url
    useEffect(() => {
        if (!tab && caseId) {
            const projectUrl = location.pathname.split("/case")[0]
            navigate(`${projectUrl}/case/${caseId}/${caseTabNames[0]}`, { replace: true })
        } else if (tab) {
            const tabIndex = caseTabNames.indexOf(tab)
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

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    return (
        <Wrapper item xs={12}>
            <div role="tabpanel" hidden={activeTabCase !== 0}>
                <CaseDescriptionTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 1}>
                <CaseProductionProfilesTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 2}>
                <CaseScheduleTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 3}>
                <CaseDrillingScheduleTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 4}>
                <CaseFacilitiesTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 5}>
                <CaseCostTab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 6}>
                <CaseCO2Tab addEdit={addEdit} />
            </div>
            <div role="tabpanel" hidden={activeTabCase !== 7}>
                <CaseSummaryTab addEdit={addEdit} />
            </div>
        </Wrapper>
    )
}

export default CaseView
