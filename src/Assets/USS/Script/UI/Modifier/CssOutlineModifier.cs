using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UssOutlineModifier
{
    [UssModifierKey("outline")]
    public void Apply(Graphic g, UssValue[] values)
    {
        if (values[0] is UssStringValue)
        {
            if (values[0].AsString() == "none")
                GameObject.DestroyImmediate(g.GetComponent<Outline>());
        }

        if (values.Length == 1)
        {
            var colorValue = values[0] as UssColorValue;
            if (colorValue == null)
                throw new UssModifierException("UssOutlineModifier", values[0], typeof(UssColorValue));

            var outline = g.GetComponent<Outline>();
            if (outline == null)
                outline = g.gameObject.AddComponent<Outline>();
            outline.effectColor = colorValue.value;
        }
        else if (values.Length == 2)
        {
            var width = values[0].AsFloat();
            var colorValue = values[1] as UssColorValue;
            if (colorValue == null)
                throw new UssModifierException("UssOutlineModifier", values[1], typeof(UssColorValue));

            var outline = g.GetComponent<Outline>();
            if (outline == null)
                outline = g.gameObject.AddComponent<Outline>();
            outline.effectDistance = new Vector2(width, width);
            outline.effectColor = colorValue.value;
        }
    }
}
