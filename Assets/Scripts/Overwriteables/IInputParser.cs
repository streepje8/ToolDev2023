using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputParser<out T>
{
    public T Parse(string from);
}
