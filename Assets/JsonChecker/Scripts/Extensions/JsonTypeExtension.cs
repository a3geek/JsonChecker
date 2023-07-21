using System.Xml.Linq;

namespace JsonChecker.Extensions
{
    public static class JsonTypeExtension
    {
        public static JsonType ToJsonType(this string type)
        {
            return type switch
            {
                "string" => JsonType.String,
                "number" => JsonType.Number,
                "boolean" => JsonType.Boolean,
                "object" => JsonType.Object,
                "array" => JsonType.Array,
                _ => JsonType.None,
            };
        }

        public static JsonType GetJsonType(this XElement element, string attributeName)
            => element.Attribute(attributeName)?.Value.ToJsonType() ?? JsonType.None;
    }
}
