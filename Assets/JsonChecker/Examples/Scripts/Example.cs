using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JsonChecker.Examples
{
    public class Example : MonoBehaviour
    {
        [SerializeField, TextArea]
        private string json = "";
        [SerializeField]
        private Text jsonText = null;
        [SerializeField]
        private Text resultText = null;


        void Start()
        {
            var deserializer = new JsonDeserializer();
            deserializer.Deserialize(this.json);

            var fieldNames = new FieldNames(typeof(RootObject));

            var result = "";
            foreach (var e in deserializer.Elements)
            {
                var label = e.Register.Combine(e.Name, JsonChecker.Consts.Slash);
                var isUnknown = !fieldNames.Fields.Contains(label);

                var log = label + " => " + e.Value;
                var rich = isUnknown ?
                    $"<color=red>{log}</color>"
                    : log;
                Debug.Log(rich);
                result += rich + "\n";
            }

            this.jsonText.text = this.json;
            this.resultText.text = result;
        }

        [Serializable]
        public class RootObject
        {
            public Rects rects;
            public List<string> color_list_1;
            public string[] color_list_2;
            public RectTest rect_test;
            public ColorTest color_test;
            public TestObjArray[] test_obj_array;

            [SerializeField]
            private float value;
        }

        [Serializable]
        public class RectTest
        {
            public int index;
            public float height;
            public float width;
            public float x;
            public float y;
        }

        public class ColorTest
        {
            public int r;
            public int g;

            private int b;
        }

        [Serializable]
        public class TestObjArray
        {
            public int id;
            public Info info;
        }

        [Serializable]
        public class Info
        {
            public string name;
            public float rate;
        }

        [Serializable]
        public class Rects
        {
            public ColorTestInRects color_test_in_rects;
            public One one;
            public Two two;
            public int count;
        }

        [Serializable]
        public class ColorTestInRects
        {
            public int r;
            public int g;
            public int b;
        }

        [Serializable]
        public class One
        {
            public float x;
            public float y;
            public float height;
            public float width;
        }

        [Serializable]
        public class Two
        {
            public float height;
            public float width;
            public float x;
            public float y;
        }
    }
}
