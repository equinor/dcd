/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/no-unused-prop-types */
/* eslint-disable camelcase */
import {
    MouseEventHandler, useState,
    Dispatch,
    SetStateAction,
} from "react"
import styled from "styled-components"

import {
    Button, Switch, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../models/Project"
import CaseArtificialLift from "../Components/CaseArtificialLift"
import CaseDescription from "../Components/CaseDescription"
import CaseDGDate from "../Components/CaseDGDate"
import ExcelUpload from "../Components/ExcelUpload"
import ProductionStrategyOverview from "../Components/ProductionStrategyOverview"
import DGEnum from "../models/DGEnum"
import { Case } from "../models/Case"
import NumberInput from "../Components/NumberInput"

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

const DescriptionDiv = styled.div`
    width: 42.875rem;
    display: flex;
    flex-wrap: wrap;
    @media screen and (max-width: 1390px) {
    margin-right: 1.875rem;
  }
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    artificialLift: Components.Schemas.ArtificialLift,
    setArtificialLift: Dispatch<SetStateAction<Components.Schemas.ArtificialLift>>,
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview,
    setProductionStrategyOverview: Dispatch<SetStateAction<Components.Schemas.ProductionStrategyOverview>>,
    switchReference: MouseEventHandler<HTMLInputElement>,
    isReferenceCase: boolean | undefined,
    facilitiesAvailability: number | undefined,
    setFacilitiesAvailability: Dispatch<SetStateAction<number | undefined>>
}

function DefinitionView({
    project,
    setProject,
    caseItem,
    setCase,
    artificialLift,
    setArtificialLift,
    productionStrategyOverview,
    setProductionStrategyOverview,
    switchReference,
    isReferenceCase,
    facilitiesAvailability,
    setFacilitiesAvailability,
}: Props) {
    return (
        <Wrapper>
            <RowWrapper>
                <Typography variant="h1">Description</Typography>
                <StyledButton
                    onClick={(e) => console.log("Clicked Edit button")}
                >
                    Edit
                </StyledButton>
            </RowWrapper>

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
                        dGType={DGEnum.DG3}
                        dGName="DG3"
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
            </RowWrapper>

            <Switch onClick={switchReference} label="Reference case" readOnly checked={isReferenceCase ?? false} />
            <ExcelUpload setProject={setProject} setCase={setCase} />
        </Wrapper>
    )
}

export default DefinitionView
