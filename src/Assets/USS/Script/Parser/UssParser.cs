using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UssParser
{
    private enum ParsingState
    {
        Root,

        Values,
        Conditions,
        Properties
    }
    private enum ValueParsingState
    {
        Key, Colon, Value, End
    }
    private enum PropertyParsingState
    {
        Key, Colon, Value
    }
    private enum CurrentNodeType
    {
        Bundle,
        Style
    }

    private ParsingState state;
    private ValueParsingState valueState;
    private PropertyParsingState propertyState;

    private List<UssToken> tokens;
    private List<UssStyleDefinition> styles;
    private Dictionary<string, UssValue> values;

    private UssStyleDefinition current;
    private List<UssStyleCondition> conditions;
    private List<UssStyleProperty> properties;
    private List<UssValue> propertyValues;

    private int cur = 0;
    private string valueKey;
    private string propertyKey;

    private CurrentNodeType nodeType;

    public static UssParsingResult Parse(string src)
    {
        return new UssParser().ParseAll(UssLexer.Parse(src));
    }

    private UssParser()
    {
        state = ParsingState.Root;
        propertyState = PropertyParsingState.Key;
        valueState = ValueParsingState.Key;
    }
    public UssParsingResult ParseAll(UssToken[] _tokens)
    {
        tokens = new List<UssToken>(_tokens);
        styles = new List<UssStyleDefinition>();
        values = new Dictionary<string, UssValue>();

        FlushCurrentDifinition();
        for (cur = 0; cur < tokens.Count; cur++)
        {
            var token = tokens[cur];

            if (state == ParsingState.Root)
            {
                if (token.IsIgnoreable) continue;

                if (token.type == UssTokenType.ValueRef)
                {
                    Debug.Log(token.body);
                    state = ParsingState.Values;
                    valueState = ValueParsingState.Key;
                }
                else
                    state = ParsingState.Conditions;
            }

            if (state == ParsingState.Values)
                ParseValues(token);
            else if (state == ParsingState.Conditions)
                ParseConditions(token);
            else if (state == ParsingState.Properties)
            {
                if (ParseProperties(token))
                    FlushCurrentDifinition();
            }
        }

        return new UssParsingResult()
        {
            styles = styles.ToArray(),
            values = values
        };
    }

    private void FlushCurrentDifinition()
    {
        if (current != null)
        {
            if (nodeType == CurrentNodeType.Bundle)
            {
                values[valueKey] = new UssBundleValue() {
                    value = properties.ToArray()
                };
            }
            else
            {
                current.conditions = conditions.ToArray();
                current.properties = properties.ToArray();
                styles.Add(current);
            }
        }

        current = new UssStyleDefinition();
        conditions = new List<UssStyleCondition>();
        properties = new List<UssStyleProperty>();
    }
    private void WasteNextToken()
    {
        cur++;
    }
    private UssToken GetNextToken()
    {
        return tokens[cur + 1];
    }

    private void ParseValues(UssToken token)
    {
        if (token.IsIgnoreable) return;

        if (token.type == UssTokenType.SemiColon)
        {
            if (valueState != ValueParsingState.End)
                throw new UssUnexpectedTokenException(token);

            state = ParsingState.Root;
            return;
        }

        if (valueState == ValueParsingState.Key)
        {
            if (token.type != UssTokenType.ValueRef)
                throw new UssUnexpectedTokenException(token, UssTokenType.ValueRef);

            valueKey = token.body.Substring(1);
            valueState = ValueParsingState.Colon;
        }
        else if (valueState == ValueParsingState.Colon)
        {
            if (token.type == UssTokenType.LeftBracket)
            {
                state = ParsingState.Values;
                nodeType = CurrentNodeType.Style;
            }
            else if (token.type == UssTokenType.Colon) 
                valueState = ValueParsingState.Value;
            else
                throw new UssUnexpectedTokenException(token);
        }
        else if (valueState == ValueParsingState.Value)
        {
            if (token.IsValue == false)
                throw new UssUnexpectedTokenException(token);

            values.Add(valueKey, UssValue.Create(token));
            valueState = ValueParsingState.End;
        }
    }
    private void ParseConditions(UssToken token)
    {
        if (token.IsIgnoreable) return;

        if (token.type == UssTokenType.LeftBracket)
        {
            state = ParsingState.Properties;
            nodeType = CurrentNodeType.Style;
            return;
        }

        // Every types of token can be accepted here
        // since name of the Unity's gameobject can contain almost characters.
        // (ex: Zu!ZU##!)
        var rawCondition = token.body;
        var styleCondition = new UssStyleCondition();

        // CLASS
        if (rawCondition[0] == '.')
        {
            styleCondition.target = UssStyleTarget.Class;
            styleCondition.name = rawCondition.Substring(1);
        }
        // NAME
        else if (rawCondition[0] == '#')
        {
            styleCondition.target = UssStyleTarget.Name;
            styleCondition.name = rawCondition.Substring(1);
        }
        else
        {
            styleCondition.target = UssStyleTarget.Component;
            styleCondition.name = rawCondition;
        }

        conditions.Add(styleCondition);
    }
    private bool ParseProperties(UssToken token)
    {
        if (token.IsIgnoreable) return false;

        if (propertyState == PropertyParsingState.Key &&
            token.type == UssTokenType.RightBracket)
        {
            state = ParsingState.Root;
            return true;
        }

        if (propertyState == PropertyParsingState.Key)
        {
            if (token.type == UssTokenType.Id)
            {
                propertyValues = new List<UssValue>();

                propertyKey = token.body;
                propertyState = PropertyParsingState.Colon;
            }
            else
                throw new InvalidOperationException("Invalid token `" + token.body + "`. (expected Id)");
        }
        else if (propertyState == PropertyParsingState.Colon)
        {
            if (token.type != UssTokenType.Colon)
                throw new InvalidOperationException("Invalid token `" + token.body + "`. (expected Colon)");

            propertyState = PropertyParsingState.Value;
        }
        else
        {
            if (token.IsValue)
            {
                propertyValues.Add(UssValue.Create(token));
            }
            else if (token.type == UssTokenType.SemiColon)
            {
                properties.Add(new UssStyleProperty()
                {
                    key = propertyKey,
                    values = propertyValues.ToArray()
                });

                propertyState = PropertyParsingState.Key;
            }
            else
                throw new InvalidOperationException("Invalid token `" + token.body + "`. (expected Value)");
        }

        return false;
    }
}