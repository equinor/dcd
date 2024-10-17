namespace api.Helpers
{
    public static class CollectionUtilities
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> array)
        {
            if (array != null)
            {
                return !array.Any();
            }
            return true;
        }
    }
}