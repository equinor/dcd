import { useMutation, useQueryClient } from "react-query"

interface UseMutationProps {
    queryKey: string[];
    mutationFn: (updatedData: any) => Promise<any>;
}

const useQuery = ({ queryKey, mutationFn }: UseMutationProps) => {
    const queryClient = useQueryClient()

    const mutation = useMutation(
        async (updatedData) => mutationFn(updatedData),
        {
            onSuccess: (results) => {
                queryClient.setQueryData(queryKey, results)
                console.log("API Response:", results)
            },
            onError: (error) => {
                console.error("Failed to update data:", error)
            },
            onSettled: () => {
                queryClient.invalidateQueries(queryKey)
            },
        },
    )

    const updateData = (key: any, value: any) => {
        const updatedData = { [key]: value }
        mutation.mutate(updatedData as any)
    }

    return { updateData }
}

export default useQuery
