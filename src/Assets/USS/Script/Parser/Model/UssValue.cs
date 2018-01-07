using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UssValue
{
    public static object Create(UssToken token)
    {
        if (token.IsValue == false)
            throw new ArgumentException("token is not a value");

        if (token.type == UssTokenType.Null)
            return new UssNullValue();
        if (token.type == UssTokenType.Int)
            return new UssIntValue() { value = int.Parse(token.body) };
        if (token.type == UssTokenType.Float)
            return new UssFloatValue() { value = float.Parse(token.body) };
        if (token.type == UssTokenType.Id)
            return new UssStringValue() { value = token.body };
        if (token.type == UssTokenType.HexColor) {
            Color color;
            if (ColorUtility.TryParseHtmlString(token.body, out color))
                return new UssColorValue() { value = color };
            else
                throw new UssParsingException("Invalid color format: " + token.body);
        }

        throw new InvalidOperationException("Unknown type: " + token.type);
    }
}
public class UssValueBase<T> : UssValue
{
    public T value;
}
public static class UssValueExt
{
    public static string AsString(this UssValue v)
    {
        if (v.GetType() == typeof(UssStringValue))
            return ((UssStringValue)(object)v).value;
        if (v.GetType() == typeof(UssNullValue))
            return null;

        throw new InvalidOperationException("Value cannot be string: " + v.GetType());
    }
    public static float AsFloat(this UssValue v)
    {
        if (v.GetType() == typeof(UssFloatValue))
            return ((UssFloatValue)(object)v).value;
        if (v.GetType() == typeof(UssIntValue))
            return ((UssIntValue)(object)v).value;

        throw new InvalidOperationException("Value cannot be float: " + v.GetType());
    }
}
public class UssNullValue : UssValueBase<object>
{
}
public class UssStringValue : UssValueBase<string>
{
}
public class UssIntValue : UssValueBase<int>
{
}
public class UssFloatValue : UssValueBase<float>
{
}
public class UssColorValue : UssValueBase<Color32>
{
}