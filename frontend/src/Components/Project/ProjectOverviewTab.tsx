import {
    MouseEventHandler,
} from "react"
import {
    Button, Icon, Typography,
} from "@equinor/eds-core-react"
import { add, archive } from "@equinor/eds-icons"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { getProjectPhaseName, getProjectCategoryName, unwrapProjectId } from "../../Utils/common"
import { GetProjectService } from "../../Services/ProjectService"
import { GetSTEAService } from "../../Services/STEAService"
import CasesTable from "../Case/OverviewCasesTable/CasesTable"
import { useModalContext } from "../../Context/ModalContext"
import { useAppContext } from "../../Context/AppContext"
import Gallery from "../Gallery/Gallery"
import { projectQueryFn } from "../../Services/QueryFunctions"
import useEditProject from "../../Hooks/useEditProject"

const ProjectOverviewTab = () => {
    const { editMode } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { addProjectEdit } = useEditProject()
    const { addNewCase } = useModalContext()

    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,

    })

    const handleBlur = (e: any) => {
        if (apiData) {
            // eslint-disable-next-line no-underscore-dangle
            const newValue = e.target._value
            const newProjectObject = { ...apiData, description: newValue }
            addProjectEdit(apiData.id, newProjectObject)
        }
    }

    const submitToSTEA: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        // should we refactor this to use react-query?
        if (apiData) {
            try {
                const projectIdOld = unwrapProjectId(apiData.id)
                const projectResult = await (await GetProjectService()).getProject(projectIdOld)
                await (await GetSTEAService()).excelToSTEA(projectResult)
            } catch (err) {
                console.error("[ProjectView] error while submitting form data", err)
            }
        }
    }

    if (!apiData) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container columnSpacing={2} rowSpacing={3}>
            <Gallery />
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography group="input" variant="label">Project Phase</Typography>
                    <Typography aria-label="Project phase">
                        {getProjectPhaseName(apiData.projectPhase)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Project Category</Typography>
                    <Typography aria-label="Project category">
                        {getProjectCategoryName(apiData.projectCategory)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Country</Typography>
                    <Typography aria-label="Country">
                        {apiData.country ?? "Not defined in Common Library"}
                    </Typography>
                </Grid>
            </Grid>
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode
                    ? (
                        <MarkdownEditor
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            onBlur={(e) => handleBlur(e)}
                        >
                            {apiData.description ?? ""}
                        </MarkdownEditor>
                    )
                    : <MarkdownViewer value={apiData.description ?? ""} />}
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography variant="h3">Cases</Typography>
                </Grid>
                <Grid item>
                    <Button onClick={() => addNewCase()}>
                        <Icon data={add} size={24} />
                        Add new Case
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    <CasesTable />
                </Grid>
                <Grid item>
                    <Button variant="outlined" onClick={submitToSTEA}>
                        <Icon data={archive} size={18} />
                        Download input to STEA
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default ProjectOverviewTab
