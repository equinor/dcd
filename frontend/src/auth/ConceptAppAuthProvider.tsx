/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-constructed-context-values */
/* eslint-disable max-len */
import { createContext, useState } from "react"

interface ConceptAppAuthContextState {
    userError?: Error;
    loading: boolean;
}

interface AuthHandlerProps {
    children: JSX.Element;
}

const ConceptAppAuthContext = createContext<ConceptAppAuthContextState | undefined>(undefined)
ConceptAppAuthContext.displayName = "ConceptAppAuthContext"

/**
 * A "wrapper" around MSAL authentication as a component, using hooks to actually perform (silent) token acquisition
 * for the application/client.
 */
const AuthHandler: React.FC<AuthHandlerProps> = ({ children }: AuthHandlerProps) => {
    const [userError] = useState<Error>()
    const [isLoading] = useState(true)

    return <ConceptAppAuthContext.Provider value={{ userError, loading: isLoading }}>{children}</ConceptAppAuthContext.Provider>
}

/**
 * A "wrapper" around the @see MsalProvider, so that we can keep track of the @see tokenProvider we are using.
 * Also abstracts the MSAL layer away from the rest of the application.
 */
const ConceptAppAuthProvider: React.FC<{ children: JSX.Element }> = (props) => <AuthHandler {...props} />

export default ConceptAppAuthProvider
