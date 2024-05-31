import { useMutation, useQueryClient } from "react-query"
import { debounce } from "lodash"
import { useState } from "react"

interface UseOptimisticMutationProps {
    queryKey: string[]
    mutationFn: (updatedData: any) => Promise<any>
    initialData?: any
}
const useOptimisticMutation = ({ queryKey, mutationFn, initialData }: UseOptimisticMutationProps) => {
    const queryClient = useQueryClient()
    const [data, setData] = useState(initialData || {})

    const mutation = useMutation(
        async (updatedData) => mutationFn(updatedData),
        {
            onMutate: async (updatedData: object) => {
                await queryClient.cancelQueries(queryKey)

                const previousData = queryClient.getQueryData(queryKey)

                queryClient.setQueryData(queryKey, (oldData: object) => ({
                    ...oldData,
                    ...updatedData,
                }))

                setData((oldData: object) => ({
                    ...oldData,
                    ...updatedData,
                }))

                return { previousData }
            },
            onSuccess: (results) => {
                queryClient.setQueryData(queryKey, results)
                console.log("API Response:", results)
                setData(results)
            },
            onError: (error, updatedData, context) => {
                if (context) {
                    queryClient.setQueryData(queryKey, context.previousData)
                }
                console.error("Failed to update data:", error)
            },
            onSettled: () => {
                queryClient.invalidateQueries(queryKey)
            },
        },
    )

    const debouncedUpdate = debounce((key, value) => {
        const updatedData = { ...data, [key]: value }
        mutation.mutate(updatedData)
    }, 300)

    const updateData = (key: any, value: any) => {
        debouncedUpdate(key, value)
    }

    return { updateData, data }
}

export default useOptimisticMutation
