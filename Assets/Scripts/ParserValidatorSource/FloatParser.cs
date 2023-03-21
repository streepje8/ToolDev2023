using UnityEngine;

[CreateAssetMenu(order = 1,fileName = "New Float Parser", menuName = "Parsers/FloatParser")]
public class FloatParser : InputParser, IInputParser<float>
{
    public float Parse(string from)
    {
        if (float.TryParse(from, out float f))
        {
            return f;
        }
        Debug.LogWarning("Failed to parse a float!");
        return 0f;
    }
}
