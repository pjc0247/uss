using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UssColorModifier
{
    [UssModifierKey("color")]
    public void Apply(Graphic g, UssValue value)
    {
        var colorValue = value as UssColorValue;
        if (colorValue == null)
            throw new UssModifierException("UssColorModifier", value, typeof(UssColorValue));

        g.color = colorValue.value;
    }
}
