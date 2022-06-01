using Dominio;

namespace WebAPI
{
    public sealed class Error : ValueObject
    {
        private const string _separator = "||";

        public string Code { get; }
        public string Message { get; }

        internal Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Serialize()
        {
            return $"{Code}{_separator}{Message}";
        }

        public static Error Deserialize(string serialized)
        {
            string[] data = serialized.Split(
                new[] { _separator },
                StringSplitOptions.RemoveEmptyEntries);

            // https://www.c-sharpcorner.com/article/guards-in-net/
            // Guard.Require(data.Length >= 2, $"Invalid error serialization: '{serialized}'");

            return new Error(data[0], data[1]);
        }
    }

    public static class Errors
    {
        public static class General
        {
            public static Error NotFound(string entityName, long id) => 
                new("record.not.found", $"'{entityName}' not found for Id '{id}'");
        }
    }
}
