using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RomDev
{
    public class BasicObjectDataSaver : ConstantID
    {
        public SerializableDictionary<int,ComponentData> CompDatas
        {
            get
			{
				if(compDatas == null)
				{
					compDatas = new();
					compDataBridge.LinkDictionary(compDatas);
					if (Application.isPlaying)
                    {
                        compDataBridge.Clear();
                    }
				}
				return compDatas;
			}
        }
        public SerializableDictionary<Component, int> CompDatas2
        {
            get
			{
				if(compDatas2 == null)
				{
					compDatas2 = new();
					compDataBridge2.LinkDictionary(compDatas2);
					if (Application.isPlaying)
                    {
                        compDataBridge2.Clear();
                    }
				}
				return compDatas2;
			}
        }
        public BasicSaverRequirement basicSaverRequirement;
        private SerializableDictionary<int,ComponentData> compDatas;
        private SerializableDictionary<Component, int> compDatas2;
        [SerializeField] private DictionaryBridgeL1<int,ComponentData> compDataBridge = new();
        [SerializeField] private DictionaryBridgeL1<Component, int> compDataBridge2 = new();
        private const string IsParented = "IsParented",
        ParentID = "ParentID",
        ObjName = "ObjName",
        ObjTag = "ObjTag",
        ObjLayer = "ObjLayer";
        #region Editor Vars
        #if UNITY_EDITOR
        private bool showCompDetail;
        private Vector2 scrollCompData;
        private Vector2 scrollCompData2;
        #endif
        #endregion
        [Serializable]
        public class ComponentData
        {
            public Component comp;
            public SaveRequirement saveRequirement;
        }
        #region Editor Settings
        #if UNITY_EDITOR

		public void ShowGUI ()
		{
			CustomGUILayout.Header ("Basic Data Saver");
			CustomGUILayout.BeginVertical ();
            showCompDetail = EditorGUILayout.Foldout (showCompDetail, "Component Detail");

            if(showCompDetail)
            {
                CustomGUILayout.LabelField("Component Data (ID) : ");
                CustomGUILayout.LabelField("ID || Component Data");
                scrollCompData = GUILayout.BeginScrollView (scrollCompData, false, true, GUILayout.MinHeight(100), GUILayout.MaxHeight(150));
                for (int i = 0; i < compDataBridge.keys.Count; i++)
                {
                    CustomGUILayout.LabelField($"{compDataBridge.keys[i]} :");
                    CustomGUILayout.ObjectField<Component> ("Component: ", compDataBridge.values[i].comp, false);
                    CustomGUILayout.ObjectField<SaveRequirement> ("Save Requirement: ", compDataBridge.values[i].saveRequirement, false);
                }
                GUILayout.EndScrollView ();
                EditorGUILayout.Space();

                CustomGUILayout.LabelField("Component Data (Component) : ");
                CustomGUILayout.LabelField("Component || ID");
                scrollCompData2 = GUILayout.BeginScrollView (scrollCompData2, false, true, GUILayout.MinHeight(100), GUILayout.MaxHeight(150) );
                for (int i = 0; i < compDataBridge2.keys.Count; i++)
                {
                    CustomGUILayout.BeginHorizontal();
                    CustomGUILayout.ObjectField<Component> ("Component: ", compDataBridge2.keys[i], false);
                    CustomGUILayout.LabelField($" || {compDataBridge2.values[i]}");
                    CustomGUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView ();
                EditorGUILayout.Space();
                if(GUILayout.Button("Gather Components"))
                {
                    GatherComponents();
                }
            }

			CustomGUILayout.EndVertical ();
		}

		#endif
        #endregion
        // protected  void OnEnable() {
        //     if(isPrefab && ! keepPrefabID)
        //     {
        //         persistID = Mathf.Abs(GetInstanceID());
        //     }
        //     AutoRegisterSelf();
        // }
        protected override void Reset() {
            // AddPersistentID();
            base.Reset();
            GatherComponents();
        }
        // protected void OnDisable() {
        //     PersistentIDManager.Instance.UnregisterBasicSaver(this);
        // }
        // public void AddPersistentID()
        // {
        //     if(getPersistentID)
        //     {
        //         return;
        //     }
        //     persistID = Mathf.Abs(GetInstanceID());
        //     getPersistentID = true;
        // }
        // public void RegisterSelf()
        // {
        //     PersistentIDManager.Instance.RegisterBasicSaver(this);
        // }
        public static bool TryGetComponentFromBasicServer<T>(BasicObjectDataSaver _basicSaver, int compID, out T targetComp)where T :Component
        {
            targetComp = null;
            if( ! _basicSaver.TryGetObjectComponent(compID, out T resultComp))
            {
                return false;
            }
            targetComp = resultComp;
            return true;
        }
        public SaveBasicData SaveBasicData()
        {
            SaveBasicData saveBasicData = new();
            BasicSaverRequirement sourceRequirement = GetBasicSaverRequirement(); 
            if(sourceRequirement.saveParent)
            {
                if(transform.parent != null)
                {
                    saveBasicData.saveDict.AddData(IsParented, true.ToString());
                    saveBasicData.saveDict.AddData(ParentID, transform.parent.GetComponent<BasicObjectDataSaver>().constantID.ToString());
                }else
                {
                    saveBasicData.saveDict.AddData(IsParented, false.ToString());
                }
            }
            if(sourceRequirement.saveObjName)
            {
                saveBasicData.saveDict.AddData(ObjName, gameObject.name);
            }
            if(sourceRequirement.saveObjTag)
            {
                saveBasicData.saveDict.AddData(ObjTag, gameObject.tag);
            }
            if(sourceRequirement.saveObjLayer)
            {
                saveBasicData.saveDict.AddData(ObjLayer, gameObject.layer.ToString());
            }
            if(sourceRequirement.saveCompData)
            {
                foreach (KeyValuePair<int, ComponentData> compDataPair in CompDatas)
                {
                    ComponentSaveData compSaveData = new();
                    if(compDataPair.Value.comp is not ISavable) continue;
                    ISavable savable = compDataPair.Value.comp as ISavable;
                    SaveRequirement compSaveReq = null;
                    try
                    {
                        compSaveReq = GetComponentSaveRequirement(compDataPair.Value);
                    }catch(Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                    compSaveData.compFullName = compDataPair.Value.comp.GetType().FullName;
                    if(compSaveReq.saveData)
                    {
                        compSaveData.saveDataDict = savable.SaveData(compSaveReq);
                    }
                    compSaveData.saveRequirement = compSaveReq;
                    saveBasicData.compDataDict.AddData(compDataPair.Key, compSaveData);
                }
            }
            return saveBasicData;
        }
        
        public IEnumerator LoadComponents(SaveBasicData saveBasicData)
        {
            BasicSaverRequirement sourceRequirement = GetBasicSaverRequirement();
            if( ! sourceRequirement.saveCompData)
            {
                yield break;
            }
            int[] compDatasOri = CompDatas.Keys.ToArray();
            int[] mandatedComp = saveBasicData.compDataDict.keys.ToArray();
            int[] addedComps = mandatedComp.Except(compDatasOri).ToArray();
            int[] removedComps = compDatasOri.Except(mandatedComp).ToArray();
            int[] sameComps = compDatasOri.Intersect(mandatedComp).ToArray();
            foreach (int addedComp in addedComps)
            {
                if( ! saveBasicData.compDataDict.TryGetValue(addedComp, out ComponentSaveData compSaveData))
                {
                    continue;
                }
                Type targetType = Type.GetType($"{compSaveData.compFullName}, {KickStarter.AssemblyName}");
                Component instantedComp = gameObject.AddComponent(targetType);
                if(instantedComp is not ISavable) continue;
                ComponentData componentData = new();
                componentData.comp = instantedComp;
                componentData.saveRequirement = compSaveData.saveRequirement;
                CompDatas.Add(addedComp, componentData);
                CompDatas2.Add(instantedComp, addedComp);
            }
            foreach (int removedComp in removedComps)
            {
                if( ! CompDatas.TryGetValue(removedComp, out ComponentData componentData))
                {
                    continue;
                }
                RemoveObjectComponent(componentData.comp);
            }
            yield break;
        }
        public IEnumerator LoadEveryComponentData(SaveBasicData saveBasicData)
        {
            BasicSaverRequirement sourceRequirement = GetBasicSaverRequirement(); 
            if(sourceRequirement.saveObjName)
            {
                gameObject.name = saveBasicData.saveDict.GetData(ObjName);
            }
            if(sourceRequirement.saveObjTag)
            {
                gameObject.tag = saveBasicData.saveDict.GetData(ObjTag);
            }
            if(sourceRequirement.saveObjLayer)
            {
                gameObject.layer = int.Parse(saveBasicData.saveDict.GetData(ObjLayer));
            }
            if(sourceRequirement.saveParent)
            {
                bool isParented = bool.Parse(saveBasicData.saveDict.GetData(IsParented));
                if(isParented)
                {
                    ConstantID parentContId = ConstantID.GetComponent(int.Parse(saveBasicData.saveDict.GetData(ParentID)));
                    BasicObjectDataSaver basicData = parentContId as BasicObjectDataSaver; 
                    if(basicData != null)
                    {
                        transform.SetParent(basicData.transform);
                    }
                    // if(PersistentIDManager.Instance.basicSaverDict.TryGetValue(int.Parse(saveBasicData.saveDict[ParentID]), out BasicObjectDataSaver resultDataSaver))
                    // {
                    //     transform.SetParent(resultDataSaver.transform);
                    // }
                }
            }
            if(sourceRequirement.saveCompData)
            {
                foreach (KeyValuePair<int, ComponentSaveData> _compDataPair  in saveBasicData.compDataDict.GetAsDictionary())
                { 
                    if( ! _compDataPair.Value.saveRequirement.saveData ) continue;
                    if( ! CompDatas.TryGetValue(_compDataPair.Key, out ComponentData _compData)) continue;
                    if(_compData.comp is not ISavable) continue;
                    ISavable savable = _compData.comp as ISavable;
                    // SaveRequirement compSaveReq = null;
                    // try
                    // {
                    //     compSaveReq = GetComponentSaveRequirement(_compData);
                    // }catch(Exception e)
                    // {
                    //     Debug.LogError(e.Message);
                    // }
                    savable.LoadData(_compDataPair.Value.saveDataDict, _compDataPair.Value.saveRequirement);
                }
            }
            yield break; 
        }
        public void GatherComponents()
        {
            DictionaryBridgeL1<int, ComponentData> newCompDatas = new();
            DictionaryBridgeL1<Component, int> newCompDatas2 = new();
            Component[] gatheredComps = GetComponents<Component>();
            List<Component> newComps = new(); 
            foreach (Component gatheredComp in gatheredComps)
            {
                if(gatheredComp is ISavable)
                {
                    newComps.Add(gatheredComp);
                }
            }
            foreach (Component newComp in newComps)
            {
                ComponentData _compData = new();
                _compData.comp = newComp;
                int compID = 0;
                if(CompDatas2.TryGetValue(newComp, out int resultCompId))
                {
                    compID = resultCompId;
                }else
                {
                    compID = Mathf.Abs(newComp.GetInstanceID());
                }
                newCompDatas.AddData(compID, _compData);
                newCompDatas2.AddData(newComp, compID);
            }
            compDataBridge = newCompDatas;
            compDataBridge2 = newCompDatas2;
        }
        public Component AddObjectComponent<T>() where T : Component
        {
            Component instantedComp = gameObject.AddComponent<T>();
            if(instantedComp is not ISavable)
            {
                Debug.LogWarning("Component is not ISavable, error may cause");
                return instantedComp;
            }
            ComponentData _compData = new();
            _compData.comp = instantedComp;
            int compID = Mathf.Abs(instantedComp.GetInstanceID());
            CompDatas.Add(compID, _compData);
            CompDatas2.Add(instantedComp, compID);
            return instantedComp;
        }
        public void RemoveObjectComponent(Component targetComp)
        {
            if( ! CompDatas2.TryGetValue(targetComp, out int compID))
            {
                return;
            }
            if( ! CompDatas.TryGetValue(compID, out ComponentData _compData))
            {
                return;
            }
            CompDatas.Remove(compID);
            CompDatas2.Remove(targetComp);
            if(targetComp is not ISavable)
            {
                Debug.LogWarning("Component is not ISavable, error may cause");
            }
            Destroy(targetComp);
        }
        public bool TryGetObjectComponent<T>(int compID, out T targetComp) where T : Component 
        {
            targetComp = null;
            if( ! CompDatas.TryGetValue(compID, out ComponentData _compData))
            {
                return false;
            }
            Component _comp = _compData.comp;
            targetComp =  _comp as T;
            return true;
        }
        public bool TryGetComponentID(Component _comp, out int compID)
        {
            compID = 0;
            if( ! CompDatas2.TryGetValue(_comp, out int resultCompID))
            {
                return false;
            }
            compID = resultCompID;
            return true;
        }
        // private void AutoRegisterSelf()
        // {
        //     RegisterSelf();
        // }
        private BasicSaverRequirement GetBasicSaverRequirement()
        {
            if(basicSaverRequirement == null)
            {
                return KickStarter.settingsManager.basicSaverRequirement;
            }else
            {
                return basicSaverRequirement;
            }
        }
        private SaveRequirement GetComponentSaveRequirement(ComponentData componentData)
        {
            if(componentData.saveRequirement != null)
            {
                return componentData.saveRequirement;
            }
            string compName = componentData.comp.GetType().Name;
            if(KickStarter.settingsManager.SaveReqDict.TryGetValue(compName, out SaveRequirement resultSaveReq))
            {
                return resultSaveReq;
            }
            throw new Exception($"There's no key with name {compName}");
        }
    }
    public class ComponentSaveData
    {
        public SaveDataDict saveDataDict;
        public string compFullName;
        public SaveRequirement saveRequirement;
    }
    [Serializable]
    public class ComponentSaveDataL2
    {
        public DictionaryBridgeL1<string, string> saveDataDict = new();
        public string compFullName;
        public int saveReqID;
    }
    public class SaveBasicData
    {
        public DictionaryBridgeL1<int, ComponentSaveData> compDataDict = new();
        public DictionaryBridgeL1<string, string> saveDict = new();
    }
    [Serializable]
    public class SaveBasicDataL2
    {
        public DictionaryBridgeL1<int, ComponentSaveDataL2> compDataDict = new();
        public DictionaryBridgeL1<string, string> saveDict = new();
    }
    public class SaveDataDict{
        public DictionaryBridgeL1<string, string> saveDict = new();
        public DictionaryBridgeL1<string, Component> compRefs = new();
        public DictionaryBridgeL1<string, ScriptableObject> soRefs = new();
    }
    public static class SaveDataL2
    {
        public const string SaveDict = "saveDict",
        CompRefs = "compRefs",
        SORefs = "soRefs";
    }
    [Serializable]
    public class ComponentSaveDataV2
    {
        public int basicSaverId;
        public int compID;
    }
}
    
