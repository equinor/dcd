import { Outlet } from "react-router-dom"
import styled from "styled-components"
import Sidebar from "./Sidebar"

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

const Overview: React.FC = () => (
    <Wrapper>
        <Body>
            <Sidebar />
            <MainView>
                <Outlet />
            </MainView>
        </Body>
    </Wrapper>
)

export default Overview
