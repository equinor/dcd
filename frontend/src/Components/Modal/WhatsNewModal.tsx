import React, { useState, useEffect } from "react"
import { Button, Typography, List } from "@equinor/eds-core-react"
import { v4 as uuidv4 } from "uuid"
import styled from "styled-components"
import Modal from "./Modal"

const Header = styled(Typography)`
    margin: 20px 0 10px 0;
`
const SubHeader = styled(Typography)`
    margin: 10px 0 5px 0;
`

type Version = `${number}.${number}.${number}`;

type Category = "New Features" | "UI Improvements" | "Bugfixes" | "Other";

type UpdateEntry = {
    description: string;
};

const whatsNewUpdates: { [key: Version]: { [key in Category]?: UpdateEntry[] } } = {
    "2.0.0": {
        "New Features": [
            { description: "All input fields Auto-save on change" },
            { description: "Undo/redo edits" },
            { description: "Upload photos in the case description tab" },
            { description: "Set the classification level of the project in the settings tab" },
            { description: "View the projects and cases in either edit or view mode" },

        ],
        "UI Improvements": [
            { description: "The current case tab is saved in the URL for sharing" },
            { description: "Case and project description input text can now have formatting" },
            { description: "Improved grid and responsiveness" },
            { description: "Improved project selection landing page" },
            { description: "Improved loading indicators" },
        ],
    },
    /*
    "2.1.0": {
        "New Features": [
            { description: "Edit history overview in the sidebar displays all changes made to each case in the past hour" },
        ],
    },
    */
}

const versions = Object.keys(whatsNewUpdates).sort((a, b) => {
    const [aMajor, aMinor, aPatch] = a.split(".").map(Number)
    const [bMajor, bMinor, bPatch] = b.split(".").map(Number)

    if (aMajor !== bMajor) { return bMajor - aMajor }
    if (aMinor !== bMinor) { return bMinor - aMinor }
    return bPatch - aPatch
}) as Version[]

const WhatsNewModal: React.FC = () => {
    const [isOpen, setIsOpen] = useState(false)
    const [unseenVersions, setUnseenVersions] = useState<Version[]>([])

    useEffect(() => {
        const lastSeenVersion = localStorage.getItem("lastSeenWhatsNewVersion") as Version | null
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
            setIsOpen(true)
        }
    }, [])

    const onClose = () => {
        localStorage.setItem("lastSeenWhatsNewVersion", versions[0])
        setIsOpen(false)
    }

    return (
        <Modal
            isOpen={isOpen}
            title="What's New"
            size="sm"
            content={unseenVersions.map((version) => (
                <div key={version}>
                    <Header variant="h4">
                        Version
                        {" "}
                        {version}
                    </Header>
                    {Object.entries(whatsNewUpdates[version]).map(([category, updates]) => (
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
