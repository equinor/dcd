import React from "react"
import { Tabs, Tab } from "@mui/material"
import { useNavigate, useLocation } from "react-router-dom"

import { caseTabNames } from "@/Utils/constants"

type CaseTabsProps = {
    activeTabCase: number
    caseId: string
}

const CaseTabs: React.FC<CaseTabsProps> = ({ activeTabCase, caseId }) => {
    const navigate = useNavigate()
    const location = useLocation()

    const handleTabChange = (index: number) => {
        const projectUrl = location.pathname.split("/case")[0]
        navigate(`${projectUrl}/case/${caseId}/${caseTabNames[index]}`)
    }

    return (
        <Tabs
            value={activeTabCase}
            onChange={(_, index) => handleTabChange(index)}
            variant="scrollable"
        >
            {caseTabNames.map((tabName) => <Tab key={tabName} label={tabName} />)}
        </Tabs>
    )
}

export default CaseTabs
