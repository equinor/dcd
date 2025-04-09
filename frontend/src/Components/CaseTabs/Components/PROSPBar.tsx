import styled from "styled-components"

import SharePointFileSelector from "../SharePointFileSelector"

import { Container } from "@/Components/ProjectTabs/TechnicalInputTab/PROSPTab/SharedStyledComponents"
import { useAppStore } from "@/Store/AppStore"

const PROSPBarWrapper = styled(Container)`
    position: fixed;
    width: auto;
    bottom: 0;
    right: 11px;
    background-color:  #d7ebf5;
    z-index: 10;
    height: 95px;
`

export const BottomMargin = styled.div`
    margin-bottom: 100px;
`

interface PROSPBarProps {
    projectId: string;
    caseId: string;
    currentSharePointFileId: string | null
}

const PROSPBar: React.FC<PROSPBarProps> = ({ projectId, caseId, currentSharePointFileId }) => {
    const { sidebarOpen, editMode } = useAppStore()

    if (!editMode) {
        return null
    }

    return (
        <>
            <BottomMargin />
            <PROSPBarWrapper style={sidebarOpen ? { left: 257 } : { left: 73 }}>
                <SharePointFileSelector
                    projectId={projectId}
                    caseId={caseId}
                    currentSharePointFileId={currentSharePointFileId || null}
                />
            </PROSPBarWrapper>
        </>
    )
}

export default PROSPBar
