import { useEffect } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import CaseDescriptionTab from "../Components/Case/Tabs/CaseDescriptionTab"
import CaseCostTab from "../Components/Case/Tabs/CaseCost/CaseCostTab"
import CaseFacilitiesTab from "../Components/Case/Tabs/CaseFacilitiesTab"
import CaseProductionProfilesTab from "../Components/Case/Tabs/CaseProductionProfilesTab"
import CaseScheduleTab from "../Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "../Components/Case/Tabs/CaseSummaryTab"
import CaseDrillingScheduleTab from "../Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseCO2Tab from "../Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import { useCaseContext } from "../Context/CaseContext"
import { caseTabNames } from "../Utils/constants"
import useEditCase from "../Hooks/useEditCase"
import { projectQueryFn } from "../Services/QueryFunctions"

const Wrapper = styled(Grid)`
    padding: 0 16px;
`
const CaseView = () => {
    const { caseId, tab } = useParams()
    const { addEdit } = useEditCase()
    const { currentContext } = useModuleCurrentContext()
    const contextId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", contextId],
        queryFn: () => projectQueryFn(contextId),
        enabled: !!contextId,
    })

    const {
        activeTabCase,
        setActiveTabCase,
        caseEdits,
        setCaseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const navigate = useNavigate()
    const location = useLocation()
    const projectUrl = location.pathname.split("/case")[0]

    useEffect(() => {
        if (tab) {
            const tabIndex = caseTabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [tab])

    useEffect(() => {
        if (apiData && !apiData?.cases.find((c: Components.Schemas.CaseDto) => c.id === caseId)) {
            navigate(projectUrl)
        }
    }, [apiData])

    // navigates to the default tab (description) if none is provided in the url
    useEffect(() => {
        if (!tab && caseId) {
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
