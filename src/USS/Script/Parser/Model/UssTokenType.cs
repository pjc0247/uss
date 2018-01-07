using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UssTokenType
{
    None,
    Whitespace,
    CrLf,

    LeftBracket, RightBracket,
    Colon, SemiColon,
    Comma,

    Id,
    Null,

    Int, Float, HexColor
}