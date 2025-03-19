import React from "react"
import styled from "styled-components"
import { Typography, Icon } from "@equinor/eds-core-react"
import {
    Container,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Box,
} from "@mui/material"
import Grid2 from "@mui/material/Grid2"
import {
    chevron_down,
    grid_on,
    trending_up,
    compare,
    history,
    bar_chart,
} from "@equinor/eds-icons"
import { versions, whatsNewUpdates } from "@/Utils/Config/whatsNewData"
import { UpdateEntry } from "@/Models/Interfaces"
import { formatFullDate } from "@/Utils/DateUtils"

const StyledContainer = styled(Container)`
    padding-top: 40px;
    padding-bottom: 40px;
`

const FeatureIcon = styled(Box)`
    display: flex;
    align-items: center;
    margin-bottom: 16px;
    color: #007079;

    svg {
        width: 32px;
        height: 32px;
        margin-right: 16px;
    }
`

const StyledAccordion = styled(Accordion)`
    margin-bottom: 8px;
    background-color: rgba(224, 247, 250, 0.4);
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12);

    &:before {
        display: none;
    }

    &.Mui-expanded {
        margin: 8px 0;
    }
`

const StyledAccordionSummary = styled(AccordionSummary)`
    &.Mui-expanded {
        min-height: 48px;
    }
`

const StyledAccordionDetails = styled(AccordionDetails)`
    padding: 24px;
`

const StyledTitle = styled(Typography)`
    text-align: center;
    margin-top: 40px;
    font-weight: bold;
    color: #007079;
`

const StyledSubtitle = styled(Typography)`
    text-align: center;
    max-width: 800px;
    margin-top: 16px;
    color: #465563;
`

const VersionList = styled.ul`
    margin: 0;
    padding-left: 20px;
    list-style-type: disc;

    li {
        margin-bottom: 8px;
    }
`

const SectionHeader = styled(Box)`
    display: flex;
    align-items: center;
    margin-bottom: 32px;
    color: #007079;

    svg {
        width: 32px;
        height: 32px;
        margin-right: 16px;
    }
`

const StyledVersionHeading = styled(Typography)`
    color: #007079;
    margin-bottom: 8px;
    font-weight: 500;
`

const ContentWrapper = styled(Box)`
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-bottom: 64px;
`

const GridWrapper = styled.div`
    margin-bottom: 96px;
`

const Feature = ({ icon, title, description }: { icon: React.ReactNode, title: string, description: string }) => (
    <Box mb={4}>
        <FeatureIcon>
            {icon}
            <Typography variant="h4" style={{ color: "#007079" }}>
                {title}
            </Typography>
        </FeatureIcon>
        <Typography variant="body_long">
            {description}
        </Typography>
    </Box>
)

const IndexView: React.FC = () => (
    <StyledContainer maxWidth="lg">
        <ContentWrapper>
            <StyledTitle variant="h1">
                Concept App
            </StyledTitle>
            <StyledSubtitle variant="h5">
                A specialized platform for evaluating and maturing offshore field development concepts through detailed cost analysis and production planning
            </StyledSubtitle>
        </ContentWrapper>

        <GridWrapper>
            <Grid2 container spacing={6}>
                <Grid2 size={{ xs: 12, md: 6 }}>
                    <Feature
                        icon={<Icon data={trending_up} />}
                        title="Cost Analysis"
                        description={`Track and analyze costs with time-series profiling for CAPEX and OPEX,
                            including detailed breakdowns for exploration, drilling, offshore facilities, and
                            onshore power supply. Supports both manual input and PROSP integration.`}
                    />
                    <Feature
                        icon={<Icon data={bar_chart} />}
                        title="Production Profiles"
                        description={`Model drainage strategies with detailed production profiles for oil, gas, and water.
                            Monitor environmental impact with CO2 emissions tracking and imported electricity consumption
                            analysis.`}
                    />
                </Grid2>
                <Grid2 size={{ xs: 12, md: 6 }}>
                    <Feature
                        icon={<Icon data={compare} />}
                        title="Business Case Comparison"
                        description={`Evaluate multiple development concepts with NPV calculations, break-even analysis,
                            and cash flow visualization. Create project revisions to track changes and compare different
                            scenarios over time.`}
                    />
                    <Feature
                        icon={<Icon data={grid_on} />}
                        title="Project Integration"
                        description={`Export data to STEA, integrate with SharePoint for PROSP imports, and connect with
                            Equinor's project portfolio through CommonLib. Includes role-based access control and project
                             classification support.`}
                    />
                </Grid2>
            </Grid2>
        </GridWrapper>

        <Box>
            <SectionHeader>
                <Icon data={history} />
                <Typography variant="h4">
                    Release Notes
                </Typography>
            </SectionHeader>
            {versions.map((version) => (
                <StyledAccordion key={version}>
                    <StyledAccordionSummary expandIcon={<Icon data={chevron_down} />}>
                        <Typography variant="h4">
                            Version
                            {" "}
                            {version}
                            <span style={{ color: "#999" }}>
                                {" "}
                                -
                                {formatFullDate(whatsNewUpdates[version].date)}
                            </span>
                        </Typography>
                    </StyledAccordionSummary>
                    <StyledAccordionDetails>
                        <Grid2 container spacing={2}>
                            {Object.entries(whatsNewUpdates[version].updates).map(([category, updates]) => (
                                <Grid2 size={{ xs: 12 }} key={category}>
                                    <StyledVersionHeading variant="h5">
                                        {category}
                                    </StyledVersionHeading>
                                    <VersionList>
                                        {updates?.map((entry: UpdateEntry) => (
                                            <li key={entry.description}>
                                                <Typography variant="body_long">
                                                    {entry.description}
                                                </Typography>
                                            </li>
                                        ))}
                                    </VersionList>
                                </Grid2>
                            ))}
                        </Grid2>
                    </StyledAccordionDetails>
                </StyledAccordion>
            ))}
        </Box>
    </StyledContainer>
)

export default IndexView
