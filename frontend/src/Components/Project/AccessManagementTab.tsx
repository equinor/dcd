import { useEffect, useState } from "react"
import {
    Icon,
    Search,
    Typography,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"
import {
    edit,
    visibility,
    info_circle,
    chevron_down,
    chevron_up,
    external_link,
} from "@equinor/eds-icons"
import { useMediaQuery } from "@mui/material"

import { projectQueryFn } from "@/Services/QueryFunctions"
import { useAppContext } from "@/Context/AppContext"
import { User, PersonDetails } from "@/Models/AccessManagement"
import { GetAccessService } from "@/Services/AccessService"
import { useProjectContext } from "@/Context/ProjectContext"
import { usePeopleApi } from "@/Hooks/usePeopleApi"
import PersonMock from "./Components/PersonMock"
import SearchResultsMenu from "./Components/SearchResultsMenu"
import { apiResponseToPersonDetails } from "@/Utils/common"

const PeopleMock = {
    admins: [
        {
            name: "Ola Nordmann",
            email: "ola.nordmann@equinor.com",
        },
        {
            name: "Kari Nordmann",
            email: "kari.nordmann@equinor.com",
        },
    ],
    editors: [
        {
            name: "Per Nordmann",
            email: "per.nordmann@equinor.com",
        },
        {
            name: "Pål Nordmann",
            email: "pal.nordmann@equinor.com",
        },
        {
            name: "Espen Nordmann",
            email: "espen.nordmann@equinor.com",
        },
        {
            name: "Grom Nordmann",
            email: "grom.nordmann@equinor.com",
        },
    ],
    viewers: [
        {
            name: "Tor Nordmann",
            email: "tor.nordmann@equinor.com",
        },
        {
            name: "Odin Nordmann",
            email: "odin.nordmann@equinor.com",
        },
        {
            name: "Olav Nordmann",
            email: "olav.nordmann@equinor.com",
        },
        {
            name: "Loke Nordmann",
            email: "loke.nordmann@equinor.com",
        },
    ],
    orgChart: [
        {
            name: "Geir Nordmann",
            email: "geir.nordmann@equinor.com",
        },
        {
            name: "Egil Nordmann",
            email: "Egil.nordmann@equinor.com",
        },
        {
            name: "Erling Nordmann",
            email: "erling.nordmann@equinor.com",
        },
        {
            name: "Goggen Nordmann",
            email: "goggen.nordmann@equinor.com",
        },
    ],
}

const deleteFunctionMock = (name: string, email: string) => {
    console.log("deleting: ", name, email)
}

const EditorViewerContainer = styled(Grid)<{ $isSmallScreen: boolean }>`
    display: flex;
    justify-content: center;
    padding: 15px;
    margin-top: 35px;
    flex-direction: ${(props) => (props.$isSmallScreen ? "column" : "row")}!important;
`

const EditorViewerContent = styled.div<{ $right?: boolean; $isSmallScreen?: boolean; }>`
    display: flex;
    flex-direction: column;
    width: 100%;
    margin: ${(props) => (props.$right ? "0 0 0 50px" : "0 50px 0 0")};
    margin: ${(props) => (props.$isSmallScreen && "0")};
`

const EditorViewerHeading = styled.div<{ $smallGap?: boolean; }>`
    display: flex;
    align-items: center;
    width: 100%;
    gap: ${(props) => (props.$smallGap ? "3px" : "10px")};
    margin-bottom: 15px;
`

const PeopleContainer = styled.div`
    display: flex;
    margin: 20px 0 40px 0;
    flex-direction: column;
    gap: 20px;
`

const ClickableHeading = styled(Grid)`
    display: flex;
    align-items: center;
    margin-top: 35px;
    gap: 10px;
    cursor: pointer;
`

const WRITE_DELAY_MS = 1000

const AccessManagementTab = () => {
    const { editMode } = useAppContext()
    const { projectId } = useProjectContext()
    const peopleApi = usePeopleApi()
    const isSmallScreen = useMediaQuery("(max-width:960px)", { noSsr: true })

    const [searchQuery, setSearchQuery] = useState<string>("")
    const [isSearching, setIsSearching] = useState<boolean>(false)
    const [searchResults, setSearchResults] = useState<PersonDetails[] | undefined>([])
    const [searchResultsMenuAnchorEl, setSearchResultsMenuAnchorEl] = useState<any | null>(null)

    const [expandAllAccess, setExpandAllAccess] = useState<boolean>(true)
    const [editors, setEditors] = useState<User[] | undefined>([])
    const [viewers, setViewers] = useState<User[] | undefined>([])

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const searchPersons = () => {
        if (searchQuery) {
            setIsSearching(true)
            peopleApi.search(searchQuery)
                .then((res) => {
                    const result = apiResponseToPersonDetails(res)
                    setSearchResults(result)
                })
                .finally(() => {
                    setIsSearching(false)
                })
        }
    }

    const handlePersonSelected = (person: PersonDetails) => {
        console.log("adding person: ", person)
    }

    const isPersonSelected = (azureId: string) => {
        // console.log("person allready selected: ", azureId)
        console.log()
        return false
    }

    const handleAddPerson = async (personId: string) => {
        if (!personId && !projectId) { return null }
        const addPerson = await (await GetAccessService()).addPerson(projectId, { projectId, userId: personId, role: 1 })
        if (addPerson) {
            console.log(addPerson)
            // gjøre en invalidering av project query for å få oppdatert personer???
        }
        return true
    }

    const handleBlur = () => {
        setSearchResults([])
        setSearchQuery("")
    }

    useEffect(() => {
        const timeout = setTimeout(() => {
            searchPersons()
        }, WRITE_DELAY_MS)
        return () => {
            clearTimeout(timeout)
        }
    }, [searchQuery])

    useEffect(() => {
        const viewersToAdd = projectApiData?.projectMembers?.filter((m) => m.role === 0) as User[]
        const editorsToAdd = projectApiData?.projectMembers?.filter((m) => m.role === 1) as User[]
        setViewers(viewersToAdd)
        setEditors(editorsToAdd)
    }, [projectApiData])

    // Lag skeletons for loading state, når vi får persondata fra api
    if (!projectApiData) {
        return <div>Loading project data...</div>
    }

    return (
        <Grid container direction="column" paddingX="10px" spacing={2}>
            <Grid item>
                <Typography variant="h3">Access Management</Typography>
            </Grid>
            <Grid item>
                <Typography variant="body_short">
                    On this page the project admins can add and remove members to the project.
                    If the project classification is set to “restricted” or “confidential”,
                    only the project members and the application admin can access it.
                    Project members from Org chart with “PMT” are automatically added as project editors after DG0. External users can also be added here.
                </Typography>
            </Grid>
            <EditorViewerContainer $isSmallScreen={isSmallScreen}>
                <EditorViewerContent>
                    <EditorViewerHeading>
                        <Icon data={edit} />
                        <Typography variant="h6">Project editors</Typography>
                    </EditorViewerHeading>
                    {editMode && (
                        <Grid item>
                            <Search
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                onBlur={() => handleBlur()}
                                placeholder="Add new"
                                ref={setSearchResultsMenuAnchorEl}
                            />
                            <SearchResultsMenu
                                isMenuOpen={(searchResults?.length ?? 0) > 0 || isSearching}
                                menuAnchorEl={searchResultsMenuAnchorEl}
                                isSearching={isSearching}
                                searchResults={searchResults || []}
                                isPersonSelected={isPersonSelected}
                                handlePersonSelected={handlePersonSelected}
                            />
                        </Grid>
                    )}
                    <PeopleContainer>
                        {PeopleMock.editors.map((person) => (
                            <PersonMock
                                key={person.email}
                                name={person.name}
                                email={person.email}
                                action={deleteFunctionMock}
                                hideAction={!editMode}
                            />
                        ))}
                    </PeopleContainer>
                    <Typography variant="h6">PMT members from the project orgchart:</Typography>
                    <PeopleContainer>
                        {PeopleMock.orgChart.map((person) => (
                            <PersonMock
                                key={person.email}
                                name={person.name}
                                email={person.email}
                                hideAction
                            />
                        ))}
                    </PeopleContainer>
                </EditorViewerContent>
                <hr />
                <EditorViewerContent $right $isSmallScreen={isSmallScreen}>
                    <EditorViewerHeading>
                        <Icon data={visibility} />
                        <Typography variant="h6">Project viewers</Typography>
                    </EditorViewerHeading>
                    {editMode && (
                        <Grid item>
                            <Search placeholder="Add new" />
                        </Grid>
                    )}
                    <PeopleContainer>
                        {PeopleMock.viewers.map((person) => (
                            <PersonMock
                                key={person.email}
                                name={person.name}
                                email={person.email}
                                action={deleteFunctionMock}
                                hideAction={!editMode}
                            />
                        ))}
                    </PeopleContainer>
                </EditorViewerContent>
            </EditorViewerContainer>
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
                                    <Typography link href="https://google.com" target="_blank">Concept App - Editor</Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                            </Grid>
                            <Grid item>
                                <EditorViewerHeading>
                                    <Typography variant="h6">Application admin</Typography>
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography link href="https://google.com" target="_blank">Concept App - Admin</Typography>
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
                                <Typography link href="https://google.com" target="_blank">Concept App - Viewer</Typography>
                                <Icon color="#007079" size={16} data={external_link} />
                            </EditorViewerHeading>
                            <Typography variant="body_short">
                                The following AccessIT groups also have view access in the app:
                            </Typography>
                            <Grid item marginTop="20px" display="flex" flexDirection="column" gap="15px">
                                <EditorViewerHeading $smallGap>
                                    <Typography link href="https://google.com" target="_blank">Chief Engineers (FUSION)</Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography link href="https://google.com" target="_blank">Leading Advisors Project Management & Control (FUSION)</Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                                <EditorViewerHeading $smallGap>
                                    <Typography link href="https://google.com" target="_blank">Project Development Center - Management (FUSION)</Typography>
                                    <Icon color="#007079" size={16} data={external_link} />
                                </EditorViewerHeading>
                            </Grid>
                        </Grid>
                    </Grid>
                )}
        </Grid>
    )
}

export default AccessManagementTab
