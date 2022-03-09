import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsalAuthentication } from "@azure/msal-react"
import { InteractionType } from "@azure/msal-browser"
import { VoidFunctionComponent } from "react"
import { useTranslation } from "react-i18next"

import { AuthenticatedViewContainer } from "./AuthenticatedViewContainer"

export const ViewsContainer: VoidFunctionComponent = () => {
    const { t } = useTranslation()
    useMsalAuthentication(InteractionType.Redirect)

    return (
        <>
            <AuthenticatedTemplate>
                <AuthenticatedViewContainer />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <p>{t("ViewsContainer.PleaseSignIn")}</p>
            </UnauthenticatedTemplate>
        </>
    )
}
