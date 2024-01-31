using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueItem : ItemBase
{
    public int id;

    public int GetId()
    {
        return id;
    }
}
