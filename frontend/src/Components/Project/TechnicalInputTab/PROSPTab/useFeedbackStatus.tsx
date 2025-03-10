import { useState, useCallback } from "react"
import { FeedbackStatus } from "./ActionButton"

interface UseFeedbackStatusResult {
    isLoading: boolean;
    feedbackStatus: FeedbackStatus;
    setLoading: (loading: boolean) => void;
    setSuccess: () => void;
    setError: () => void;
    resetStatus: () => void;
    withFeedback: <T>(promise: Promise<T>) => Promise<T>;
}

/**
 * Custom hook to manage feedback status and loading state
 * @param resetDelay Time in ms after which the feedback status will be reset to "none"
 * @returns Object with loading state, feedback status, and methods to manage them
 */
export const useFeedbackStatus = (resetDelay = 3000): UseFeedbackStatusResult => {
    const [isLoading, setIsLoading] = useState(false)
    const [feedbackStatus, setFeedbackStatus] = useState<FeedbackStatus>("none")

    const setLoading = useCallback((loading: boolean) => {
        setIsLoading(loading)
    }, [])

    const resetStatus = useCallback(() => {
        setFeedbackStatus("none")
    }, [])

    const setSuccess = useCallback(() => {
        setFeedbackStatus("success")
        setTimeout(resetStatus, resetDelay)
    }, [resetDelay, resetStatus])

    const setError = useCallback(() => {
        setFeedbackStatus("error")
        setTimeout(resetStatus, resetDelay)
    }, [resetDelay, resetStatus])

    const withFeedback = useCallback(async <T, >(promise: Promise<T>): Promise<T> => {
        setIsLoading(true)
        try {
            const result = await promise
            setSuccess()
            return result
        } catch (error) {
            setError()
            throw error
        } finally {
            setIsLoading(false)
        }
    }, [setSuccess, setError])

    return {
        isLoading,
        feedbackStatus,
        setLoading,
        setSuccess,
        setError,
        resetStatus,
        withFeedback,
    }
}
