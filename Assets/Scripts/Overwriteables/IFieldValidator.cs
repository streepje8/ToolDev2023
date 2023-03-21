using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldValidator<T>
{
    public T Invoke(T input);
}
