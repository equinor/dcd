import {
    MouseEventHandler, useState,
    Dispatch,
    SetStateAction,
    useEffect,
} from "react"
import styled from "styled-components"

import {
    Button, Switch, Typography,
} from "@equinor/eds-core-react"
import { useParams } from "react-router"
import { Project } from "../models/Project"
import CaseArtificialLift from "../Components/Case/CaseArtificialLift"
import CaseDescription from "../Components/Case/CaseDescription"
import CaseDGDate from "../Components/Case/CaseDGDate"
import ExcelUpload from "../Components/ExcelUpload"
import ProductionStrategyOverview from "../Components/ProductionStrategyOverview"
import DGEnum from "../models/DGEnum"
import { Case } from "../models/case/Case"
import NumberInput from "../Components/NumberInput"
import { GetCaseService } from "../Services/CaseService"

const Wrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: column;
`

const DGWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-between;
`

const StyledButton = styled(Button)`
    color: white;
    background-color: #007079;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
}

function DefinitionView({
    project,
    setProject,
    caseItem,
    setCase,
}: Props) {
    const { caseId } = useParams<Record<string, string | undefined>>()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift>(0)
    const [producerCount, setProducerCount] = useState<number>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>()
    const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()
    const [productionStrategyOverview,
        setProductionStrategyOverview] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [isReferenceCase, setIsReferenceCase] = useState<boolean | undefined>()

    useEffect(() => {
        if (project !== undefined) {
            const caseResult = project.cases.find((o) => o.id === caseId)
            if (caseResult !== undefined) {
                setArtificialLift(caseResult.artificialLift)
                setProductionStrategyOverview(caseResult.productionStrategyOverview)
                setFacilitiesAvailability(caseResult?.facilitiesAvailability)
                setIsReferenceCase(caseResult?.referenceCase ?? false)
            }
            setCase(caseResult)
            setProducerCount(caseResult?.producerCount)
            setGasInjectorCount(caseResult?.gasInjectorCount)
            setWaterInjectorCount(caseResult?.waterInjectorCount)
            setFacilitiesAvailability(caseResult?.facilitiesAvailability)
        }
    }, [project, caseId])

    useEffect(() => {
        (async () => {
            if (caseItem) {
                const caseDto = Case.Copy(caseItem)
                caseDto.producerCount = producerCount
                caseDto.gasInjectorCount = gasInjectorCount
                caseDto.waterInjectorCount = waterInjectorCount
                caseDto.facilitiesAvailability = facilitiesAvailability
                caseDto.referenceCase = isReferenceCase ?? false

                const newProject = await (await GetCaseService()).updateCase(caseDto)
                setCase(newProject.cases.find((o) => o.id === caseItem.id))
            }
        })()
    }, [producerCount, gasInjectorCount, waterInjectorCount, facilitiesAvailability, isReferenceCase])

    const switchReference: MouseEventHandler<HTMLInputElement> = () => {
        if (!isReferenceCase || isReferenceCase === undefined) {
            setIsReferenceCase(true)
        } else setIsReferenceCase(false)
    }

    return (
        <Wrapper>
            <Typography variant="h6">Description:</Typography>

            <CaseDescription
                caseItem={caseItem}
                setProject={setProject}
                setCase={setCase}
            />

            <RowWrapper>
                <DGWrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG0}
                        dGName="DG0"
                    />
                </DGWrapper>
                <DGWrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG1}
                        dGName="DG1"
                    />
                </DGWrapper>
                <DGWrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG2}
                        dGName="DG2"
                    />
                </DGWrapper>
                <DGWrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG3}
                        dGName="DG3"
                    />
                </DGWrapper>
                <DGWrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG4}
                        dGName="DG4"
                    />
                </DGWrapper>
            </RowWrapper>

            <RowWrapper>
                <ProductionStrategyOverview
                    currentValue={productionStrategyOverview}
                    setProductionStrategyOverview={setProductionStrategyOverview}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <CaseArtificialLift
                    currentValue={artificialLift}
                    setArtificialLift={setArtificialLift}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <NumberInput
                    setValue={setFacilitiesAvailability}
                    value={facilitiesAvailability ?? 0}
                    integer
                    disabled={false}
                    label={`Facilities availability ${project?.physUnit === 0 ? "(%)" : "(Oilfield)"}`}
                />
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
            </RowWrapper>

            <Switch onClick={switchReference} label="Reference case" readOnly checked={isReferenceCase ?? false} />
            <ExcelUpload setProject={setProject} setCase={setCase} />
        </Wrapper>
    )
}

export default DefinitionView
