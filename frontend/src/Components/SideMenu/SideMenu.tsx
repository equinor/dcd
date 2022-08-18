import { useLocation, useParams } from "react-router-dom"

import { useEffect, useState } from "react"
import styled from "styled-components"

import ProjectMenu from "./ProjectMenu"
import { Project } from "../../models/Project"
import { GetProjectService } from "../../Services/ProjectService"
import { unwrapProjectId } from "../../Utils/common"

const SidebarDiv = styled.div`
    width: 15rem;
    display: flex;
    border-right: 1px solid lightgrey;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    height: 100vh;
    width: 100vw;
`

const Body = styled.div`
    display: flex;
    flex-direction: row;
    flex-row: 1;
    width: 100%;
    height: 100%;
`

const MainView = styled.div`
    width: calc(100% - 15rem);
    overflow: scroll;
`

interface Props {
    children: JSX.Element;
}

const SideMenu: React.FC<Props> = ({ children }) => {
    const [project, setProject] = useState<Project>()
    const { fusionProjectId } = useParams<Record<string, string | undefined>>()
    const location = useLocation()

    useEffect(() => {
        if (fusionProjectId) {
            (async () => {
                try {
                    const projectId = unwrapProjectId(fusionProjectId)
                    const fetchedProject = await (await GetProjectService()).getProjectByID(projectId)
                    setProject(fetchedProject)
                } catch (error) {
                    console.error()
                }
            })()
        }
    }, [location.pathname])

    if (project) {
        return (
            <Wrapper>
                <Body>
                    <SidebarDiv>
                        <ProjectMenu project={project} />
                    </SidebarDiv>
                    <MainView>
                        {children}
                    </MainView>
                </Body>
            </Wrapper>
        )
    }
    return null
}

export default SideMenu
