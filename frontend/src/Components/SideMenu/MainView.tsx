import React from 'react'
import SideMenu from './SideMenu'
import { Outlet } from 'react-router-dom'

const MainView = () => {
    return (
        <>
            <SideMenu />
            <Outlet />
        </>
    )
}

export default MainView
