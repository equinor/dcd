import React from "react"
import styled from "styled-components"
import { Box } from "@mui/material"
import { Icon, Typography } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import ExternalAccessInfo from "@/Components/Project/Components/ExternalAccessInfo"
import { NoAccessReason } from "@/enums"

const Container = styled.div`
  max-width: 800px;
  margin: 90px auto 0 auto;
  padding: 20px;
`

const Header = styled(Box)`
  display: flex;
  align-items: center;
  gap: 18px;
  padding: 20px 0 16px 0;
`

const InfoContainer = styled(Box)`
    margin-top: -20px;
`

interface Props {
  projectClassification: NoAccessReason.ClassificationRestricted |
      NoAccessReason.ClassificationConfidential |
      NoAccessReason.ClassificationInternal
}

const ACCESS_MESSAGES = {
    [NoAccessReason.ClassificationRestricted]: `This project is classified as 'restricted'.
    In order to access projects with this classification, you need to be a project member.
    You can request to join this project by contacting the project's valuation lead / concept architect.`,
    [NoAccessReason.ClassificationConfidential]: `This project is classified as 'confidential'.
    In order to access projects with this classification, you need to be a project member.
    You can request to join this project by contacting the project's valuation lead / concept architect.`,
    [NoAccessReason.ClassificationInternal]: `This project is classified as 'internal', and you can access it two ways:
    Either by joining an AccessIT group or by joining the project as a project member.`,
} as const

const NoAccessErrorView: React.FC<Props> = ({ projectClassification }) => {
    const message = ACCESS_MESSAGES[projectClassification]

    return (
        <Container>
            <Header>
                <Icon data={info_circle} size={24} color="#007079" />
                <Typography variant="h2">
                    You do not have access to this project
                </Typography>
            </Header>
            <Typography>
                {message}
            </Typography>
            <InfoContainer>
                <ExternalAccessInfo />
            </InfoContainer>
        </Container>
    )
}

export default NoAccessErrorView
