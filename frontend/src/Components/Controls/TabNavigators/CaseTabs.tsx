import { Tabs, Tab } from "@mui/material"
import { useParams } from "react-router-dom"
import { caseTabNames } from "@/Utils/constants"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useCaseContext } from "@/Context/CaseContext"

interface CaseTabsProps {
    caseId: string
}

const CaseTabs = ({ caseId }: CaseTabsProps) => {
    const { revisionId } = useParams()
    const { navigateToCaseTab } = useAppNavigation()
    const { activeTabCase } = useCaseContext()

    const handleTabChange = (_: any, index: number) => {
        navigateToCaseTab(caseId, caseTabNames[index], revisionId)
    }

    return (
        <Tabs
            value={activeTabCase}
            onChange={handleTabChange}
            variant="scrollable"
        >
            {caseTabNames.map((tabName) => <Tab key={tabName} label={tabName} />)}
        </Tabs>
    )
}

export default CaseTabs
