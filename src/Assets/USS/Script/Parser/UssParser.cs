using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UssParser
{
    private enum ParsingState
    {
        Conditions,
        Properties
    }
    private enum PropertyParsingState
    {
        Key, Colon, Value
    }

    private ParsingState state;
    private PropertyParsingState propertyState;

    private List<UssToken> tokens;

    private UssStyleDefinition current;
    private List<UssStyleCondition> conditions;
    private List<UssStyleProperty> properties;

    private int cur = 0;
    private string propertyKey;

    public static UssStyleDefinition[] Parse(string src)
    {
        return new UssParser().ParseAll(UssLexer.Parse(src));
    }

    private UssParser()
    {
        state = ParsingState.Conditions;
        propertyState = PropertyParsingState.Key;
    }

    public UssStyleDefinition[] ParseAll(UssToken[] _tokens)
    {
        tokens = new List<UssToken>(_tokens);
        var result = new List<UssStyleDefinition>();

        current = new UssStyleDefinition();
        conditions = new List<UssStyleCondition>();
        properties = new List<UssStyleProperty>();

        for (cur = 0; cur < tokens.Count; cur++)
        {
            var token = tokens[cur];

            if (state == ParsingState.Conditions)
                ParseConditions(token);
            else if (state == ParsingState.Properties)
            {
                if (ParseProperties(token))
                {
                    current.conditions = conditions.ToArray();
                    current.properties = properties.ToArray();
                    result.Add(current);
                    current = new UssStyleDefinition();
                    conditions = new List<UssStyleCondition>();
                    properties = new List<UssStyleProperty>();
                }
            }
        }

        return result.ToArray();
    }

    private void WasteNextToken()
    {
        cur++;
    }
    private UssToken GetNextToken()
    {
        return tokens[cur + 1];
    }

    private void ParseConditions(UssToken token)
    {
        if (token.IsIgnoreable) return;

        if (token.type == UssTokenType.LeftBracket)
        {
            state = ParsingState.Properties;
            return;
        }

        var condition = token.body;
        // CLASS
        if (condition[0] == '.')
        {
            conditions.Add(new UssStyleCondition()
            {
                target = UssStyleTarget.Class,
                name = condition.Substring(1)
            });
        }
        // NAME
        else if (condition[0] == '#')
        {
            conditions.Add(new UssStyleCondition()
            {
                target = UssStyleTarget.Name,
                name = condition.Substring(1)
            });
        }
        else
        {
            conditions.Add(new UssStyleCondition()
            {
                target = UssStyleTarget.Component,
                name = condition
            });
        }
    }
    private bool ParseProperties(UssToken token)
    {
        if (token.IsIgnoreable) return false;

        if (propertyState == PropertyParsingState.Key &&
            token.type == UssTokenType.RightBracket)
        {
            state = ParsingState.Conditions;
            return true;
        }

        if (propertyState == PropertyParsingState.Key)
        {
            if (token.type == UssTokenType.Id)
            {
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
                properties.Add(new UssStyleProperty()
                {
                    key = propertyKey,
                    value = UssValue.Create(token)
                });
            }
            else
                throw new InvalidOperationException("Invalid token `" + token.body + "`. (expected Value)");

            if (GetNextToken().type == UssTokenType.SemiColon)
                WasteNextToken();

            propertyState = PropertyParsingState.Key;
        }

        return false;
    }
}