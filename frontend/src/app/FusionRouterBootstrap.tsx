import { useFusionEnvironment, HistoryContext } from "@equinor/fusion"
import { createBrowserHistory } from "history"
import { useMemo } from "react"
import { Router } from "react-router-dom"

interface Props {
    children: React.ReactNode;
}

const appKey = "conceptapp"
const fusionPrefix = `/apps/${appKey}`

/**
 * This component is required for the app to be loaded properly in the Fusion portal.
 *
 * A little gotcha regarding the fusionPrefix. In localhost you interact with the
 * application on the root (/) level. In the CI/QA/PRODUCTION environments, the
 * application is running in the Fusion portal, meaning that the root path to
 * the application is /apps/corporate-project-review. Therefore, we need to
 * consider based on the environment.
 *
 * {@todo}
 * According to Odin (Fusion Core), setting the basename from the environment
 * causes issues with a Fusion tool called App Configurator. Currently, this is
 * not an application we have knowledge about.
 *
 * @export
 * @param {Props} {children}
 * @return {*}  {JSX.Element}
 */
export function FusionRouterBootstrap({ children }: Props): JSX.Element {
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
