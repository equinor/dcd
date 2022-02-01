import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsalAuthentication } from '@azure/msal-react'
import { InteractionType } from '@azure/msal-browser'
import { VoidFunctionComponent } from 'react'

import { AuthenticatedViewContainer } from './AuthenticatedViewContainer'

export const ViewsContainer: VoidFunctionComponent = () => {
    useMsalAuthentication(InteractionType.Redirect)

    return (
        <>
            <AuthenticatedTemplate>
                <AuthenticatedViewContainer />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <p>Please sign in to DCD</p>
            </UnauthenticatedTemplate>
        </>
    )
}
