import { useMemo } from "react"
import {
    delete_to_trash, edit, swap_horizontal, visibility,
} from "@equinor/eds-icons"
import {
    Button, Icon, Tooltip, Typography,
} from "@equinor/eds-core-react"
import { PersonListItem, PersonSelect, PersonSelectEvent } from "@equinor/fusion-react-person"

import { EditorViewerContent, EditorViewerHeading, PeopleContainer } from "./AccessManagement.styles"
import { ProjectMemberRole } from "@/Models/enums"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

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
}: RolePanelProps) => {
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
                        onSelect={(selectedPerson) => handleAddPerson(selectedPerson as PersonSelectEvent, isViewers ? ProjectMemberRole.Observer : ProjectMemberRole.Editor)}
                    />
                )}
                {manuallyAddedPeople && manuallyAddedPeople.length > 0 ? (
                    <PeopleContainer>
                        {manuallyAddedPeople.filter((p) => !p.isPmt).map((person) => (
                            <PersonListItem key={person.userId} azureId={person.userId}>
                                {canEdit() && (
                                    <>
                                        <Tooltip title={`Switch to ${isViewers ? "editor" : "viewer"}`}>
                                            <Button variant="ghost_icon" onClick={() => handleSwitchPerson(person.userId, isViewers ? ProjectMemberRole.Editor : ProjectMemberRole.Observer)}>
                                                <Icon data={swap_horizontal} />
                                            </Button>
                                        </Tooltip>
                                        <Tooltip title={`Remove ${isViewers ? "viewer" : "editor"}`}>
                                            <Button variant="ghost_icon" color="danger" onClick={() => handleRemovePerson(person.userId)}>
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
                            orgChartPeople.map((p) => (<PersonListItem key={p.userId} azureId={p.userId} />))
                        }
                    </>
                ) : <Typography variant="h6">No PMT members from the project orgchart found</Typography>}
            </PeopleContainer>
        </EditorViewerContent>
    )
}

export default RolePanel
