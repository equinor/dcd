import { Tabs, Tab, Box } from "@mui/material"
import { useParams } from "react-router-dom"

import { useAppNavigation } from "@/Hooks/useNavigate"
import { useWindowSize } from "@/Hooks/useWindowSize"
import { useCaseStore } from "@/Store/CaseStore"
import { caseTabNames } from "@/Utils/Config/constants"

interface CaseTabsProps {
    caseId: string
}

const CaseTabs = ({ caseId }: CaseTabsProps): React.ReactNode => {
    const { revisionId } = useParams()
    const { navigateToCaseTab } = useAppNavigation()
    const { activeTabCase } = useCaseStore()
    const { width } = useWindowSize()

    const handleTabChange = (_: any, index: number): void => {
        navigateToCaseTab(caseId, caseTabNames[index], revisionId)
    }

    return (
        <Box sx={{
            maxWidth: `${width - 256}px`,
            width: "100%",
        }}
        >
            <Tabs
                value={activeTabCase}
                onChange={handleTabChange}
                variant="scrollable"
                scrollButtons="auto"
            >
                {caseTabNames.map((tabName) => (
                    <Tab key={tabName} label={tabName} />
                ))}
            </Tabs>
        </Box>
    )
}

export default CaseTabs
