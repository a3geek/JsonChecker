JsonChecker
===


## Description
This library is for use on Unity.

JsonChecker is a library for parsing JSON to obtain a list of the element names and values it contains.
The library uses when to verify a JSON from communicating with other applications.

JsonChecker contains a program that parses the members in the class type for JSON serialization.
Using this program, you can check for discrepancies between the element names of the received JSON and the members of the class type for serialization.

## Example
JSON
```` csharp
{
  "value": 1.1,
  "unknown_test": "unknown_test",
  "color_list_1": [ "red1", "green1", "blue1" ],
  "color_list_2": [ "green2", "blue2", "red2" ],
  "rect_test": {
    "height": 0.11,
    "width": 0.22,
    "x": 0.33,
    "y": 0.44,
    "index": 1
  },
  "color_test": {
    "r": 250,
    "g": 240,
    "b": 230
  },
  "test_obj_array": [
    {
      "id": 0,
      "info": {
        "name": "test0",
        "rate": 0.0
      }
    },
    {
      "id": 1,
      "info": {
        "name": "test1",
        "rate": 1.1
      }
    }
  ],
  "rects": {
    "color_test_in_rects": {
      "b": 1,
      "g": 2,
      "r": 3
    },
    "one": {
      "x": 0.3,
      "y": 0.4,
      "height": 0.1,
      "width": 0.2
    },
    "two": {
      "height": 0.5,
      "width": 0.6,
      "x": 0.7,
      "y": 0.8
    },
    "count": 2
  }
}
````

Result
````
value => 1.1
<color=red>unknown_test => unknown_test</color>
color_list_1 => [red1,green1,blue1]
color_list_2 => [green2,blue2,red2]
rect_test => 0.33,0.44,0.22,0.11
rect_test/index => 1
color_test => 250,240,230
test_obj_array/id => 0
test_obj_array/info/name => test0
test_obj_array/info/rate => 0.0
test_obj_array/id => 1
test_obj_array/info/name => test1
test_obj_array/info/rate => 1.1
rects/color_test_in_rects => 3,2,1
rects/one => 0.3,0.4,0.2,0.1
rects/two => 0.7,0.8,0.6,0.5
rects/count => 2
````

Class type for JSON serialization
```` csharp
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
````

## Usage
Add the following address to UnityPackageManager gitURL.
````
https://github.com/a3geek/JsonChecker.git?path=Packages/com.a3geek.jsonchecker
````

Sample code
```` csharp
using JsonChecker;

public class Example : MonoBehaviour
{
    [SerializeField, TextArea]
    private string json = "";

    
    void Start()
    {
        // Deserialize json.
        var deserializer = new JsonDeserializer();
        deserializer.Deserialize(this.json);

        // Parse the members in the class type for JSON serialization.
        var fieldNames = new FieldNames(typeof(RootObject));

        // Check for discrepancies between JSON element names and members of the class type for serialization.
        foreach (var e in deserializer.Elements)
        {
            var label = e.Register.Combine(e.Name, JsonChecker.Consts.Slash);
            var isUnknown = !fieldNames.Fields.Contains(label);

            var log = label + " => " + e.Value;
            var rich = isUnknown ?
                $"<color=red>{log}</color>"
                : log;
            Debug.Log(rich);
        }
    }
}
````
If you want to view more details, Let's check Example codes.  
[Example codes](Assets/JsonChecker/Examples/)

## Behaviour
- If a member of the class for JSON serialization has a different class type, the member list is parsed recursively

- If elements named r, g, b, are parsed as Color type
  
- If elements named x, y, width, height, are parsed as Rect type

