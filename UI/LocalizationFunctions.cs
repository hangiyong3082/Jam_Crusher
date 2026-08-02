using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

public static class LocalizationFunctions
{
    public static List<string> GetLocalizationKeys(string tableName)
    {
        List<string> keyList = new List<string>();

        var table = LocalizationSettings.StringDatabase.GetTable(tableName);
        if (table != null)
        {
            foreach (var entry in table.Values)
            {
                keyList.Add(entry.Key);
            }
        }
        else
        {
            Debug.LogWarning("Table not found: " + tableName);
        }

        return keyList;
    }

    static string GetTableName(LocalizedString localizedString)
    {
        return localizedString.TableReference;
    }
    static string GetKey(LocalizedString localizedString)
    {
        var tableEntry = LocalizationSettings.StringDatabase.GetTableEntry(localizedString.TableReference, localizedString.TableEntryReference);
        string key = tableEntry.Entry.SharedEntry.Key;
        return key;
    }

    public static string GetLocalizedString(LocalizedString localizedString)
    {
        string tableName = GetTableName(localizedString);
        string key = GetKey(localizedString);
        string result;
        result = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, key, LocalizationSettings.SelectedLocale);    
        return result;
    }

    public static IEnumerator SynchronizeStringWEvent(LocalizedString savedLocalizedString, LocalizeStringEvent targetLocalizeStringEvent)
    {
        yield return LocalizationSettings.InitializationOperation;

        targetLocalizeStringEvent.SetTable(GetTableName(savedLocalizedString));
        targetLocalizeStringEvent.SetEntry(GetKey(savedLocalizedString));

        try
        {
            var variable = savedLocalizedString["variable"] as StringVariable;
            (targetLocalizeStringEvent.StringReference["variable"] as StringVariable).Value = variable.Value;
        }
        catch { }

        targetLocalizeStringEvent.RefreshString();


    }

    public static void UpdateVariable(LocalizeStringEvent targetLocalizeStringEvent, string value)
    {
        (targetLocalizeStringEvent.StringReference["variable"] as StringVariable).Value = value;
    }
}
