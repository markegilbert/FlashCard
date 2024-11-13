
namespace DataImporter
{
    public static class ArgsExtensions
    {
        public static String ExtractValueFor(this String[] Args, String ArgumentName)
        {
            return ExtractValueFor(Args, ArgumentName, false);
        }

        public static String ExtractValueFor(this String[] Args, String ArgumentName, bool ValueIsRequired)
        {
            ArgumentName = (ArgumentName ?? "").Trim();
            if (String.IsNullOrEmpty(ArgumentName)) { throw new ArgumentException($"The parameter '{nameof(ArgumentName)}' was null or otherwise invalid"); }


            for (int i = 0; i < Args.Length - 1; i++)
            {
                if (Args[i].Equals($"--{ArgumentName}", StringComparison.CurrentCultureIgnoreCase)) { return Args[i + 1]; }
            }

            if (ValueIsRequired) { throw new ArgumentMissingException(ArgumentName); }
            return "";
        }
    }
}
