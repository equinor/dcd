import { Outlet } from "react-router-dom"
import { useEffect, VoidFunctionComponent } from "react"
import styled from "styled-components"

import { useMsal } from "@azure/msal-react"
import Header from "../Components/Header"
import SideMenu from "../Components/SideMenu/SideMenu"

import { fusionApiScope } from "../config"
import { loginRequest } from "../auth/authContextProvider"

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

export const AuthenticatedViewContainer: VoidFunctionComponent = () => {
    const { instance, accounts } = useMsal()

    useEffect(() => {
        if (instance && accounts) {
            (async () => {
                // Silently acquires an access token
                // which is then attached to a request for MS Graph data
                try {
                    const { accessToken } = await instance.acquireTokenSilent({
                        ...loginRequest,
                        account: accounts[0],
                    })
                    window.sessionStorage.setItem("loginAccessToken", accessToken)
                } catch (error) {
                    console.error("[AuthenticatedViewContainer] Login failed", error)
                }

                try {
                    const { accessToken } = await instance.acquireTokenSilent({
                        scopes: fusionApiScope,
                        account: accounts[0],
                    })
                    window.sessionStorage.setItem("fusionAccessToken", accessToken)
                } catch (error) {
                    console.error("[AuthenticatedViewContainer] Failed to get fusion token", error)
                }
            })()
        }
    }, [instance, accounts])

    return (
        <Wrapper>
            <Header name={accounts[0].name} />
            <Body>
                <SideMenu />
                <MainView>
                    <Outlet />
                </MainView>
            </Body>
        </Wrapper>
    )
}
