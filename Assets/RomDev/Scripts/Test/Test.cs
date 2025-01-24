using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RomDev
{
    public class Test : MonoBehaviour
    {
        public GameObject prefabSource;
        public string targetScene;
        public GameObject objDestroy;
        public GameObject targetToAdd;
        public GameObject targetToRemove;
        public TestSavable componentToRemove;
        public void InstantObject()
        {
            KickStarter.levelStorage.InstantiateObject(prefabSource, targetScene);
        }
        public void DestroyObject()
        {
            KickStarter.levelStorage.DeleteObject(objDestroy);
        }
        public void AddObjectComponent()
        {
            targetToAdd.GetComponent<BasicObjectDataSaver>().AddObjectComponent<TestSavable>();
        }
        public void RemoveObjectComponent()
        {
            targetToAdd.GetComponent<BasicObjectDataSaver>().RemoveObjectComponent(componentToRemove);
        }
    }
}

