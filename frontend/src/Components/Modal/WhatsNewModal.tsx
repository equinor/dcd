import React, { useState, useEffect } from "react"
import { Button, Typography, List } from "@equinor/eds-core-react"
import { v4 as uuidv4 } from "uuid"
import styled from "styled-components"
import BaseModal from "./BaseModal"
import { useModalContext } from "@/Store/ModalContext"
import { useLocalStorage } from "@/Hooks"
import { Version } from "@/Models/Interfaces"
import { versions, whatsNewUpdates } from "@/Utils/Config/whatsNewData"

const Header = styled(Typography)`
    margin: 20px 0 10px 0;
`
const SubHeader = styled(Typography)`
    margin: 10px 0 5px 0;
`

const WhatsNewModal: React.FC = () => {
    const { featuresModalIsOpen, setFeaturesModalIsOpen } = useModalContext()
    const [unseenVersions, setUnseenVersions] = useState<Version[]>([])
    const [lastSeenVersion, setLastSeenVersion] = useLocalStorage<Version | null>("lastSeenWhatsNewVersion", null)

    useEffect(() => {
        const unseen = lastSeenVersion
            ? versions.filter((version) => {
                const [lastMajor, lastMinor, lastPatch] = lastSeenVersion.split(".").map(Number)
                const [versionMajor, versionMinor, versionPatch] = version.split(".").map(Number)

                if (versionMajor > lastMajor) { return true }
                if (versionMajor < lastMajor) { return false }
                if (versionMinor > lastMinor) { return true }
                if (versionMinor < lastMinor) { return false }
                return versionPatch > lastPatch
            })
            : versions

        if (unseen.length > 0) {
            setUnseenVersions(unseen)
            setFeaturesModalIsOpen(true)
        }
    }, [lastSeenVersion])

    const onClose = () => {
        setLastSeenVersion(versions[0])
        setFeaturesModalIsOpen(false)
    }

    return (
        <BaseModal
            isOpen={featuresModalIsOpen}
            title="What's New"
            size="sm"
            content={unseenVersions.map((version) => (
                <div key={version}>
                    <Header variant="h4">
                        Version
                        {" "}
                        {version}
                    </Header>
                    {Object.entries(whatsNewUpdates[version].updates).map(([category, updates]) => (
                        <div key={category}>
                            <SubHeader variant="h6">{category}</SubHeader>
                            <List>
                                {updates?.map((entry) => (
                                    <List.Item key={uuidv4()}>
                                        {entry.description}
                                    </List.Item>
                                ))}
                            </List>
                        </div>
                    ))}
                </div>
            ))}
            actions={[
                <Button key="close" onClick={onClose}>Close</Button>,
            ]}
        />
    )
}

export default WhatsNewModal
