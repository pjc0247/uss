Create your custom property
----

```csharp
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
```

```csharp
UssStyleModifier.LoadModifier<MyModifier>();
```