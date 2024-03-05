import {
    MouseEventHandler,
    FormEventHandler,
} from "react"
import {
    Button, Icon, Label, Typography,
} from "@equinor/eds-core-react"
import { add, archive } from "@equinor/eds-icons"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { getProjectPhaseName, getProjectCategoryName, unwrapProjectId } from "../../Utils/common"
import { GetProjectService } from "../../Services/ProjectService"
import { GetSTEAService } from "../../Services/STEAService"
import { useProjectContext } from "../../Context/ProjectContext"
import CasesTable from "../Case/OverviewCasesTable/CasesTable"
import { useModalContext } from "../../Context/ModalContext"
import Grid from "@mui/material/Grid"
import { useAppContext } from "../../Context/AppContext"

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

    const handleDescriptionChange: FormEventHandler = (e) => {
        const target = e.target as typeof e.target & {
            value: string
        }
        if (projectEdited) {
            const updatedProject = { ...projectEdited, description: target.value }
            setProjectEdited(updatedProject)
        }
    }

    const submitToSTEA: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        if (project) {
            try {
                const projectId = unwrapProjectId(project.id)
                const projectResult = await (await GetProjectService()).getProject(projectId)
                await (await GetSTEAService()).excelToSTEA(projectResult)
            } catch (error) {
                console.error("[ProjectView] error while submitting form data", error)
            }
        }
    }

    if (!project) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container columnSpacing={2} rowSpacing={3}>
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography group="input" variant="label">Project Phase:</Typography>
                    <Typography aria-label="Project phase">
                        {getProjectPhaseName(project.projectPhase)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Project Category:</Typography>
                    <Typography aria-label="Project category">
                        {getProjectCategoryName(project.projectCategory)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Country:</Typography>
                    <Typography aria-label="Country">
                        {project.country ?? "Not defined in Common Library"}
                    </Typography>
                </Grid>
                <Grid item>
                    <Button onClick={submitToSTEA}>
                        <Icon data={archive} size={18} />
                        Export to STEA
                    </Button>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <Typography group="input" variant="label" htmlFor="description">Project description</Typography>
                {editMode 
                ? <TextArea
                    id="description"
                    placeholder="Enter a description"
                    onInput={handleDescriptionChange}
                    value={projectEdited ? projectEdited.description : project?.description}
                    cols={10000}
                    rows={8}
                />
                : <Typography>{project.description ?? undefined}</Typography>}
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
