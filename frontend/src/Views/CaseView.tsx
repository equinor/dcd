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
import CaseDGDate from "../Components/CaseDGDate"
import CaseArtificialLift from "../Components/CaseArtificialLift"
import DGEnum from "../models/DGEnum"
import NumberInput from "../Components/NumberInput"
import { GetCaseService } from "../Services/CaseService"

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    > *:not(:last-child) {
        margin-right: 1rem;
    }
    flex-direction: row;
`

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift>(0)
    const [producerCount, setProducerCount] = useState<number>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>()
    const [rigMobDemob, setRigMobDemob] = useState<number>()
    const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()

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
                setFacilitiesAvailability(caseResult?.facilitiesAvailability)
            }
            setCase(caseResult)
            setProducerCount(caseResult?.producerCount)
            setGasInjectorCount(caseResult?.gasInjectorCount)
            setWaterInjectorCount(caseResult?.waterInjectorCount)
            setRigMobDemob(caseResult?.rigMobDemob)
            setFacilitiesAvailability(caseResult?.facilitiesAvailability)
        }
    }, [project])

    useEffect(() => {
        (async () => {
            if (caseItem) {
                const caseDto = Case.Copy(caseItem)
                caseDto.producerCount = producerCount
                caseDto.gasInjectorCount = gasInjectorCount
                caseDto.waterInjectorCount = waterInjectorCount
                caseDto.rigMobDemob = rigMobDemob
                caseDto.facilitiesAvailability = facilitiesAvailability

                const newProject = await GetCaseService().updateCase(caseDto)
                setCase(newProject.cases.find((o) => o.id === caseItem.id))
            }
        })()
    }, [producerCount, gasInjectorCount, waterInjectorCount, rigMobDemob, facilitiesAvailability])

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
                <Wrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG0}
                        dGName="DG0"
                    />
                </Wrapper>
                <Wrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG1}
                        dGName="DG1"
                    />
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG3}
                        dGName="DG3"
                    />
                </Wrapper>
                <Wrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG2}
                        dGName="DG2"
                    />
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG4}
                        dGName="DG4"
                    />
                </Wrapper>
                <CaseArtificialLift
                    currentValue={artificialLift}
                    setArtificialLift={setArtificialLift}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <Wrapper>
                    <NumberInput
                        setValue={setProducerCount}
                        value={producerCount ?? 0}
                        integer
                        disabled={false}
                        label="Producer count"
                    />
                    <NumberInput
                        setValue={setGasInjectorCount}
                        value={gasInjectorCount ?? 0}
                        integer
                        disabled={false}
                        label="Gas injector count"
                    />
                    <NumberInput
                        setValue={setWaterInjectorCount}
                        value={waterInjectorCount ?? 0}
                        integer
                        disabled={false}
                        label="Water injector count"
                    />
                    <NumberInput
                        setValue={setRigMobDemob}
                        value={rigMobDemob ?? 0}
                        integer={false}
                        disabled={false}
                        label="Rig mob demob"
                    />
                    <NumberInput
                        setValue={setFacilitiesAvailability}
                        value={facilitiesAvailability ?? 0}
                        integer
                        disabled={false}
                        label="Facilities Availability"
                    />
                </Wrapper>
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
