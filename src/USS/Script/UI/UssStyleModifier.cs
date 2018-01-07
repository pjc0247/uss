using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UssStyleModifier : MonoBehaviour
{
    private class ModifierData
    {
        public object modifier;
        public MethodInfo method;
        public Type acceptedComponent;
    }

    private static List<UssStyleDefinition> styles;
    private static Dictionary<string, ModifierData> modifiers;

    static UssStyleModifier()
    {
        modifiers = new Dictionary<string, ModifierData>();

        // Load default modifiers
        LoadModifier<UssColorModifier>();
    }
    public static void LoadModifier<T>()
        where T : new()
    {
        var obj = new T();

        foreach (var method in typeof(T).GetMethods())
        {
            var attrs = method.GetCustomAttributes(typeof(UssModifierKeyAttribute), true);
            if (attrs.Length == 0)
                continue;

            var key = ((UssModifierKeyAttribute)attrs[0]).key;
            if (modifiers.ContainsKey(key))
                throw new InvalidOperationException("Already has modifier with key: " + key);

            modifiers.Add(key, new ModifierData()
            {
                modifier = obj,
                method = method,
                acceptedComponent = method.GetParameters().First().ParameterType
            });
        }
    }
    public static void LoadUss(string uss)
    {
        Debug.Log("LoadUSS: " + uss);

        styles = new List<UssStyleDefinition>(UssParser.Parse(uss));

        Debug.Log("Loaded " + styles.Count + " style(s).");

        Apply(GameObject.Find("Canvas"));
    }

    public static void Apply(GameObject g)
    {
        if (styles == null)
            throw new InvalidOperationException(".uss file not loaded yet.");

        foreach (var style in styles)
        {
            if (CheckConditions(g, style.conditions) == false)
                continue;

            foreach (var p in style.properties)
            {
                foreach (var m in modifiers)
                {
                    if (p.key != m.Key) continue;

                    var comp = g.GetComponent(m.Value.acceptedComponent);
                    if (comp == null) continue;

                    m.Value.method.Invoke(m.Value.modifier, new object[]{
                        comp, p.value
                    });
                }
            }
        }

        for (int i = 0; i < g.transform.childCount; i++)
            Apply(g.transform.GetChild(i).gameObject);
    }

    private static bool CheckConditions(GameObject g, UssStyleCondition[] conditions)
    {
        foreach (var c in conditions)
        {
            if (c.target == UssStyleTarget.Name)
            {
                if (g.name != c.name)
                    return false;
            }
            else if (c.target == UssStyleTarget.Component)
            {
                if (g.GetComponent(c.name) == null)
                    return false;
            }
            else if (c.target == UssStyleTarget.Class)
            {
                var klass = g.GetComponent<UssClass>();
                if (klass == null)
                    return false;
                if (klass.classes.Split(' ').Contains(c.name) == false)
                    return false;
            }
        }

        return true;
    }
}
