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

type Category = "New Functionalities" | "UI Improvements" | "Bugfixes" | "Other";

type UpdateEntry = {
    description: string;
};

const whatsNewUpdates: { [key: Version]: { [key in Category]?: UpdateEntry[] } } = {
    "2.0.0": {
        "New Functionalities": [
            { description: "All input fields Auto-save on change" },
            { description: "Upload photos in the case description tab" },
            { description: "Set the classification level of the project in the settings tab" },
            { description: "View the projects and cases in either edit or view mode" },
            { description: "The current case tab is saved in the URL for sharing" },
            { description: "Allow inserting 4 decimals instead of 2 in production profile table" },
            { description: "Navigate the project and add new cases in the sidebar" },
            { description: "A modal box appears with information about all changes whenever there is a new release" },
            { description: "View 'Export to stea' data in each case summary tab" },
            { description: "Added new production profile 'Deferred oil production' & 'Deferred gas production'" },
            { description: "Added input validation with descriptive error messages" },
        ],
        "UI Improvements": [
            { description: "Case and project description input text can now have formatting" },
            { description: "Improved grid and responsiveness" },
            { description: "Improved project selection landing page" },
            { description: "Improved loading indicators" },
            { description: "Improved sidebar styling" },
            { description: "Added case deletion confirmation prompt" },
            { description: "Added table description to Well costs" },
            { description: "When creating a new case, user is no longer automatically redireted to it" },
            { description: "Removed send feedback button" },
            { description: "Open cases directly from the Case overview table" },
            { description: "Combined title and unit into a single column to save space" },
            { description: "Show only relevant DG dates in Scehdule tab" },
        ],
        Bugfixes: [
            { description: "Technical input window does not appear in project 'chloris'" },
            { description: "Removed 'Expected Ream MNOK'21 from STEA export" },
            { description: "Pasting numbers from Excel to tables puts all inserted values into a single cell" },
            { description: "Unable to add numbers to tables if they have custom format from Excel" },
        ],
    },
    /*
    "2.1.0": {
        "New Features": [
            { description: "Undo/redo edits" },
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
