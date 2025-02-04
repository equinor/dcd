import React from "react"
import { Typography } from "@mui/material"
import {
    Container,
    Section,
    SectionTitle,
    Formula,
    FormulaSection,
    Note,
    FormulaList,
    SpecialNote,
} from "../../../shared.styles"

interface Props {
    developerMode: boolean
    hasOverride: boolean
}

const CessationOffshoreFacilities: React.FC<Props> = ({ developerMode, hasOverride }) => (
    <Container>
        <Section>
            <SectionTitle variant="h6">Cessation - Offshore Facilities Cost</SectionTitle>
            {hasOverride && (
                <Note>
                    <Typography variant="body2" component="div">
                        <strong>Note:</strong>
                        This profile is currently using manual input values instead of calculated values.
                    </Typography>
                </Note>
            )}
            <FormulaSection>
                <Typography variant="body1" gutterBottom>
                    The cessation cost for offshore facilities is calculated based on the following components:
                </Typography>
                <Formula>
                    Cessation Offshore Facilities Cost = Platform Removal Cost + Marine Operations Cost + Onshore Disposal Cost
                </Formula>
            </FormulaSection>

            <FormulaSection>
                <h4>Cost Components</h4>
                <FormulaList>
                    <li>
                        Platform Removal Cost
                        <ul>
                            <li>Cost of dismantling and removing platform structures</li>
                            <li>Includes engineering and preparation work</li>
                            <li>Heavy lift vessel operations</li>
                        </ul>
                    </li>
                    <li>
                        Marine Operations Cost
                        <ul>
                            <li>Vessel mobilization and demobilization</li>
                            <li>Transportation of removed structures</li>
                            <li>Support vessel operations</li>
                        </ul>
                    </li>
                    <li>
                        Onshore Disposal Cost
                        <ul>
                            <li>Waste management and recycling</li>
                            <li>Site preparation and handling</li>
                            <li>Environmental compliance measures</li>
                        </ul>
                    </li>
                </FormulaList>
            </FormulaSection>

            <Note>
                <Typography variant="body2" component="div">
                    <strong>Important Considerations:</strong>
                    <FormulaList>
                        <li>Costs are typically spread over multiple years based on the decommissioning schedule</li>
                        <li>Environmental regulations and requirements can significantly impact total costs</li>
                        <li>Weather conditions and seasonal restrictions may affect timing and costs</li>
                        <li>Facility size, complexity, and water depth are key cost drivers</li>
                    </FormulaList>
                </Typography>
            </Note>

            {developerMode && (
                <SpecialNote variant="body2">
                    Developer Note: Cessation costs are typically estimated during the planning phase and refined as the project approaches decommissioning.
                    The calculation considers multiple factors including facility size, location, and regulatory requirements.
                </SpecialNote>
            )}
        </Section>
    </Container>
)

export default CessationOffshoreFacilities
