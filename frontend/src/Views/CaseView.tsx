import {
    Tabs,
    Typography,
    Input,
} from "@equinor/eds-core-react"
import {
    useEffect,
    useState,
    ChangeEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetProjectService } from "../Services/ProjectService"
import DescriptionView from "./DescriptionView"
import { GetCaseService } from "../Services/CaseService"
import CaseAsset from "../Components/CaseAsset"

const {
    Panels, Panel,
} = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const CaseHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const Dg4Field = styled.div`
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [dg4DateRec, setDg4DateRec] = useState<Record<string, any>>({})

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDg4DateRec({
            dg4DateRec,
            [e.target.name]: e.target.value,
        })
        const updateDG4 = {
            id: params.caseId,
            projectId: project?.id,
            name: caseItem?.name,
            description: caseItem?.description,
            dG4Date: e.target.value,
        }
        const newProject = await GetCaseService().updateCase(updateDG4)
        setProject(newProject)
    }

    const dg4ReturnDate = () => {
        const dg4DateGet = caseItem?.DG4Date?.toLocaleDateString("en-CA")
        if (dg4DateGet !== "0001-01-01") {
            return dg4DateGet
        }
        return ""
    }

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader>
                <Typography variant="h2">{caseItem?.name}</Typography>
            </CaseHeader>
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <Panels>
                    <Panel>
                        <DescriptionView />
                    </Panel>
                </Panels>
                <Panels>
                    <Panel>
                        <Typography>DG4</Typography>
                        <Dg4Field>
                            <Input
                                defaultValue={dg4ReturnDate()}
                                key={dg4ReturnDate()}
                                id="dg4Date"
                                type="date"
                                name="dg4Date"
                                onChange={handleDg4FieldChange}
                            />
                        </Dg4Field>
                    </Panel>
                </Panels>
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
