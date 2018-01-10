using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UssStyleConditionType
{
    None,
    DirectDescendant
}
public class UssStyleCondition
{
    public UssStyleTarget target;
    public UssStyleConditionType type;
    public string name;
}