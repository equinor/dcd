import { FC } from "react"
import { AppRouter } from "./app/AppRouter"
import { AppContextProvider } from "./context/AppContext"

const Root: FC = () => {
    console.log("Root")

    return (
        <AppContextProvider>
            <AppRouter />
        </AppContextProvider>
    )
}

export default Root
