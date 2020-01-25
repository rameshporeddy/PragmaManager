using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PragmaData", menuName = "G2W/PragmaHandler", order = 1)]
public class PragmaScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    [System.Serializable]
    public class PragmaKeyValue
    {
        public string Key;
        public string PragmaPattern;

        public bool IsEnabled;
    }
    public List<PragmaKeyValue> pragmaKeys;

    public void AddPragmaKey(PragmaKeyValue keyValue)
    {
        foreach (PragmaKeyValue n in pragmaKeys)
        {
            if (n.Key.Equals(keyValue.Key) || n.PragmaPattern.Equals(keyValue.PragmaPattern)) return;
        }
        pragmaKeys.Add(keyValue);
    }
    public void SelectDeselectAll(bool enable)
    {
        foreach (PragmaKeyValue keyValue in pragmaKeys)
        {
            keyValue.IsEnabled = enable;
        }
    }

}


