/* eslint-disable camelcase */
import React, {
    Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
import Currency from "../Components/Currency"
import PhysicalUnit from "../Components/PhysicalUnit"
import { Project } from "../models/Project"

const Wrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
`

const RowWrapper = styled.div`
    margin: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

const DataDiv = styled.div`
    margin-right: 2rem;
 `

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>
    physicalUnit: Components.Schemas.PhysUnit,
    setPhysicalUnit: Dispatch<SetStateAction<Components.Schemas.PhysUnit>>,
    currency: Components.Schemas.Currency,
    setCurrency: Dispatch<SetStateAction<Components.Schemas.Currency>>
}

function SettingsView({
    project,
    setProject,
    physicalUnit,
    setPhysicalUnit,
    currency,
    setCurrency,

}: Props) {
    return (
        <Wrapper>
            <RowWrapper>
                <DataDiv>
                    <PhysicalUnit
                        currentValue={physicalUnit}
                        setPhysicalUnit={setPhysicalUnit}
                        setProject={setProject}
                        project={project}
                    />
                </DataDiv>
                <DataDiv>
                    <Currency
                        currentValue={currency}
                        setCurrency={setCurrency}
                        setProject={setProject}
                        project={project}
                    />
                </DataDiv>
            </RowWrapper>
        </Wrapper>
    )
}

export default SettingsView
