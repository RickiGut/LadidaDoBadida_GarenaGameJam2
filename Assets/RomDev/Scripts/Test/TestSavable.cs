using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RomDev
{
    public class TestSavable : MonoBehaviour, ISavable
    {
        public int num;
        private const string num_PROP = "num";
        public SaveDataDict SaveData(SaveRequirement saveRequirement)
        {
            // Debug.Log("save Data");
            SaveDataDict saveDataDict = new ();
            saveDataDict.saveDict.AddData(num_PROP, num.ToString());
            return saveDataDict;
        }
        public void LoadData(SaveDataDict saveDataDict, SaveRequirement saveRequirement)
        {
            if(saveDataDict == null)
            {
                Debug.Log("Is Null");
                return;
            }
            string numString = saveDataDict.saveDict.GetData(num_PROP);
            num = int.Parse(numString);
        }
    }
}