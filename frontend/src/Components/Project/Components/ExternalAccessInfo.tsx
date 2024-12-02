import { useState } from "react"
import {
    chevron_down, chevron_up, edit, external_link, info_circle, visibility,
} from "@equinor/eds-icons"
import { Icon, Typography } from "@equinor/eds-core-react"
import { Grid } from "@mui/material"

import { ClickableHeading, EditorViewerHeading } from "./AccessManagement.styles"

const ExternalAccessInfo = () => {
    const [expandAllAccess, setExpandAllAccess] = useState<boolean>(true)
    return (
        <>
            <ClickableHeading item onClick={() => setExpandAllAccess(!expandAllAccess)}>
                <Icon data={info_circle} />
                <Typography variant="h4">Would you like to access all internal Concept App projects?</Typography>
                <Icon data={expandAllAccess ? chevron_up : chevron_down} />
            </ClickableHeading>
            {expandAllAccess
                && (
                    <Grid container item>
                        <Typography variant="body_short">
                            In order to access all internal projects in Concept App, you need to apply for access in one of the AccessIT groups listed below.
                            Keep in mind that “Restricted” or “Confidential” projects are only accesible to project members.
                        </Typography>
                        <Grid container item gap="100px" marginTop="25px">
                            <Grid item>
                                <EditorViewerHeading $smallGap>
                                    <Icon data={edit} />
                                    <Typography variant="h6">Application editor</Typography>
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography
                                        link
                                        href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+App+-+Project+Member+%28FUSION%29"
                                        target="_blank"
                                    >
                                        Concept App - Editor
                                    </Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                            </Grid>
                            <Grid item>
                                <EditorViewerHeading>
                                    <Typography variant="h6">Application admin</Typography>
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography
                                        link
                                        href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+App+-+Admin+%28FUSION%29"
                                        target="_blank"
                                    >
                                        Concept App - Admin
                                    </Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                            </Grid>
                        </Grid>
                        <Grid item marginTop="20px">
                            <EditorViewerHeading>
                                <Icon data={visibility} />
                                <Typography variant="h6">Application viewers</Typography>
                            </EditorViewerHeading>
                            <EditorViewerHeading $smallGap>
                                <Typography
                                    link
                                    href="https://accessit.equinor.com/Search/Search?term=Fusion+-+Concept+App+-+Observer+%28FUSION%29"
                                    target="_blank"
                                >
                                    Concept App - Viewer
                                </Typography>
                                <Icon color="#007079" size={16} data={external_link} />
                            </EditorViewerHeading>
                            <Grid item marginTop="50px" display="flex" flexDirection="column" gap="15px">
                                <Typography variant="body_short">
                                    The following AccessIT groups also have view access in the app:
                                </Typography>
                                <EditorViewerHeading $smallGap>
                                    <Typography
                                        link
                                        href="https://accessit.equinor.com/Search/Search?term=Chief+Engineers+%28FUSION%29"
                                        target="_blank"
                                    >
                                        Chief Engineers (FUSION)
                                    </Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography
                                        link
                                        href="https://accessit.equinor.com/Search/Search?term=Leading+Advisors+Project+Management+%26+Control+%28FUSION%29"
                                        target="_blank"
                                    >
                                        Leading Advisors Project Management & Control (FUSION)
                                    </Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography
                                        link
                                        href="https://accessit.equinor.com/Search/Search?term=Project+Development+Center+-+Management+%28FUSION%29"
                                        target="_blank"
                                    >
                                        Project Development Center - Management (FUSION)
                                    </Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                            </Grid>
                        </Grid>
                    </Grid>
                )}
        </>
    )
}

export default ExternalAccessInfo
