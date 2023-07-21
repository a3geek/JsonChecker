namespace JsonChecker
{
    public enum JsonElementType
    {
        Standard = 1, Rect, Color
    }

    public class JsonElement
    {
        public string Name { get; } = string.Empty;
        public NameRegister Register { get; } = new();
        public JsonElementType Type { get; } = JsonElementType.Standard;
        public string Value { get; } = string.Empty;


        public JsonElement(string name, NameRegister register, JsonElementType type, string value)
        {
            this.Name = name;
            this.Register = register;
            this.Type = type;
            this.Value = value;
        }

        public JsonElement(JsonElement other)
            : this(other.Name, new NameRegister(other.Register), other.Type, other.Value)
        {
        }

        public static string GetRectValue(string x, string y, string width, string height)
            => x + Consts.Separator + y + Consts.Separator + width + Consts.Separator + height;

        public static string GetColorValue(string r, string g, string b)
            => r + Consts.Separator + g + Consts.Separator + b;
    }
}
