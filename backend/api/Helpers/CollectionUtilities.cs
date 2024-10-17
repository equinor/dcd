namespace API.Helpers
{
    public static class CollectionUtilities
    {
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            if (array != null)
            {
                return !array.Any();
            }
            return true;
        }
    }
}