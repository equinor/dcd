import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { 
    Container, 
    Accordion, 
    AccordionSummary, 
    AccordionDetails,
    Grid,
    Box,
    Divider
} from "@mui/material"
import ExpandMoreIcon from "@mui/icons-material/ExpandMore"
import AccountTreeIcon from "@mui/icons-material/AccountTree"
import TimelineIcon from "@mui/icons-material/Timeline"
import BarChartIcon from "@mui/icons-material/BarChart"
import CompareIcon from "@mui/icons-material/Compare"
import HistoryIcon from "@mui/icons-material/History"
import { Version, Category, UpdateEntry, whatsNewUpdates } from "../Components/Modal/WhatsNewModal"

const StyledContainer = styled(Container)`
    padding-top: 40px;
    padding-bottom: 40px;
`;

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
`;

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
`;

const StyledAccordionSummary = styled(AccordionSummary)`
    &.Mui-expanded {
        min-height: 48px;
        background-color: rgba(224, 247, 250, 0.8);
    }
`;

const StyledAccordionDetails = styled(AccordionDetails)`
    padding: 24px;
`;

const StyledTitle = styled(Typography)`
    text-align: center;
    margin-top: 90px;
    font-weight: bold;
    color: #007079;
`;

const StyledSubtitle = styled(Typography)`
    text-align: center;
    max-width: 800px;
    margin-top: 16px;
    color: #465563;
`;

const VersionList = styled.ul`
    margin: 0;
    padding-left: 20px;
    list-style-type: disc;

    li {
        margin-bottom: 8px;
    }
`;

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
`;

const Feature = ({ icon, title, description }: { icon: React.ReactNode, title: string, description: string }) => (
    <Box mb={4}>
        <FeatureIcon>
            {icon}
            <Typography variant="h4" style={{ color: '#007079' }}>
                {title}
            </Typography>
        </FeatureIcon>
        <Typography variant="body_long">
            {description}
        </Typography>
    </Box>
);

const IndexView: React.FC = () => {
    const versions = Object.keys(whatsNewUpdates).sort((a, b) => {
        const [aMajor, aMinor, aPatch] = a.split(".").map(Number)
        const [bMajor, bMinor, bPatch] = b.split(".").map(Number)
        if (aMajor !== bMajor) return bMajor - aMajor
        if (aMinor !== bMinor) return bMinor - aMinor
        return bPatch - aPatch
    }) as Version[];

    return (
        <StyledContainer maxWidth="lg">
            <Box display="flex" flexDirection="column" alignItems="center" mb={8}>
                <StyledTitle variant="h1">
                    Digital Concept Development
                </StyledTitle>
                <StyledSubtitle variant="h5">
                    A comprehensive platform for evaluating and comparing offshore field development concepts
                </StyledSubtitle>
            </Box>

            <Grid container spacing={6}>
                    <Grid item xs={12} md={6}>
                        <Feature 
                            icon={<TimelineIcon />}
                            title="Cost Analysis"
                            description="Manage and analyze offshore facility costs, exploration expenses, and drilling operations with detailed time-series profiling."
                        />
                        <Feature 
                            icon={<BarChartIcon />}
                            title="Production Profiles"
                            description="Create and monitor drainage strategies with comprehensive production profiles and environmental impact assessments."
                        />
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <Feature 
                            icon={<CompareIcon />}
                            title="Business Case Comparison"
                            description="Compare different concepts with automated NPV calculations and break-even analysis to make informed decisions."
                        />
                        <Feature 
                            icon={<AccountTreeIcon />}
                            title="Project Integration"
                            description="Seamlessly export your data to STEA and integrate with other Equinor systems for comprehensive project management."
                        />
                    </Grid>
                </Grid>

            <Divider sx={{ my: 8 }} />

            <Box>
                <SectionHeader>
                    <HistoryIcon />
                    <Typography variant="h4" style={{ color: '#007079' }}>
                        Version History
                    </Typography>
                </SectionHeader>
                {versions.map((version) => (
                    <StyledAccordion key={version}>
                        <StyledAccordionSummary expandIcon={<ExpandMoreIcon />}>
                            <Typography variant="h4">Version {version}</Typography>
                        </StyledAccordionSummary>
                        <StyledAccordionDetails>
                            <Grid container spacing={2}>
                                {Object.entries(whatsNewUpdates[version]).map(([category, updates]) => (
                                    <Grid item xs={12} key={category}>
                                        <Typography variant="h5" style={{ color: '#007079', marginBottom: '8px', fontWeight: 500 }}>
                                            {category}
                                        </Typography>
                                        <VersionList>
                                            {updates?.map((entry: UpdateEntry, index: number) => (
                                                <li key={index}>
                                                    <Typography variant="body_long">
                                                        {entry.description}
                                                    </Typography>
                                                </li>
                                            ))}
                                        </VersionList>
                                    </Grid>
                                ))}
                            </Grid>
                        </StyledAccordionDetails>
                    </StyledAccordion>
                ))}
            </Box>
        </StyledContainer>
    );
}

export default IndexView
