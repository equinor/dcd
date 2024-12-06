import React, { useMemo } from "react"
import { Grid } from "@mui/material"
import { Tooltip } from "@equinor/eds-core-react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import { useQuery } from "@tanstack/react-query"

import {
    productionStrategyOverviewToString, truncateText, caseRevisionPath,
} from "@/Utils/common"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useAppContext } from "@/Context/AppContext"
import { EMPTY_GUID } from "@/Utils/constants"
import { ReferenceCaseIcon } from "../../../Case/Components/ReferenceCaseIcon"
import { TimelineElement } from "../Sidebar"
import { useProjectContext } from "@/Context/ProjectContext"

const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

const CasesList: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { isRevision, projectId } = useProjectContext()
    const { revisionId } = useParams()

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    const location = useLocation()
    const navigate = useNavigate()

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        navigate(caseRevisionPath(currentContext.id, caseId, isRevision, revisionId))
        return null
    }

    const cases = useMemo(
        () => {
            const filteredCases = isRevision ? apiRevisionData?.commonProjectAndRevisionData.cases : apiData?.commonProjectAndRevisionData.cases
            return filteredCases ? filteredCases.filter((c) => !c.archived) : []
        },
        [apiData, apiRevisionData, isRevision],
    )

    if (
        (!apiData && !isRevision)
        || (!apiRevisionData && isRevision)
        || !currentContext
    ) {
        return null
    }

    return (
        <>
            {cases.sort((a, b) => new Date(a.createTime).getDate() - new Date(b.createTime).getDate()).map((projectCase, index) => (
                <Grid
                    item
                    container
                    justifyContent="space-between"
                    key={`menu - item - ${index + 1} `}
                    data-timeline-active={location.pathname.includes(projectCase.id)}
                >
                    <Tooltip
                        title={`${projectCase.name ? truncateText(projectCase.name, 120) : "Untitled"} - Strategy: ${productionStrategyOverviewToString(projectCase.productionStrategyOverview)}`}
                        placement="right"
                    >
                        <TimelineElement variant="ghost" className="GhostButton" onClick={() => selectCase(projectCase.id)}>
                            {apiData?.commonProjectAndRevisionData.referenceCaseId !== EMPTY_GUID
                                ? (
                                    <SideBarRefCaseWrapper>

                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                        {apiData?.commonProjectAndRevisionData.referenceCaseId === projectCase?.id && (
                                            <ReferenceCaseIcon iconPlacement="sideBar" />
                                        )}
                                    </SideBarRefCaseWrapper>
                                )
                                : (
                                    <>
                                        {!sidebarOpen && `#${index + 1}`}
                                        {(sidebarOpen && projectCase.name) && truncateText(projectCase.name, 30)}
                                        {(sidebarOpen && (projectCase.name === "" || projectCase.name === undefined)) && "Untitled"}
                                    </>
                                )}
                        </TimelineElement>
                    </Tooltip>
                </Grid>
            ))}
        </>
    )
}

export default CasesList
