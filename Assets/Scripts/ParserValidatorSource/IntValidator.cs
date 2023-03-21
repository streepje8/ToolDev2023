using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "New Int Validator", menuName = "Validators/IntValidator")]
public class IntValidator : InputValidator, IFieldValidator<int>
{
    public int Invoke(int input)
    {
        return input;
    }
}
