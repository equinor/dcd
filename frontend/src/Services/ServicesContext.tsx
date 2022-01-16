import React, { FunctionComponent } from 'react'

type ServicesContextValue = {
    accessToken: string,
}

const initState: ServicesContextValue = {
    accessToken: '',
}

export const ServicesContext = React.createContext(initState)

type Props = {
    accessToken: string
}

export const ServicesContextProvider: FunctionComponent<Props> = ({ children, accessToken }) => {
    return (
        <ServicesContext.Provider value={{ ...initState, accessToken }}>
            {children}
        </ServicesContext.Provider>
    )
}
