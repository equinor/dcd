import { Typography } from "@material-ui/core"
import React, {
    Dispatch, MouseEventHandler, SetStateAction, useEffect,
} from "react"
import styled from "styled-components"
import {
    Button, Input, Label,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import PROSPCaseList from "./PROSPCaseList"
import { Case } from "../../models/case/Case"
import { unwrapCase } from "../../Utils/common"
import { GetCaseService } from "../../Services/CaseService"

const ProspFieldWrapper = styled.div`
    margin-bottom: 2.5rem;
    width: 24rem;
    display: flex;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
}

function PROSPTab({
    project,
    setProject,
}: Props) {
    const saveUri: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        // try {
        //
        // } catch (error) {
        //     console.error("[CaseView] error while submitting form data", error)
        // }
    }
    return (
        <div color="yellow">
            <TopWrapper color="danger">
                <Typography variant="h4">PROSP</Typography>
            </TopWrapper>
            <Label htmlFor="textfield-normal" label="Sharepoint Site addresse" />
            <ProspFieldWrapper>

                <Input id="textfield-normal" placeholder="Paste Uri here" />
                <Button variant="outlined" onClick={saveUri}>Refresh</Button>
            </ProspFieldWrapper>

            <PROSPCaseList project={project} setProject={setProject} />

        </div>
    )
}

export default PROSPTab
