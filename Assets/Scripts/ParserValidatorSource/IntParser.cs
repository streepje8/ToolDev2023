using UnityEngine;

[CreateAssetMenu(order = 1,fileName = "New Int Parser", menuName = "Parsers/IntParser")]
public class IntParser : InputParser, IInputParser<int>
{
    public int Parse(string from)
    {
        if (int.TryParse(from, out int result))
        {
            return result;
        }
        Debug.LogWarning("Failed to parse an int!");
        return 0;
    }
}
