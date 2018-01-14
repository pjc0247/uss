Syntax
====

`.ucss`'s syntax is oriented from __css__.<br>
If you can write css code already, you may skip beginning of this section.

Basic Structure
----
```css
// VALUE DEFINITIONS
@foo: 1234;
@bar: #FF0000FF;

// STYLE DEFINITIONS
sel1 #sel2 .sel3 {
    outline: 1;
    color: black;
}
```
`.ucss` file is consist of __VALUEs__ and __STYLEs__.<br>
<br>
You must append semicolon(`;`) every end of syntax.

Value
----
Every value starts with `@`. This makes distictions with other __KEYWORDs__ and __IDs__.  
```css
@NAME: 1234;
```

Type
----
__COLOR__
```css
@color_by_hex: #FF0000FF;
@without_opacity: #FF0000;

@color_by_name: red;
```

__NUMBER__
```css
@integer: 1234;
@float: 123.44;
```

__KEYWORD__
```css
@outline: none;
```

__STRING__
```css
@path: "asdf";
```

Style
----
```css
SELECTOR1 SELECTOR2 {
    PROPERTY-KEY: PROPERTY-VALUE;
}
```
__STYLE__ is a set of properties which describes specified object styles.<br>
<br>
First, you need to specify the object to which you want to apply the style. You can select single or multiple objects with __SELECTOR__.
__SELECTORs__ in __uss__ is very simillar to __css__'s. Here's some rules how to specify objects in __uss__.<br>

__Component selector__

```css
Image {

}
```

__Name selector__

```css
#DateOfMonth {

}
```

__Class selector__

```css
.bold {

}
```

__Special selectors__

```css
* { 
}
```

__Descendant__

```css
Button > Text {

}
```

__State specifier__

```css
Button:hover {

}
```


Bundle
----
__Bundle__ is kind of value which is set of __properties__.<br>
With this syntax, you can simplify the code with many duplicates.

```css
@h1 {
    font-size: 40;
    font-style: bold;
}
```
The example below shows how to import bundle to style.

```css
.title {
    @h1;
}
// above code is exactly same with below.
.title {
    font-size: 40;
    font-style: bold;
}
```

You can also import bundle to bundle:

```css
@foo {
    font-size: 40;
}
@bar {
    font-style: bold;
}
@foo_bar {
    @foo; @bar;
}

.title {
    @foo_bar;
}
```