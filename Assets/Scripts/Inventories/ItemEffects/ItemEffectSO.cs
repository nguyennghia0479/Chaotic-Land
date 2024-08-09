using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectSO : ScriptableObject
{
    [TextArea] public string itemDes;

    public virtual void ExecuteItemEffect(Transform _target)
    {

    }
}
