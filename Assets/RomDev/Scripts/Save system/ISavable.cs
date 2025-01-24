using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public interface ISavable 
    {
        public SaveDataDict SaveData(SaveRequirement saveRequirement);
        public void LoadData(SaveDataDict saveData, SaveRequirement saveRequirement);
    }

}