using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JsonChecker
{
    using Extensions;

    public class JsonDeserializer : IDisposable
    {
        public List<JsonElement> Elements { get; } = new();


        public void Deserialize(string json)
        {
            this.Dispose();

            try
            {
                using var reader = GetXml(json);
                var xml = XElement.Load(reader);
                this.Deserialize(
                    xml, string.Empty,
                    new JsonElement(
                        string.Empty,
                        new NameRegister(),
                        JsonElementType.Standard,
                        xml.Value
                    )
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
            => this.Elements.Clear();

        private string Deserialize(XElement xElement, string name, in JsonElement element, int counter = 0)
        {
            var jsonType = xElement.GetJsonType(Consts.TypeAttribute);
            if (jsonType == JsonType.Object)
            {
                return this.DeserializeObject(xElement, name, element, 0);
            }
            if (jsonType == JsonType.Array)
            {
                return this.DeserializeArray(xElement, name, in element, counter);
            }
            if (counter > 0)
            {
                return xElement.Value;
            }

            this.Elements.Add(new JsonElement(
                name,
                element.Register,
                element.Type,
                xElement.Value
            ));

            return xElement.Value;
        }

        private string DeserializeObject(XElement xElements, string name, in JsonElement element, int counter = 0)
        {
            element.Register.Regist(name);
            var elements = xElements.Elements().ToList();

            if (CheckSpecifyObject(
                elements, out var v, Consts.Keys.ColorR, Consts.Keys.ColorG, Consts.Keys.ColorB)
            )
            {
                this.Elements.Add(new JsonElement(
                    string.Empty,
                    new NameRegister(element.Register),
                    JsonElementType.Color,
                    JsonElement.GetColorValue(v[0], v[1], v[2])
                ));
            }
            else if (CheckSpecifyObject(
                elements, out v, Consts.Keys.X, Consts.Keys.Y, Consts.Keys.Width, Consts.Keys.Height)
            )
            {
                this.Elements.Add(new JsonElement(
                    string.Empty,
                    new NameRegister(element.Register),
                    JsonElementType.Rect,
                    JsonElement.GetRectValue(v[0], v[1], v[2], v[3])
                ));
            }

            foreach (var e in elements)
            {
                this.Deserialize(e, e.Name.ToString(), new JsonElement(element), counter);
            }

            return string.Empty;
        }

        private string DeserializeArray(XElement array, string name, in JsonElement element, int counter = 0)
        {
            element.Register.Regist(name);

            var val = "";
            foreach (var e in array.Elements())
            {
                val += this.Deserialize(
                    e, string.Empty, new JsonElement(element), counter + 1
                ) + Consts.Separator;
            }

            if ((val = val.TrimEnd(Consts.Separator)).Length <= 0)
            {
                return val;
            }

            val = "[" + val + "]";
            if (counter != 0)
            {
                return val;
            }

            this.Elements.Add(new JsonElement(
                string.Empty,
                new NameRegister(element.Register),
                element.Type,
                val
            ));

            return val;
        }

        private static bool CheckSpecifyObject(List<XElement> elements, out string[] result, params string[] keys)
        {
            result = Array.Empty<string>();
            if (elements.Count < keys.Length)
            {
                return false;
            }

            var indexs = new List<int>();
            var values = new string[keys.Length];
            for (var i = 0; i < elements.Count; i++)
            {
                var element = elements[i];

                for (var j = 0; j < keys.Length; j++)
                {
                    if (element.Name.LocalName == keys[j])
                    {
                        values[j] = element.Value;
                        indexs.Add(i);
                    }
                }
            }

            if (indexs.Count < keys.Length)
            {
                return false;
            }

            elements.Remove(indexs);
            result = values;

            return true;
        }

        private static XmlDictionaryReader GetXml(string json)
            => JsonReaderWriterFactory.CreateJsonReader(
                Encoding.Unicode.GetBytes(json), XmlDictionaryReaderQuotas.Max
            );
    }
}
