using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
namespace RomDev
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeReference] public List<TKey> keys = new List<TKey>();
        [SerializeReference] public List<TValue> values = new List<TValue>();
        public bool forbidDeserialize;
        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this) 
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load the dictionary from lists
        public void OnAfterDeserialize()
        {
            if(forbidDeserialize)
            {
                return;
            }
            this.Clear();
            if (keys.Count != values.Count) 
            {
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                    + keys.Count + ") does not match the number of values (" + values.Count 
                    + ") which indicates that something went wrong");
            }

            for (int i = 0; i < keys.Count; i++) 
            {
                this.Add(keys[i], values[i]);
            }
        }
    }
    // [System.Serializable]
    // public class SerializableDictionaryV2<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    // {
    //     [SerializeField] private List<TKey> keys = new List<TKey>();
    //     [SerializeField] private List<TValue> values = new List<TValue>();
    //     // save the dictionary to lists
    //     public void OnBeforeSerialize()
    //     {
    //         keys.Clear();
    //         values.Clear();
    //         foreach (KeyValuePair<TKey, TValue> pair in this) 
    //         {
    //             keys.Add(pair.Key);
    //             values.Add(pair.Value);
    //         }
    //     }

    //     // load the dictionary from lists
    //     public void OnAfterDeserialize()
    //     {
    //         this.Clear();
    //         if (keys.Count != values.Count) 
    //         {
    //             Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
    //                 + keys.Count + ") does not match the number of values (" + values.Count 
    //                 + ") which indicates that something went wrong");
    //         }

    //         for (int i = 0; i < keys.Count; i++) 
    //         {
    //             this.Add(keys[i], values[i]);
    //         }
    //     }
    // }
    [System.Serializable]
    public class DictionaryBridgeL1<Tkey, Tvalue>
    {
        public List<Tkey> keys = new List<Tkey>();
        [SerializeField] public List<Tvalue> values = new List<Tvalue>();
        #region Editor Variables
        [HideInInspector] public bool isSelected;
        [HideInInspector] public string varName;
        #endregion
        public bool LinkDictionary(Dictionary<Tkey, Tvalue> serializableDict)
        {
            if(keys.Count != values.Count)
            {
                Debug.LogError("Different element count between keys and value ! ");
                return false;
            }
            Tkey comparedvalue = default(Tkey);
            foreach(Tkey tkey in keys)
            {
                if (tkey.Equals(comparedvalue))
                {
                    Debug.LogError("Same key value detected ! ");
                    return false;
                }
            }
            for(int i = 0; i < keys.Count ; i++)
            {
                serializableDict.Add(keys[i], values[i]);
            }
            return true;
        } 
        public void AddData(Tkey _key, Tvalue _value)
        {
            keys.Add(_key);
            values.Add(_value);
        }
        public void SetValue(Tkey _key, Tvalue _value)
        {
            int keyIndex = keys.IndexOf(_key);
            if(keyIndex < 0 )
            {
                return;
            }
            values[keyIndex] = _value;
        }
        public void RemoveData(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if(keyIndex < 0 )
            {
                return;
            }
            keys.RemoveAt(keyIndex);
            values.RemoveAt(keyIndex);
        }
        public bool ContainsKey(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if( keyIndex < 0 )
            {
                // Debug.LogWarning("Can't find data");
                return false;
            }
            return true;
        }
        public Tvalue GetData(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if( keyIndex < 0 )
            {
                Debug.LogWarning("Can't find data");
                return default;
            }
            return values[keyIndex];
        }
        public bool TryGetValue(Tkey _key, out Tvalue result)
        {
            int keyIndex = keys.IndexOf(_key);
            if( keyIndex < 0 )
            {
                Debug.LogWarning("Can't find data");
                result = default;
                return false;
            }
            result = values[keyIndex];
            return true;
        }
        public Tvalue GetCopy(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if( keyIndex < 0 )
            {
                Debug.LogWarning("Can't find data");
                return default;
            }
            return AdvGame.DeepCopy(values[keyIndex]);
        }
        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }
        public Dictionary<Tkey, Tvalue> GetAsDictionary()
        {
            Dictionary<Tkey, Tvalue> dict = new();
            for (int i = 0; i < keys.Count; i++)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }
        #if UNITY_EDITOR
        public void DrawProperty(SerializedProperty parentPath)
        {
            string foldoutName = "Property";
            if(varName != null && varName != "")
            {
                foldoutName = varName;
            }
            isSelected = EditorGUILayout.Foldout(isSelected, foldoutName);
            if(isSelected)
            {
                SerializedProperty keysProp = parentPath.FindPropertyRelative("keys");
                SerializedProperty valuesProp = parentPath.FindPropertyRelative("values");
                EditorGUILayout.PropertyField(keysProp, true);
                if(typeof(IPropertyDrawerL2).IsAssignableFrom(values.GetType().GetGenericArguments().Single()))
                {
                    List<IPropertyDrawerL2> valuePropDrawers = new();
                    // int i = 0;
                    foreach (Tvalue childValue in values) 
                    {
                        // Debug.Log("Prop draw : " + i);
                        if(childValue is IPropertyDrawerL2 childValueDrawer)
                        {
                            valuePropDrawers.Add(childValueDrawer);
                        }
                        // i++; 
                    }
                    int a = 0;
                    foreach (SerializedPack<IPropertyDrawerL2> serializedPack in EditorHelper.GetPairSerializedPack<IPropertyDrawerL2, List<IPropertyDrawerL2>>(valuesProp, valuePropDrawers))
                    {
                        IPropertyDrawerL2 serializedDrawer = serializedPack.packValue;
                        serializedDrawer.DrawPropertyL2(serializedPack.serializedProperty, a);
                        a++;
                    }
                }else
                {
                    EditorGUILayout.PropertyField(valuesProp, true);
                }
            }
        }
        #endif
    }
    [Serializable]
    public class DictionaryBridgeL2<Tkey, Tvalue>
    {
        public List<Tkey> keys = new List<Tkey>();
        [SerializeReference] public List<Tvalue> values = new List<Tvalue>();
        #region Editor Variables
        [HideInInspector] public bool isSelected;
        [HideInInspector] public string varName;
        #endregion
        public bool LinkDictionary(SerializableDictionary<Tkey, Tvalue> serializableDict)
        {
            if(keys.Count != values.Count)
            {
                Debug.LogError("Different element count between keys and value ! ");
                return false;
            }
            Tkey comparedvalue = default(Tkey);
            foreach(Tkey tkey in keys)
            {
                if (tkey.Equals(comparedvalue))
                {
                    Debug.LogError("Same key value detected ! ");
                    return false;
                }
            }
            for(int i = 0; i < keys.Count ; i++)
            {
                serializableDict.Add(keys[i], values[i]);
            }
            return true;
        } 
        public void AddData(Tkey _key, Tvalue _value)
        {
            keys.Add(_key);
            values.Add(_value);
        }
        public void RemoveData(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if(keyIndex < 0 )
            {
                return;
            }
            keys.RemoveAt(keyIndex);
            values.RemoveAt(keyIndex);
        }
        public Tvalue GetData(Tkey _key)
        {
            int keyIndex = keys.IndexOf(_key);
            if( keyIndex < 0 )
            {
                return default;
            }
            return values[keyIndex];
        }
        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }
    }
}