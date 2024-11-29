// import { useMemo } from "react"
import {
    delete_to_trash, edit, swap_horizontal, visibility,
} from "@equinor/eds-icons"
import {
    Button, Icon, Tooltip, Typography,
} from "@equinor/eds-core-react"
import { PersonListItem, PersonSelect, PersonSelectEvent } from "@equinor/fusion-react-person"

import { EditorViewerContent, EditorViewerHeading, PeopleContainer } from "./AccessManagement.styles"
import { UserRole } from "@/Models/AccessManagement"
import { useAppContext } from "@/Context/AppContext"
import { useProjectContext } from "@/Context/ProjectContext"

interface RolePanelProps {
    isSmallScreen: boolean;
    isViewers?: boolean;
    people?: Components.Schemas.ProjectMemberDto[] | undefined;
    handleAddPerson: (e: PersonSelectEvent, role: UserRole) => void;
    handleSwitchPerson: (userId: string, role: UserRole) => void;
    handleRemovePerson: (userId: string) => void;
}

const RolePanel = ({
    isSmallScreen, isViewers, people, handleAddPerson, handleSwitchPerson, handleRemovePerson,
}: RolePanelProps) => {
    const { editMode } = useAppContext()
    const { accessRights } = useProjectContext()

    // const orgChartPeople = useMemo(() => people?.filter(person => person.isOrgChart === true), [people])

    return (
        <EditorViewerContent $right={isViewers} $isSmallScreen={isSmallScreen}>
            <div>
                <EditorViewerHeading>
                    <Icon data={isViewers ? visibility : edit} />
                    <Typography variant="h6">{isViewers ? "Project viewers" : "Project editors"}</Typography>
                </EditorViewerHeading>
                {editMode && accessRights?.canEdit && (
                    <PersonSelect
                        placeholder={`Add new ${isViewers ? "viewer" : "editor"}`}
                        selectedPerson={null}
                        onSelect={(selectedPerson) => handleAddPerson(selectedPerson as PersonSelectEvent, isViewers ? UserRole.Viewer : UserRole.Editor)}
                    />
                )}
                {people && people.length > 0 ? (
                    <PeopleContainer>
                        {people.map((person) => (
                            <PersonListItem key={person.userId} azureId={person.userId}>
                                {editMode && accessRights?.canEdit && (
                                    <>
                                        <Tooltip title={`Switch to ${isViewers ? "editor" : "viewer"}`}>
                                            <Button variant="ghost_icon" onClick={() => handleSwitchPerson(person.userId, isViewers ? UserRole.Editor : UserRole.Viewer)}>
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
                ) : (<Typography style={{ marginBottom: "150px" }} variant="body_short">{!editMode && `No project ${isViewers ? "viewers" : "editors"} found`}</Typography>)}
            </div>
            {/* <PeopleContainer $orgChart>
                {orgChartPeople && orgChartPeople.length > 0 ? (
                    <>
                        <Typography variant="h6">PMT members from the project orgchart:</Typography>
                        {
                            orgChartPeople.map((p) => (<PersonListItem key={p.azureUniqueId} azureId={p.azureUniqueId} />))
                        }
                    </>
                ) : <Typography variant="h6">No PMT members from the project orgchart found</Typography>}
            </PeopleContainer> */}
        </EditorViewerContent>
    )
}

export default RolePanel
