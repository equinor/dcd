import { Outlet } from "react-router-dom"
import styled from "styled-components"
import Sidebar from "./Sidebar"
import ProjectControls from "./ProjectControls"

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

const Overview: React.FC = () => (
    <Body>
        <Sidebar />
        <MainView>
            <ProjectControls />
            <Outlet />
        </MainView>
    </Body>
)

export default Overview
