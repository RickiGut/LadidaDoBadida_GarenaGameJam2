using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    [CreateAssetMenu(fileName = "BasicSaverRequirement", menuName = "ScriptableObject/SaveRequirement/BasicSaverRequirement")]
    public class BasicSaverRequirement : ScriptableObject
    {
        public bool saveCompData;
        public bool saveParent;
        public bool saveObjName;
        public bool saveObjTag;
        public bool saveObjLayer;
    }
}
