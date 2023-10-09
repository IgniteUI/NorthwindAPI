namespace NorthwindCRUD.Tests
{
    public static class EnumerableExtensions
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("The array cannot be null or empty.");
            }

            Random random = new Random();
            int randomIndex = random.Next(array.Length);
            return array[randomIndex];
        }
    }
}
