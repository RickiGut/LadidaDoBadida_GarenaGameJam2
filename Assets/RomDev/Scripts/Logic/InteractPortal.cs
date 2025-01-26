using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class InteractPortal : MonoBehaviour
    {
        public ActionList actionList;
        public GameObject instructObj;
        public string targetTag = "Player";
        private void Awake() {
            enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag != targetTag) return;
            EnableChecking();
        }
        private void OnTriggerExit2D(Collider2D other) {   
            if(other.tag != targetTag) return;
            DisableChecking();
        }
        private void Update() {
            PerformChecking();
        }
        public void EnableChecking()
        {
            enabled = true;
            instructObj.SetActive(true);
        }
        public void DisableChecking()
        {
            enabled = false;
            instructObj.SetActive(false);
        }
        private void PerformChecking()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                actionList.Interact();
            }
        }
    }
}

