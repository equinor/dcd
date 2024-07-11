import React, { useState, useEffect } from "react"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { Typography } from "@equinor/eds-core-react"
import { useQuery } from "react-query"
import Classification from "./Classification"
import { GetProjectService } from "../../Services/ProjectService"
import { useProjectContext } from "../../Context/ProjectContext"

const Wrapper = styled.div`
    background-color: white;
    padding: 20px;
    display: flex;
    flex-direction: row;
    gap: 20px;
    align-items: center;
    justify-content: space-between;
`

const ProjectControls = () => {
    const { currentContext } = useModuleCurrentContext()
    const [modifyTime, setModifyTime] = useState<string | undefined>(undefined)
    const {
        project,
    } = useProjectContext()
    const projectId: string | null = project?.id || null

    const { data: projectData } = useQuery(
        ["projectData", projectId],
        async () => {
            if (!projectId) { return null }
            const service = await GetProjectService()
            return service.getProject(projectId)
        },
        {
            enabled: !!projectId,
            initialData: project,
        },
    )

    useEffect(() => {
        if (projectData && projectData.cases && projectData.cases.length > 0) {
            const latestModifyTime = projectData.cases.reduce((latest, currentCase) => (currentCase.modifyTime > latest ? currentCase.modifyTime : latest), projectData.cases[0].modifyTime)
            setModifyTime(latestModifyTime)
        }
    }, [projectData])

    const formatDate = (dateString: string | undefined) => {
        if (!dateString) {
            return ""
        }
        const date = new Date(dateString)
        const options: Intl.DateTimeFormatOptions = {
            day: "2-digit",
            month: "short",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        }
        return new Intl.DateTimeFormat("en-GB", options).format(date).replace(",", "")
    }

    return (
        <Wrapper>
            <Typography variant="h4">
                {currentContext?.title}
            </Typography>
            <Classification />
            <Typography variant="caption">
                Last updated:
                {" "}
                {formatDate(modifyTime)}
            </Typography>
        </Wrapper>
    )
}

export default ProjectControls
