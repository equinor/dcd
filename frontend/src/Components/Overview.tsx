import { useState, useEffect } from "react"
import { Outlet } from "react-router-dom"
import {
    Button,
    Snackbar,
    Typography,
} from "@equinor/eds-core-react"
import { useCurrentUser } from "@equinor/fusion-framework-react/hooks"
import styled from "styled-components"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import Sidebar from "./Controls/Sidebar/Sidebar"
import Controls from "./Controls/Controls"
import { useAppContext } from "../Context/AppContext"
import Modal from "./Modal/Modal"
import { PROJECT_CLASSIFICATION } from "../Utils/constants"
import { useModalContext } from "../Context/ModalContext"
import ProjectSkeleton from "./LoadingSkeletons/ProjectSkeleton"
import { projectQueryFn } from "../Services/QueryFunctions"
import { useProjectContext } from "../Context/ProjectContext"

const ControlsWrapper = styled.div`
    position: sticky;
    top: 0;
    z-index: 1;

`
const ContentWrapper = styled.div`
    display: flex;
    padding-bottom: 20px;
`

const MainView = styled.div`
    flex: 1;
`

interface WarnedProjectInterface {
    [key: string]: string[]
}

const Overview = () => {
    const currentUser = useCurrentUser()
    const { currentContext } = useModuleCurrentContext()
    const contextId = currentContext?.externalId
    const {
        isCreating,
        isLoading,
        snackBarMessage,
        setSnackBarMessage,
    } = useAppContext()
    const { setProjectId } = useProjectContext()
    const { featuresModalIsOpen } = useModalContext()
    const [warnedProjects, setWarnedProjects] = useState<WarnedProjectInterface | null>(null)
    const [projectClassificationWarning, setProjectClassificationWarning] = useState<boolean>(false)
    const [currentUserId, setCurrentUserId] = useState<string | null>(null)

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", contextId],
        queryFn: () => projectQueryFn(contextId),
        enabled: !!contextId,
    })

    function addVisitedProject() {
        if (apiData && currentUserId) {
            if (warnedProjects && warnedProjects[currentUserId]) {
                const wp = { ...warnedProjects }
                wp[currentUserId].push(apiData.id)
                setWarnedProjects(wp)
            } else {
                setWarnedProjects({ [currentUserId]: [apiData.id] })
            }
        }
    }

    function fetchPV() {
        setWarnedProjects(JSON.parse(String(localStorage.getItem("pv"))))
        // NOTE: pv stands for "projects visited". It's been abbreviated to avoid disclosing the classification of the project's ID
    }

    useEffect(() => {
        fetchPV()
        window.addEventListener("storage", () => fetchPV())
    }, [])

    useEffect(() => {
        if (currentUser && (!currentUserId || currentUser.localAccountId !== currentUserId)) {
            setCurrentUserId(currentUser.localAccountId as keyof typeof warnedProjects)
        }
    }, [currentUser])

    useEffect(() => {
        if (warnedProjects && JSON.stringify(warnedProjects) !== localStorage.getItem("pv")) {
            localStorage.setItem("pv", JSON.stringify(warnedProjects))
        }
    }, [warnedProjects])

    useEffect(() => {
        if (apiData) {
            setProjectId(apiData.id)
        }
    }, [apiData])

    useEffect(() => {
        if (apiData && currentUserId) {
            if (
                !projectClassificationWarning
                && PROJECT_CLASSIFICATION[apiData.classification].warn
                && (
                    (warnedProjects && !warnedProjects[currentUserId].some((vp: string) => vp === apiData.id))
                    || (warnedProjects && !warnedProjects[currentUserId])
                    || !warnedProjects
                )
            ) {
                if (!featuresModalIsOpen) {
                    setProjectClassificationWarning(true)
                }
            }
            if (warnedProjects && warnedProjects[currentUserId].some((vp: string) => vp === apiData.id)) {
                setProjectClassificationWarning(false)
            }
        }
    }, [apiData, currentUserId, warnedProjects, featuresModalIsOpen])

    if (isCreating || isLoading) {
        return (
            <ProjectSkeleton />
        )
    }

    return (
        <>
            <Snackbar open={snackBarMessage !== undefined} autoHideDuration={6000} onClose={() => setSnackBarMessage(undefined)}>
                {snackBarMessage}
            </Snackbar>
            <ContentWrapper>
                <Sidebar />
                <MainView className="ag-theme-alpine-fusion ">
                    {apiData && (
                        <Modal
                            isOpen={projectClassificationWarning}
                            size="sm"
                            title={`Attention - ${PROJECT_CLASSIFICATION[apiData.classification].label} project`}
                            content={(
                                <Typography key="text">
                                    {PROJECT_CLASSIFICATION[apiData.classification].description}
                                </Typography>
                            )}
                            actions={
                                <Button key="ok" onClick={() => addVisitedProject()}>OK</Button>
                            }
                        />
                    )}
                    <ControlsWrapper>
                        <Controls />
                    </ControlsWrapper>
                    <Outlet />
                </MainView>
            </ContentWrapper>

        </>
    )
}

export default Overview
