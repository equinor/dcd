import { Tabs, Tab, Box } from "@mui/material"
import { useParams } from "react-router-dom"
import { caseTabNames } from "@/Utils/Config/constants"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useCaseStore } from "@/Store/CaseStore"
import { useWindowSize } from "@/Hooks/useWindowSize"

interface CaseTabsProps {
    caseId: string
}

const CaseTabs = ({ caseId }: CaseTabsProps) => {
    const { revisionId } = useParams()
    const { navigateToCaseTab } = useAppNavigation()
    const { activeTabCase } = useCaseStore()
    const { width } = useWindowSize()

    const handleTabChange = (_: any, index: number) => {
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
