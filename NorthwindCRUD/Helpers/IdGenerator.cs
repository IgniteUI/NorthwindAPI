namespace NorthwindCRUD.Helpers
{
    public static class IdGenerator
    {
        public static string CreateLetterId(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static int CreateDigitsId()
        {
            Random r = new Random();
            return r.Next(10000000, 99999999);
        }
    }
}
