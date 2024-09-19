import {
    Button, Icon, Typography,
} from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"

import { getProjectPhaseName, getProjectCategoryName } from "@/Utils/common"
import { useProjectContext } from "@/Context/ProjectContext"
import { useModalContext } from "@/Context/ModalContext"
import { useAppContext } from "@/Context/AppContext"
import CasesTable from "../Case/OverviewCasesTable/CasesTable"
import Gallery from "../Gallery/Gallery"

const ProjectOverviewTab = () => {
    const { editMode } = useAppContext()
    const {
        project,
        projectEdited,
        setProjectEdited,
    } = useProjectContext()

    const {
        addNewCase,
    } = useModalContext()

    function handleDescriptionChange(value: string) {
        if (projectEdited) {
            const updatedProject = { ...projectEdited, description: value }
            setProjectEdited(updatedProject)
        }
    }

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container columnSpacing={2} rowSpacing={3}>
            <Gallery />
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography group="input" variant="label">Project Phase</Typography>
                    <Typography aria-label="Project phase">
                        {getProjectPhaseName(project.projectPhase)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Project Category</Typography>
                    <Typography aria-label="Project category">
                        {getProjectCategoryName(project.projectCategory)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Country</Typography>
                    <Typography aria-label="Country">
                        {project.country ?? "Not defined in Common Library"}
                    </Typography>
                </Grid>
            </Grid>
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode
                    ? (
                        <MarkdownEditor
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            onInput={(markdown) => {
                                // eslint-disable-next-line no-underscore-dangle
                                const value = (markdown as any).target._value
                                handleDescriptionChange(value)
                            }}
                        >
                            {projectEdited?.description !== undefined ? projectEdited.description : project?.description}
                        </MarkdownEditor>
                    )
                    : <MarkdownViewer value={project.description} />}
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
            </Grid>
        </Grid>
    )
}

export default ProjectOverviewTab
