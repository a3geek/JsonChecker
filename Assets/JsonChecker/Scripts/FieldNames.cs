using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonChecker
{
    using Extensions;

    public class FieldNames
    {
        public List<string> Fields { get; } = new();


        public FieldNames(Type type)
        {
            this.Fields.AddRange(Get(type, new NameRegister()));
        }

        public static List<string> Get(Type type, NameRegister nameRegister)
        {
            var list = new List<string>();
            var fields = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(info => !info.IsNotSerialized)
                .ToList();

            if (IsContainsColorField(fields))
            {
                list.Add(nameRegister.Combine(string.Empty, Consts.Slash));
            }
            else if (IsContainsRectField(fields))
            {
                fields.ForEach(e => UnityEngine.Debug.Log(e.Name));
                list.Add(nameRegister.Combine(string.Empty, Consts.Slash));
            }

            foreach (var field in fields)
            {
                CheckField(field, list, nameRegister);
            }

            return list;
        }

        private static void CheckField(FieldInfo field, List<string> list, NameRegister nameRegister)
        {
            var type = field.FieldType;
            type = type.IsArray ? type.GetElementType()
                    : (type.IsList() ? type.GetGenericArguments()[0] : type);
            UnityEngine.Debug.Log(type + " : " + field.Name + " => " + nameRegister.Combine(field.Name, Consts.Slash));

            if (type.IsClassOrStruct(stringIsPrimitive: true))
            {
                list.AddRange(Get(type,new NameRegister(nameRegister, field.Name)));
            }
            else
            {
                list.Add(nameRegister.Combine(field.Name, Consts.Slash));
            }
        }

        private static bool IsContainsColorField(List<FieldInfo> fields)
            => CheckSpecifyField(
                fields, typeof(int),
                Consts.Keys.ColorR, Consts.Keys.ColorG, Consts.Keys.ColorB
            );

        private static bool IsContainsRectField(List<FieldInfo> fields)
            => CheckSpecifyField(
                fields, typeof(float),
                Consts.Keys.X, Consts.Keys.Y,
                Consts.Keys.Width, Consts.Keys.Height
            );

        private static bool CheckSpecifyField(List<FieldInfo> fields, Type fieldType, params string[] names)
        {
            var indexs = new List<int>();

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field.FieldType != fieldType)
                {
                    continue;
                }

                if (names.Any(name => field.Name == name))
                {
                    indexs.Add(i);
                }
            }

            if (indexs.Count < names.Length)
            {
                return false;
            }

            fields.Remove(indexs);
            return true;
        }
    }
}
