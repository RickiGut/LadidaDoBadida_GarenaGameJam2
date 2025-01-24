using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    [CreateAssetMenu(fileName = "RegularSaveRequirement", menuName = "ScriptableObject/SaveRequirement/RegularSaveRequirement")]
    public class SaveRequirement : ScriptableObject
    {
        public bool saveData;
        public bool saveSaveDict;
        public bool saveCompRefs;
        public bool saveSORefs;
    }
}

