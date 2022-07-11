/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/no-unused-prop-types */
/* eslint-disable camelcase */
import React, {
    MouseEventHandler, useState,
    ChangeEventHandler,
    Dispatch,
    SetStateAction,
} from "react"
import styled from "styled-components"

import {
    Button, Icon, Switch, Tabs, TextField, Typography,
} from "@equinor/eds-core-react"
import {
    add, archive, edit, save,
} from "@equinor/eds-icons"
import { useNavigate } from "react-router-dom"
import { Project } from "../models/Project"
import { GetProjectPhaseName, GetProjectCategoryName, unwrapProjectId } from "../Utils/common"
import { WrapperColumn, WrapperRow } from "./Asset/StyledAssetComponents"
import { GetProjectService } from "../Services/ProjectService"
import { GetSTEAService } from "../Services/STEAService"
import { Modal } from "../Components/Modal"
import { GetCaseService } from "../Services/CaseService"
import CasesTableView from "./CasesTableView"
import CaseArtificialLift from "../Components/CaseArtificialLift"
import CaseDescription from "../Components/CaseDescription"
import CaseDGDate from "../Components/CaseDGDate"
import CaseName from "../Components/CaseName"
import ExcelUpload from "../Components/ExcelUpload"
import ProductionStrategyOverview from "../Components/ProductionStrategyOverview"
import DGEnum from "../models/DGEnum"
import { Case } from "../models/Case"
import { ArtificialLift } from "../models/ArtificialLift"
import NumberInput from "../Components/NumberInput"

const Wrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: column;
`

const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const StyledButton = styled(Button)`
    color: white;
    background-color: #007079;
`

const DataDiv = styled.div`

 `

const DescriptionDiv = styled.div`
    width: 42.875rem;
    display: flex;
    flex-wrap: wrap;
    @media screen and (max-width: 1390px) {
    margin-right: 1.875rem;
  }
`

const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ProjectDataFieldLabel = styled(Typography)`
    margin-right: 0.5rem;
    font-weight: bold;
    white-space: pre-wrap;
`

const CreateCaseForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
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
    const [isEditingMode, setIsEditingMode] = useState<boolean>(false)
    return (
        <Wrapper>
            <RowWrapper>
                <Typography variant="h1">Description</Typography>

                <StyledButton
                    onClick={() => setIsEditingMode(!isEditingMode)}
                >
                    {isEditingMode
                        ? (
                            <Icon data={save} />
                        )
                        : (
                            <Icon data={edit} />
                        ) }
                    {isEditingMode
                        ? (
                            <Typography color="white">Edit</Typography>
                        )
                        : (
                            <Typography color="white">Save</Typography>
                        ) }
                </StyledButton>
            </RowWrapper>

            {isEditingMode
                ? (
                    <Typography>Description Editing Mode</Typography>
                )
                : (
                    <CaseDescription
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                    />
                )}

            <RowWrapper>
                <Wrapper>
                    {isEditingMode
                        ? (
                            <Typography>DG 0 </Typography>
                        )
                        : (
                            <CaseDGDate
                                caseItem={caseItem}
                                setProject={setProject}
                                setCase={setCase}
                                dGType={DGEnum.DG0}
                                dGName="DG0"
                            />
                        )}

                </Wrapper>
                <Wrapper>
                    {isEditingMode
                        ? (
                            <Typography>DG 1 </Typography>
                        )
                        : (
                            <CaseDGDate
                                caseItem={caseItem}
                                setProject={setProject}
                                setCase={setCase}
                                dGType={DGEnum.DG1}
                                dGName="DG1"
                            />
                        )}

                </Wrapper>
                <Wrapper>
                    {isEditingMode
                        ? (
                            <Typography>DG 2 </Typography>
                        )
                        : (
                            <CaseDGDate
                                caseItem={caseItem}
                                setProject={setProject}
                                setCase={setCase}
                                dGType={DGEnum.DG2}
                                dGName="DG2"
                            />
                        )}

                </Wrapper>
                <Wrapper>
                    {isEditingMode
                        ? (
                            <Typography>DG 3 </Typography>
                        )
                        : (
                            <CaseDGDate
                                caseItem={caseItem}
                                setProject={setProject}
                                setCase={setCase}
                                dGType={DGEnum.DG3}
                                dGName="DG3"
                            />
                        )}

                </Wrapper>

                <Wrapper>
                    {isEditingMode
                        ? (
                            <Typography>DG 4 </Typography>
                        )
                        : (
                            <CaseDGDate
                                caseItem={caseItem}
                                setProject={setProject}
                                setCase={setCase}
                                dGType={DGEnum.DG4}
                                dGName="DG4"
                            />
                        )}

                </Wrapper>

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
