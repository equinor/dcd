import {
    Button, Icon, Typography, NativeSelect, Tooltip,
} from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import { ChangeEventHandler, useState } from "react"

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
    const { isRevision, projectId } = useProjectContext()
    const { revisionId } = useParams()
    const { editMode } = useAppContext()
    const { addProjectEdit } = useEditProject()
    const { addNewCase } = useModalContext()
    const { isEditDisabled, getEditDisabledText } = useEditDisabled()

    const [debounceTimeout, setDebounceTimeout] = useState<NodeJS.Timeout | null>(null)

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const { data: revisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    const handleDescriptionChange = (e: any) => {
        if (debounceTimeout) {
            clearTimeout(debounceTimeout)
        }
        // eslint-disable-next-line no-underscore-dangle
        const newDescription = e.target._value

        const timeout = setTimeout(() => {
            if (projectData) {
                const updatedProject = {
                    ...projectData.commonProjectAndRevisionData,
                    description: newDescription,
                }
                addProjectEdit(projectData.projectId, updatedProject)
            }
        }, 3000)

        setDebounceTimeout(timeout)
    }

    const handlePhaseChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
        const selectedPhase = Number(e.currentTarget.value)
        if ([0, 1, 2].includes(selectedPhase) && projectData) {
            const updatedProject = {
                ...projectData.commonProjectAndRevisionData,
                internalProjectPhase: selectedPhase as Components.Schemas.InternalProjectPhase,
            }
            addProjectEdit(projectData.projectId, updatedProject)
        }
    }

    if (!projectData) {
        return <div>Loading project data...</div>
    }

    const renderProjectPhase = () => {
        const { projectPhase, internalProjectPhase } = projectData.commonProjectAndRevisionData
        const revisionProjectPhase = revisionData?.commonProjectAndRevisionData.projectPhase
        const revisionInternalPhase = revisionData?.commonProjectAndRevisionData.internalProjectPhase

        if ([3, 4, 5, 6, 7, 8].includes(projectPhase)) {
            return getProjectPhaseName(isRevision ? revisionProjectPhase : projectPhase)
        }

        const phaseOptions = Object.entries(INTERNAL_PROJECT_PHASE).map(([key, value]) => (
            <option key={key} value={key}>{value.label}</option>
        ))

        return (
            <InputSwitcher
                value={isRevision && revisionInternalPhase
                    ? INTERNAL_PROJECT_PHASE[revisionInternalPhase].label ?? ""
                    : INTERNAL_PROJECT_PHASE[internalProjectPhase].label ?? ""}
            >
                <NativeSelect
                    id="internalProjectPhase"
                    label=""
                    onChange={handlePhaseChange}
                    value={internalProjectPhase}
                >
                    {phaseOptions}
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
                        {isRevision
                            ? getProjectCategoryName(revisionData?.commonProjectAndRevisionData.projectCategory)
                            : getProjectCategoryName(projectData.commonProjectAndRevisionData.projectCategory)}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography group="input" variant="label">Country</Typography>
                    <Typography aria-label="Country">
                        {isRevision
                            ? revisionData?.commonProjectAndRevisionData.country ?? "Not defined in Common Library"
                            : projectData.commonProjectAndRevisionData.country ?? "Not defined in Common Library"}
                    </Typography>
                </Grid>
            </Grid>
            <Grid item xs={12} sx={{ marginBottom: editMode ? "32px" : 0 }}>
                <Typography group="input" variant="label">Description</Typography>
                {editMode
                    ? (
                        <MarkdownEditor
                            menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                            onInput={handleDescriptionChange}
                        >
                            {projectData.commonProjectAndRevisionData.description ?? ""}
                        </MarkdownEditor>
                    )
                    : (
                        <MarkdownViewer
                            value={isRevision
                                ? revisionData?.commonProjectAndRevisionData.description ?? ""
                                : projectData.commonProjectAndRevisionData.description ?? ""}
                        />
                    )}
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="space-between">
                <Grid item>
                    <Typography variant="h3">Cases</Typography>
                </Grid>
                <Grid item>
                    <Tooltip title={getEditDisabledText()}>
                        <Button
                            disabled={isEditDisabled}
                            onClick={addNewCase}
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
