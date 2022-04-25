import {
    Tabs,
} from "@equinor/eds-core-react"
import {
    useEffect,
    useState,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetProjectService } from "../Services/ProjectService"
import CaseAsset from "../Components/CaseAsset"
import CaseDescription from "../Components/CaseDescription"
import CaseName from "../Components/CaseName"
import CaseDG4Date from "../Components/CaseDG4Date"
import CaseArtificialLift from "../Components/CaseArtificialLift"

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift>(0)

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    useEffect(() => {
        if (project !== undefined) {
            const caseResult = project.cases.find((o) => o.id === params.caseId)
            if (caseResult !== undefined) {
                setArtificialLift(caseResult.artificialLift)
            }
            setCase(caseResult)
        }
    }, [project])

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseName
                caseItem={caseItem}
                setProject={setProject}
                setCase={setCase}
            />
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <CaseDescription
                    caseItem={caseItem}
                    setProject={setProject}
                    setCase={setCase}
                />
                <CaseDG4Date
                    caseItem={caseItem}
                    setProject={setProject}
                    setCase={setCase}
                />
                <CaseArtificialLift
                    currentValue={artificialLift}
                    setArtificialLift={setArtificialLift}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <CaseAsset
                    caseItem={caseItem}
                    project={project}
                    setProject={setProject}
                    setCase={setCase}
                    caseId={params.caseId}
                />
            </Tabs>
        </CaseViewDiv>
    )
}

export default CaseView
