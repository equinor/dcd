import {
    Button, Icon, Tooltip, Typography,
} from "@equinor/eds-core-react"
import {
    delete_to_trash, edit, swap_horizontal, visibility,
} from "@equinor/eds-icons"
import { PersonListItem, PersonSelect, PersonSelectEvent } from "@equinor/fusion-react-person"
import { useMemo } from "react"

import { EditorViewerContent, EditorViewerHeading, PeopleContainer } from "./AccessManagement.styles"

import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { ProjectMemberRole } from "@/Models/enums"

interface RolePanelProps {
    isSmallScreen: boolean;
    isViewers?: boolean;
    people?: Components.Schemas.ProjectMemberDto[] | undefined;
    handleAddPerson: (e: PersonSelectEvent, role: ProjectMemberRole) => void;
    handleSwitchPerson: (userId: string, role: ProjectMemberRole) => void;
    handleRemovePerson: (userId: string) => void;
}

const RolePanel = ({
    isSmallScreen, isViewers, people, handleAddPerson, handleSwitchPerson, handleRemovePerson,
}: RolePanelProps): React.ReactNode => {
    const { canEdit } = useCanUserEdit()
    const orgChartPeople = useMemo(() => people?.filter((person) => person.isPmt === true), [people])
    const manuallyAddedPeople = useMemo(() => people?.filter((person) => person.isPmt === false), [people])

    return (
        <EditorViewerContent $right={isViewers} $isSmallScreen={isSmallScreen}>
            <div>
                <EditorViewerHeading>
                    <Icon data={isViewers ? visibility : edit} />
                    <Typography variant="h6">{isViewers ? "Project viewers" : "Project editors"}</Typography>
                </EditorViewerHeading>
                {canEdit() && (
                    <PersonSelect
                        placeholder={`Add new ${isViewers ? "viewer" : "editor"}`}
                        selectedPerson={null}
                        onSelect={(selectedPerson): void => handleAddPerson(selectedPerson as PersonSelectEvent, isViewers ? ProjectMemberRole.Observer : ProjectMemberRole.Editor)}
                    />
                )}
                {manuallyAddedPeople && manuallyAddedPeople.length > 0 ? (
                    <PeopleContainer>
                        {manuallyAddedPeople.filter((p) => !p.isPmt).map((person) => (
                            <PersonListItem key={person.azureAdUserId} azureId={person.azureAdUserId}>
                                {canEdit() && (
                                    <>
                                        <Tooltip title={`Switch to ${isViewers ? "editor" : "viewer"}`}>
                                            <Button
                                                variant="ghost_icon"
                                                onClick={(): void => handleSwitchPerson(person.azureAdUserId, isViewers ? ProjectMemberRole.Editor : ProjectMemberRole.Observer)}
                                            >
                                                <Icon data={swap_horizontal} />
                                            </Button>
                                        </Tooltip>
                                        <Tooltip title={`Remove ${isViewers ? "viewer" : "editor"}`}>
                                            <Button variant="ghost_icon" color="danger" onClick={(): void => handleRemovePerson(person.azureAdUserId)}>
                                                <Icon data={delete_to_trash} />
                                            </Button>
                                        </Tooltip>
                                    </>
                                )}
                            </PersonListItem>
                        ))}
                    </PeopleContainer>
                ) : (<Typography style={{ marginBottom: "150px" }} variant="body_short">{!canEdit() && `No project ${isViewers ? "viewers" : "editors"} found`}</Typography>)}
            </div>
            <PeopleContainer $orgChart>
                {orgChartPeople && orgChartPeople.length > 0 ? (
                    <>
                        <Typography variant="h6">PMT members from the project orgchart:</Typography>
                        {
                            orgChartPeople.map((p) => (
                                <PersonListItem key={p.azureAdUserId} azureId={p.azureAdUserId}>
                                    {canEdit() && (
                                        <Tooltip title={`Switch to ${isViewers ? "editor" : "viewer"}`}>
                                            <Button
                                                variant="ghost_icon"
                                                onClick={(): void => handleSwitchPerson(p.azureAdUserId, isViewers ? ProjectMemberRole.Editor : ProjectMemberRole.Observer)}
                                            >
                                                <Icon data={swap_horizontal} />
                                            </Button>
                                        </Tooltip>

                                    )}
                                </PersonListItem>
                            ))
                        }
                    </>
                ) : <Typography variant="h6">No PMT members from the project orgchart found</Typography>}
            </PeopleContainer>
        </EditorViewerContent>
    )
}

export default RolePanel
