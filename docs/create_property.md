Create custom property
====

You can make your custom properties.<br>
<br>
Here's most simple code which is already contained in __uss__.
```csharp
public class UssTextModifier
{
    [UssModifierKey("font-size")]
    public void ApplyFontSize(Text g, UssValue value)
    {
        g.fontSize = value.AsInt();
    }
}
```

You don't need to inspect value's type or casting at all.<br>
Just use `AsInt()`, this already contains type-checking logic.


UssValue
----
```csharp
UssValue v;

string s = v.AsString();
int i = v.AsInt();
float f = v.AsFloat();
Color c = v.AsColor(); 
```
```csharp
if (v.IsNone())
    ; // none
```

Register
----
```csharp
UssStyleModifier.LoadModifier<UssTextModifier>();
```