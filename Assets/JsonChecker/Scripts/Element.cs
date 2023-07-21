using UnityEngine;

namespace JsonChecker
{
    using Extensions;

    public readonly ref struct Element
    {
        public readonly string Name;
        public readonly string Label;
        public readonly JsonElementType Type;
        public readonly string Value;
        public readonly bool IsUnknown;


        public Element(string name, string label, JsonElementType type, string value, bool isUnknown)
        {
            this.Name = name;
            this.Label = label;
            this.Type = type;
            this.Value = value;
            this.IsUnknown = isUnknown;
        }

        public Rect GetRect()
        {
            var split = this.Value.Split(Consts.Separator);
            return this.Type != JsonElementType.Rect || split.Length < 4
                ? Rect.zero
                : new Rect(
                    split.Get(0).ToFloat(), split.Get(1).ToFloat(),
                    split.Get(2).ToFloat(), split.Get(3).ToFloat()
                );
        }

        public Color GetColor32(bool alpha = false)
        {
            var split = this.Value.Split(Consts.Separator);
            return this.Type != JsonElementType.Color || split.Length < (alpha ? 4 : 3)
                ? Color.black
                : GetColor32(split, alpha);
        }

        private static Color GetColor32(string[] split, bool alpha = false)
            => new Color32(
                split.Get(0).ToByte(), split.Get(1).ToByte(),
                split.Get(2).ToByte(), (byte)(alpha ? split.Get(3).ToByte() : 255)
            );
    }
}
