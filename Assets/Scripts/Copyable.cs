using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Copyable<T>
{
    void Copy(T anotherObject);
}
