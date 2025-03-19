import {
    Button, Icon, Typography, NativeSelect, Tooltip,
} from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { MarkdownEditor, MarkdownViewer } from "@equinor/fusion-react-markdown"
import Grid from "@mui/material/Grid2"
import { ChangeEventHandler, useState } from "react"

import { getProjectPhaseName, getProjectCategoryName } from "@/Utils/common"
import { useModalContext } from "@/Store/ModalContext"
import { useAppStore } from "@/Store/AppStore"
import useEditProject from "@/Hooks/useEditProject"
import { INTERNAL_PROJECT_PHASE } from "@/Utils/constants"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useDataFetch } from "@/Hooks"
import InputSwitcher from "@/Components/Input/Components/InputSwitcher"
import AllCasesTable from "@/Components/Tables/ProjectTables/AllCasesTable"
import Gallery from "@/Components/Gallery/Gallery"
import ProjectSkeleton from "@/Components/LoadingSkeletons/ProjectSkeleton"

const ProjectOverviewTab = () => {
    const { editMode } = useAppStore()
    const { addProjectEdit } = useEditProject()
    const { addNewCase } = useModalContext()
    const { canEdit, isEditDisabled, getEditDisabledText } = useCanUserEdit()
    const revisionAndProjectData = useDataFetch()

    const [debounceTimeout, setDebounceTimeout] = useState<NodeJS.Timeout | null>(null)

    const handleDescriptionChange = (e: any) => {
        if (debounceTimeout) {
            clearTimeout(debounceTimeout)
        }
        // eslint-disable-next-line no-underscore-dangle
        const newDescription = e.target._value

        const timeout = setTimeout(() => {
            if (revisionAndProjectData) {
                const updatedProject = {
                    ...revisionAndProjectData.commonProjectAndRevisionData,
                    description: newDescription,
                }
                addProjectEdit(revisionAndProjectData.projectId, updatedProject)
            }
        }, 3000)

        setDebounceTimeout(timeout)
    }

    const handlePhaseChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
        const selectedPhase = Number(e.currentTarget.value)
        if ([0, 1, 2].includes(selectedPhase) && revisionAndProjectData) {
            const updatedProject = {
                ...revisionAndProjectData.commonProjectAndRevisionData,
                internalProjectPhase: selectedPhase as Components.Schemas.InternalProjectPhase,
            }
            addProjectEdit(revisionAndProjectData.projectId, updatedProject)
        }
    }

    const renderProjectPhase = () => {
        const { projectPhase, internalProjectPhase } = revisionAndProjectData?.commonProjectAndRevisionData ?? {}

        if (projectPhase !== undefined && [3, 4, 5, 6, 7, 8].includes(projectPhase)) {
            return getProjectPhaseName(projectPhase)
        }

        const phaseOptions = Object.entries(INTERNAL_PROJECT_PHASE).map(([key, value]) => (
            <option key={key} value={key}>{value.label}</option>
        ))

        return (
            <InputSwitcher
                value={internalProjectPhase !== undefined ? INTERNAL_PROJECT_PHASE[internalProjectPhase].label : ""}
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

    if (!revisionAndProjectData) {
        return <ProjectSkeleton />
    }

    return (
        <Grid container columnSpacing={2} rowSpacing={3}>
            <Gallery />
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={12} container spacing={1} justifyContent="space-between">
                        <Grid size={4}>
                            <Typography group="input" variant="label">Project Phase</Typography>
                            {renderProjectPhase()}
                        </Grid>
                        <Grid size={4}>
                            <Typography group="input" variant="label">Project Category</Typography>
                            <Typography aria-label="Project category">
                                {getProjectCategoryName(revisionAndProjectData.commonProjectAndRevisionData.projectCategory)}
                            </Typography>
                        </Grid>
                        <Grid size={4}>
                            <Typography group="input" variant="label">Country</Typography>
                            <Typography aria-label="Country">
                                {revisionAndProjectData.commonProjectAndRevisionData.country ?? "Not defined in Common Library"}
                            </Typography>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>

            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={12} sx={{ marginBottom: (canEdit()) ? "32px" : 0 }}>
                        <Typography group="input" variant="label">Description</Typography>
                        {canEdit()
                            ? (
                                <MarkdownEditor
                                    menuItems={["strong", "em", "bullet_list", "ordered_list", "blockquote", "h1", "h2", "h3", "paragraph"]}
                                    onInput={handleDescriptionChange}
                                >
                                    {revisionAndProjectData.commonProjectAndRevisionData.description ?? ""}
                                </MarkdownEditor>
                            )
                            : (
                                <MarkdownViewer
                                    value={revisionAndProjectData.commonProjectAndRevisionData.description ?? ""}
                                />
                            )}
                    </Grid>
                </Grid>
            </Grid>

            <Grid size={12} container spacing={1} justifyContent="space-between">
                <Grid size={12}>
                    <Typography variant="h3">Cases</Typography>
                </Grid>
                <Grid size={12}>
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
                <Grid size={12}>
                    <AllCasesTable />
                </Grid>
            </Grid>
        </Grid>
    )
}

export default ProjectOverviewTab
