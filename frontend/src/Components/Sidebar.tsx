import styled from "styled-components"
import { NavLink } from "react-router-dom"
import { Icon, Button } from "@equinor/eds-core-react"
import { file, folder } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { tokens } from "@equinor/eds-tokens"
import { useAppContext } from "../context/AppContext"
import { casePath, projectPath } from "../Utils/common"

const Wrapper = styled.div`
    z-index: 1000;
    display: flex;
    flex-direction: column;
    width: 250px;
    align-items: stretch;
    box-shadow:
        10px 0 5px -5px rgba(0, 0, 0, 0.03),
        15px 0 15px -5px rgba(0, 0, 0, 0.03);
`

const Body = styled.div`
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    gap: 10px;
    justify-content: space-between;
    flex: 1;
`
const ProjectRow = styled.div`
    display: flex;
    align-items: center;
    padding: 20px 10px;
    cursor: pointer;
    font-weight: 600;
    background-color: ${tokens.colors.interactive.primary__resting.rgba};
    color: white;
    transition: background-color 0.3s ease;

    :hover {
        background-color: ${tokens.colors.interactive.primary__hover.rgba};
    }

    svg {
        margin-right: 10px;
    }
`

const ProjectNavLink = styled(NavLink)`
    text-decoration: none;
`
const CasesWrapper = styled.div`
`

const Cases = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0 5px;
    gap: 5px;
`

const Link = styled.a`
    color: black;
    text-decoration: none;
    font-size: 12px;
    cursor: pointer;
    padding: 10px;
    text-align: center;

    :hover {
        color: ${tokens.colors.interactive.primary__hover.rgba};
    }
`

const CasesHeader = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 20px 10px 5px 10px;

    input[type="button"] {
        color: ${tokens.colors.interactive.primary__resting.rgba};
        border: none;
        background-color: transparent;
        cursor: pointer;
        border-radius: 50%;
        transition: transform 0.3s ease;

        :hover {
            transform: scale(1.3);
        }
    }
`

const CasesHeaderText = styled.div`
    font-size: 10px;
    opacity: 0.5;
    text-transform: uppercase;
    letter-spacing: 1.5;
    font-weight: 600;
`
const CaseNavLink = styled(NavLink)`
    text-decoration: none;
    display: flex;
    align-items: center;
    padding: 10px;
    border-radius: 5px;
    color: black;
    transition: background-color 0.3s ease;
    background-color: white;

    :hover {
        background-color: ${tokens.colors.interactive.primary__hover_alt.rgba};
    }

    svg {
        margin-right: 10px;
    }

    &.active {
        background-color: ${tokens.colors.interactive.primary__hover_alt.rgba};
    }
`

const Sidebar = () => {
    const { project, setCreateCaseModalIsOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()

    return (
        <Wrapper>
            <ProjectNavLink to={projectPath(currentContext?.externalId!)}>
                <ProjectRow>
                    <Icon data={folder} size={18} />
                    {project!.name}
                </ProjectRow>
            </ProjectNavLink>
            <Body>
                <CasesWrapper>
                    <CasesHeader>
                        <CasesHeaderText>Cases</CasesHeaderText>
                        <input
                            type="button"
                            value="+"
                            onClick={() => { setCreateCaseModalIsOpen(true) }}
                        />
                    </CasesHeader>
                    <Cases>
                        {
                            project!.cases.map((subItem, index) => (
                                <CaseNavLink
                                    key={`menu - item - ${index + 1} `}
                                    to={casePath(currentContext?.id!, subItem.id ? subItem.id : "")}
                                    className={({ isActive }) => (isActive ? "active" : "")}
                                >
                                    <Icon data={file} size={16} />
                                    {subItem.name ? subItem.name : "Untitled"}
                                </CaseNavLink>
                            ))
                        }

                    </Cases>
                </CasesWrapper>
                <Link
                    href="https://forms.office.com/Pages/ResponsePage.aspx?id=NaKkOuK21UiRlX_PBbRZsCjGTHQnxJxIkcdHZ_YqW4BUMTQyTVNLOEY0VUtSUjIwN1QxUVJIRjBaNC4u"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Send feedback
                </Link>
            </Body>
        </Wrapper>
    )
}

export default Sidebar
