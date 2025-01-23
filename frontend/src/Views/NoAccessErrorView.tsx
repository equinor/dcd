import React from "react"
import styled from "styled-components"
import { Box } from "@mui/material"
import { Icon, Typography } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import ExternalAccessInfo from "@/Components/Project/Components/ExternalAccessInfo"
import { NoAccessReason } from "@/Models/Interfaces"

const Container = styled.div`
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
`

const Header = styled(Box)`
  display: flex;
  align-items: center;
  gap: 18px;
  padding: 24px 0 16px 0;
`

interface Props {
  projectClassification: number
}

const ForbiddenAccess: React.FC<Props> = ({ projectClassification }) => {
    const restrictedOrConfidentialExplanation = `This project is classified as 'restricted' or 'confidential'.
        In order to access projects with these classifications, you need to be a project member.
        You can request to join this project by contacting the project's valuation lead / concept architect.`

    const internalExplanation = `This project is classified as 'internal', and you can access it two ways:
        Either by joining an AccessIT group or by joining the project as a project member.`

    return (
        <Container>
            <Header>
                <Icon data={info_circle} size={24} color="#007079" />
                <Typography variant="h2">
                    You do not have access to this project
                </Typography>
            </Header>
            <Typography>
                {projectClassification === NoAccessReason.ClassificationRestricted && restrictedOrConfidentialExplanation}
                {projectClassification === NoAccessReason.ClassificationConfidential && restrictedOrConfidentialExplanation}
                {projectClassification === NoAccessReason.ClassificationInternal && internalExplanation}
            </Typography>
            <ExternalAccessInfo />
        </Container>
    )
}

export default ForbiddenAccess
