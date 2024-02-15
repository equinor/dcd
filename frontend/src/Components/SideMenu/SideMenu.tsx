import { useLocation } from "react-router-dom"
import { useEffect, useState } from "react"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import ProjectMenu from "./ProjectMenu"
import { GetProjectService } from "../../Services/ProjectService"

const SidebarDiv = styled.div`
    width: 15rem;
    display: flex;
    border-right: 1px solid lightgrey;
    display: flex;
    flex-direction: column;
    overflow-y: auto;
    overflow-x: hidden;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
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
    overflow: auto;
    overflow-x: hidden;
`
const SideMenuFooter = styled.div`
    bottom: 10px;
    left: 70px;
    text-align: center;
    position: fixed;
`

interface Props {
    children: JSX.Element
}

const SideMenu: React.FC<Props> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto>()
    const { currentContext } = useModuleCurrentContext()
    const location = useLocation()

    useEffect(() => {
        if (currentContext?.externalId) {
            (async () => {
                try {
                    const fetchedProject = await (await GetProjectService()).getProjectByID(currentContext.externalId!)
                    if (!fetchedProject || fetchedProject.id === "") {
                        // Workaround for retrieving project in sidemenu while project is created
                        // eslint-disable-next-line no-promise-executor-return
                        await new Promise((r) => setTimeout(r, 2000))
                        const secondAttempt = await (await GetProjectService())
                            .getProjectByID(currentContext.externalId!)

                        setProject(secondAttempt)
                    } else {
                        setProject(fetchedProject)
                    }
                } catch (error) {
                    console.error("Error fetching project", error)
                }
            })()
        }
    }, [currentContext?.externalId, location.pathname])

    if (currentContext) {
        return (
            <Wrapper>
                <Body>
                    <SidebarDiv>
                        <ProjectMenu project={project} />
                        <SideMenuFooter>
                            <a
                                href="https://forms.office.com/Pages/ResponsePage.aspx?id=NaKkOuK21UiRlX_PBbRZsCjGTHQnxJxIkcdHZ_YqW4BUMTQyTVNLOEY0VUtSUjIwN1QxUVJIRjBaNC4u"
                                target="_blank"
                                rel="noopener noreferrer"
                            >
                                Send feedback
                            </a>
                        </SideMenuFooter>
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
