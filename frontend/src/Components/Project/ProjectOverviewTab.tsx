import {
    Button, Icon, Typography, NativeSelect, Tooltip,
} from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { ChangeEventHandler } from "react"

import { getProjectPhaseName, getProjectCategoryName } from "@/Utils/common"
import { useModalContext } from "@/Context/ModalContext"
import { useAppContext } from "@/Context/AppContext"
import useEditProject from "@/Hooks/useEditProject"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { INTERNAL_PROJECT_PHASE } from "@/Utils/constants"
import useEditDisabled from "@/Hooks/useEditDisabled"
import InputSwitcher from "../Input/Components/InputSwitcher"
import CasesTable from "../Case/OverviewCasesTable/CasesTable"
import Gallery from "../Gallery/Gallery"

const ProjectOverviewTab = () => {
    const { isRevision, accessRights } = useProjectContext()
    const { revisionId } = useParams()
    const { editMode } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { addProjectEdit } = useEditProject()
    const { addNewCase } = useModalContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()

    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", externalId],
        queryFn: () => revisionQueryFn(externalId, revisionId),
        enabled: !!externalId && isRevision,
    })

    const handleBlur = (e: any) => {
        if (apiData) {
            // eslint-disable-next-line no-underscore-dangle
            const newValue = e.target._value
            const newProjectObject = { ...apiData, description: newValue }
            addProjectEdit(apiData.id, newProjectObject)
        }
    }

    const handleInternalProjectPhaseChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2].indexOf(Number(e.currentTarget.value)) !== -1 && apiData) {
            const newInternalProjectPhase: Components.Schemas.InternalProjectPhase = Number(e.currentTarget.value) as unknown as Components.Schemas.InternalProjectPhase
            const newProject: Components.Schemas.ProjectWithAssetsDto = { ...apiData }
            newProject.internalProjectPhase = newInternalProjectPhase
            addProjectEdit(apiData.id, newProject)
        }
    }

    if (!apiData) {
        return <div>Loading project data...</div>
    }

    const renderProjectPhase = () => {
        if (!apiData) { return null }

        const { projectPhase, internalProjectPhase } = apiData

        if ([3, 4, 5, 6, 7, 8].includes(projectPhase)) {
            return getProjectPhaseName(projectPhase)
        }

        const internalProjectPhaseOptions = Object.entries(INTERNAL_PROJECT_PHASE).map(([key, value]) => (
            <option key={key} value={key}>{value.label}</option>
        ))

        return (
            <InputSwitcher
                value={INTERNAL_PROJECT_PHASE[internalProjectPhase].label}
            >
                <NativeSelect
                    id="internalProjectPhase"
                    label=""
                    onChange={handleInternalProjectPhaseChange}
                    value={internalProjectPhase}
                >
                    {internalProjectPhaseOptions}
                </NativeSelect>
            </InputSwitcher>
        )
    }

    return (
        <Grid container columnSpacing={2} rowSpacing={3}>
            <Gallery />
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography group="input" variant="label">Project Phase</Typography>
                    {renderProjectPhase()}
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
                    : <MarkdownViewer value={isRevision ? apiRevisionData?.description : apiData.description ?? ""} />}
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography variant="h3">Cases</Typography>
                </Grid>
                <Grid item>
                    <Tooltip title={getEditDisabledText()}>
                        <Button
                            disabled={isEditDisabled}
                            onClick={() => addNewCase()}
                        >
                            <Icon data={add} size={24} />
                            Add new Case
                        </Button>
                    </Tooltip>
                </Grid>
                <Grid item xs={12}>
                    <CasesTable />
                </Grid>
            </Grid>
        </Grid>
    )
}

export default ProjectOverviewTab
