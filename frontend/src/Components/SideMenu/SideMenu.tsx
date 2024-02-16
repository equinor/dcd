import { Outlet } from "react-router-dom"
import styled from "styled-components"
import ProjectMenu from "./ProjectMenu"

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

const SideMenu: React.FC = () => (
    <Wrapper>
        <Body>
            <SidebarDiv>
                <ProjectMenu />
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
                <Outlet />
            </MainView>
        </Body>
    </Wrapper>
)

export default SideMenu
