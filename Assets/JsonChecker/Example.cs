using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Example : MonoBehaviour
{
    [SerializeField, TextArea]
    private string json = "";

    public List<string> fields;

    void Start()
    {
        var deserializer = new JsonChecker.JsonDeserializer();
        deserializer.Deserialize(this.json);

        var fieldNames = new JsonChecker.FieldNames(typeof(RootObject));
        this.fields = fieldNames.Fields;

        fieldNames.Fields.ForEach(e => Debug.Log(e));
        Debug.Log("");
        foreach (var e in deserializer.Elements)
        {
            var label = e.Register.Combine(e.Name, JsonChecker.Consts.Slash);
            var isUnknown = !fieldNames.Fields.Contains(label);

            var log = label + " => " + e.Value;
            var rich = isUnknown ?
                $"<color=red>{log}</color>"
                : log;
            Debug.Log(rich);
            //Debug.Log(e.Name + " : " + label + " :: " + e.Type + " => " + e.Value);
        }
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
