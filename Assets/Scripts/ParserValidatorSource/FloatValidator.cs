using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "New Float Validator", menuName = "Validators/FloatValidator")]
public class FloatValidator : InputValidator,IFieldValidator<float>
{
    public float Invoke(float input)
    {
        return input;
    }
}
