Syntax
====

`.ucss` syntax is oriented from __css__.<br>
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
`.ucss` is consist of __VALUEs__ and __STYLEs__.<br>
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

Bundle
----
__Bundle__ is kind of value which is set of __properties__.<br>

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