using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UssTextModifier
{
    [UssModifierKey("font-size")]
    public void ApplyFontSize(Text g, UssValue value)
    {
        g.fontSize = (int)value.AsFloat();
    }

    [UssModifierKey("text-align")]
    public void ApplyTextAlign(Text g, UssValue value)
    {
        var align = value.AsString();

        if (align == "left")
            g.alignment = TextAnchor.MiddleLeft;
        else if (align == "center")
            g.alignment = TextAnchor.MiddleLeft;
        else if (align == "right")
            g.alignment = TextAnchor.MiddleLeft;
        else
            throw new ArgumentException("Invalid param: " + align);
    }
}
