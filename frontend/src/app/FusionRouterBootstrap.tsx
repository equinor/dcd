import { useFusionEnvironment, HistoryContext } from "@equinor/fusion"
import { createBrowserHistory } from "history"
import { useMemo } from "react"
import { Router } from "react-router-dom"

interface Props {
    children: any;
}

const appKey = "conceptapp"
const fusionPrefix = `/apps/${appKey}`

export const FusionRouterBootstrap = ({ children }: Props): JSX.Element => {
    const { env } = useFusionEnvironment()
    const history = useMemo(() => {
        const basename = env ? fusionPrefix : "/"
        return createBrowserHistory({ basename })
    }, [env])

    return (
        // eslint-disable-next-line react/jsx-no-constructed-context-values
        <HistoryContext.Provider value={{ history }}>
            <Router key={appKey} history={history}>
                {children}
            </Router>
        </HistoryContext.Provider>
    )
}
