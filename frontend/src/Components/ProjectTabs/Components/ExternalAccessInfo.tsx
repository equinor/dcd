import { Icon, Typography } from "@equinor/eds-core-react"
import {
    chevron_down, chevron_up, edit, external_link, visibility,
} from "@equinor/eds-icons"
import Grid from "@mui/material/Grid2"
import { useState } from "react"

import {
    ExternalAccessHeader,
    AccessGroupContainer,
    ExternalLinkIcon,
    AccessDescription,
    AccessLinkContainer,
    OptionsContainer,
} from "./AccessManagement.styles"

const ExternalAccessInfo = () => {
    const [expandAllAccess, setExpandAllAccess] = useState<boolean>(true)

    return (
        <>
            <ExternalAccessHeader onClick={() => setExpandAllAccess(!expandAllAccess)}>
                <Typography variant="h4">Would you like to access all internal Concept App projects?</Typography>
                <Icon data={expandAllAccess ? chevron_up : chevron_down} />
            </ExternalAccessHeader>
            {expandAllAccess
                && (
                    <Grid container>
                        <AccessDescription variant="body_short">
                            In order to access all internal projects in Concept App, you need to apply for access in one of the AccessIT groups listed below.
                            Keep in mind that &quot;Restricted&quot; or &quot;Confidential&quot; projects are only accesible to project members.
                        </AccessDescription>
                        <OptionsContainer container justifyContent="space-around">
                            <Grid size={{ xs: 12, md: 3 }}>
                                <AccessGroupContainer>
                                    <Grid container alignItems="center" spacing={1} wrap="nowrap">
                                        <Grid size="auto">
                                            <Icon data={edit} />
                                        </Grid>
                                        <Grid size="grow">
                                            <Typography variant="h6">Application editor</Typography>
                                        </Grid>
                                    </Grid>
                                    <Grid container alignItems="center" wrap="nowrap">
                                        <AccessLinkContainer
                                            link
                                            href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+app+-+Editor+%28FUSION%29"
                                            target="_blank"
                                        >
                                            Concept App - Editor
                                            <ExternalLinkIcon data={external_link} />
                                        </AccessLinkContainer>
                                    </Grid>
                                </AccessGroupContainer>
                            </Grid>

                            {/* <Grid size={{ xs: 12, md: 3 }}>
                                <AccessGroupContainer>
                                    <Grid container alignItems="center" spacing={1} wrap="nowrap">
                                        <Grid size="auto">
                                            <Icon data={badge} />
                                        </Grid>
                                        <Grid size="grow">
                                            <Typography variant="h6">Application admin</Typography>
                                        </Grid>
                                    </Grid>
                                    <Grid container alignItems="center" wrap="nowrap">
                                        <AccessLinkContainer
                                            link
                                            href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+App+-+Admin+%28FUSION%29"
                                            target="_blank"
                                        >
                                            Concept App - Admin
                                            <ExternalLinkIcon data={external_link} />
                                        </AccessLinkContainer>
                                    </Grid>
                                </AccessGroupContainer>
                            </Grid> */}

                            <Grid size={{ xs: 12, md: 3 }}>
                                <AccessGroupContainer>
                                    <Grid container alignItems="center" spacing={1} wrap="nowrap">
                                        <Grid size="auto">
                                            <Icon data={visibility} />
                                        </Grid>
                                        <Grid size="grow">
                                            <Typography variant="h6">Application viewers</Typography>
                                        </Grid>
                                    </Grid>
                                    <Grid container alignItems="center" wrap="nowrap">
                                        <AccessLinkContainer
                                            link
                                            href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+App+-+Viewer+%28FUSION%29"
                                            target="_blank"
                                        >
                                            Concept App - Viewer
                                            <ExternalLinkIcon data={external_link} />
                                        </AccessLinkContainer>
                                    </Grid>
                                </AccessGroupContainer>
                            </Grid>
                        </OptionsContainer>
                    </Grid>
                )}
        </>
    )
}

export default ExternalAccessInfo
