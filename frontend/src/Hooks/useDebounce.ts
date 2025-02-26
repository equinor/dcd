import {
    useCallback,
    useEffect,
    useRef,
    useState,
} from "react"

export function useDebounce<T>(value: T, delay: number): T | undefined {
    const [debouncedValue, setDebouncedValue] = useState<T>()

    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedValue(value)
        }, delay)

        return () => {
            clearTimeout(timer)
        }
    }, [value, delay])

    return debouncedValue
}

export function useDebouncedCallback<T extends(
    ...args: any[]) => void>(
    callback: T,
    delay: number,
): T {
    const timer = useRef<NodeJS.Timeout | null>(null)

    return useCallback(
        (...args: Parameters<T>) => {
            if (timer.current) {
                clearTimeout(timer.current)
            }
            timer.current = setTimeout(() => {
                callback(...args)
            }, delay)
        },
        [callback, delay],
    ) as T
}
