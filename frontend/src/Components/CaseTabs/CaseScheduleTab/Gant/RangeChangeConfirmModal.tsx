import { Typography, Button } from "@mui/material"
import { FC } from "react"

import BaseModal from "@/Components/Modal/BaseModal"

export interface MilestoneToRemove {
    key: string;
    label: string;
    fromYear: number;
}

export interface MilestoneToMove {
    key: string;
    label: string;
    fromYear: number;
}

interface RangeChangeConfirmModalProps {
    isOpen: boolean;
    pendingStartYear: number | null;
    pendingEndYear: number | null;
    milestonesToRemove: MilestoneToRemove[];
    milestonesToMove: MilestoneToMove[];
    onConfirm: () => void;
    onCancel: () => void;
}

const RangeChangeConfirmModal: FC<RangeChangeConfirmModalProps> = ({
    isOpen,
    pendingStartYear,
    pendingEndYear,
    milestonesToRemove,
    milestonesToMove,
    onConfirm,
    onCancel,
}) => {
    const confirmModalContent = (
        <>
            <Typography variant="body1" gutterBottom>
                Changing the timeline range to
                {" "}
                {pendingStartYear}
                {" "}
                -
                {" "}
                {pendingEndYear}
                {" "}
                will affect milestones that are outside this range:
            </Typography>

            {milestonesToRemove.length > 0 && (
                <>
                    <Typography variant="subtitle2" gutterBottom sx={{ mt: 2 }}>
                        These optional milestones will be
                        {" "}
                        <Typography component="span" fontWeight="bold" color="error" display="inline">
                            removed
                        </Typography>
                        :
                    </Typography>
                    <ul>
                        {milestonesToRemove.map((milestone) => (
                            <li key={milestone.key}>
                                <Typography variant="body2">
                                    {milestone.label}
                                    {" "}
                                    (currently in year
                                    {" "}
                                    {milestone.fromYear}
                                    )
                                </Typography>
                            </li>
                        ))}
                    </ul>
                </>
            )}

            {milestonesToMove.length > 0 && (
                <>
                    <Typography variant="subtitle2" gutterBottom sx={{ mt: 2 }}>
                        These required milestones will be
                        {" "}
                        <Typography component="span" fontWeight="bold" display="inline">
                            moved
                        </Typography>
                        {" "}
                        to the center of the new range:
                    </Typography>
                    <ul>
                        {milestonesToMove.map((milestone) => (
                            <li key={milestone.key}>
                                <Typography variant="body2">
                                    {milestone.label}
                                    {" "}
                                    (currently in year
                                    {" "}
                                    {milestone.fromYear}
                                    )
                                </Typography>
                            </li>
                        ))}
                    </ul>
                </>
            )}

            <Typography variant="body1" sx={{ mt: 2 }}>
                Do you want to continue?
            </Typography>
        </>
    )

    const confirmModalActions = (
        <>
            <Button onClick={onCancel} color="inherit">
                Cancel
            </Button>
            <Button onClick={onConfirm} color="primary" variant="contained">
                Confirm
            </Button>
        </>
    )

    return (
        <BaseModal
            isOpen={isOpen}
            title="Confirm Timeline Range Change"
            content={confirmModalContent}
            actions={confirmModalActions}
            onClose={onCancel}
            size="sm"
        />
    )
}

export default RangeChangeConfirmModal
