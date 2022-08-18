/* eslint-disable @typescript-eslint/no-unused-vars */
import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { Case } from "../models/case/Case"
import { Project } from "../models/Project"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    caseItem: Case | undefined
}

const ProductionProfilesTab = ({
    caseItem,
}: Props) => (

    <div color="yellow">
        <TopWrapper color="danger">
            <Typography variant="h4">Production Profiles</Typography>
            <Typography variant="h6">
                Last updated:
                {" "}
                {caseItem?.updatedAt?.toLocaleDateString("en-CA")}
            </Typography>
        </TopWrapper>

    </div>

)

export default ProductionProfilesTab
