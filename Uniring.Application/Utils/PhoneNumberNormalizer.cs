using PhoneNumbers;

namespace Uniring.Application.Utils
{
    public static class PhoneNumberNormalizer
    {
        private static readonly PhoneNumberUtil _util = PhoneNumberUtil.GetInstance();

        /// <summary>
        /// Normalize a phone number to E.164 given a defaultRegion (ISO 3166-1 alpha-2, "IR").
        /// Returns null if the number is not a possible/valid number.
        /// </summary>
        public static string? ToE164(string input, string defaultRegion = "IR")
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            try
            {
                var parsed = _util.Parse(input, defaultRegion);
                if (!_util.IsPossibleNumber(parsed) || !_util.IsValidNumber(parsed))
                    return null;

                return _util.Format(parsed, PhoneNumberFormat.E164);
            }
            catch (NumberParseException)
            {
                return null;
            }
        }
    }
}
