import { Container} from "@/Components/ProjectTabs/TechnicalInputTab/PROSPTab/SharedStyledComponents";
import SharePointFileSelector from "../SharePointFileSelector";
import styled from "styled-components";
import { useAppStore } from "@/Store/AppStore";


const PROSPBarWrapper = styled(Container)`
    position: fixed;
    width: auto;
    bottom: 0;
    right: 11px;
    background-color:  #d7ebf5;
    z-index: 10;
    height: 115px;
`;

export const BottomMargin = styled.div`
    margin-bottom: 165px;
`

interface PROSPBarProps {
    projectId: any;
    caseId: any;
    currentSharePointFileId: any;
}

const PROSPBar: React.FC<PROSPBarProps> = ({projectId, caseId, currentSharePointFileId}) => {
    const { sidebarOpen, editMode } = useAppStore()

    if(!editMode) {
        return null; 
    }

    return <>
        <BottomMargin />
        <PROSPBarWrapper style={sidebarOpen ? {left: 257} : {left: 73}}>
        <SharePointFileSelector
            projectId={projectId}
            caseId={caseId}
            currentSharePointFileId={currentSharePointFileId || null}
        />
    </PROSPBarWrapper></>

}

export default PROSPBar;