using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class InteractPortal : MonoBehaviour
    {
        public ActionList actionList;
        private void Awake() {
            enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            EnableChecking();
        }
        private void OnTriggerExit2D(Collider2D other) {   
            DisableChecking();
        }
        private void Update() {
            PerformChecking();
        }
        public void EnableChecking()
        {
            enabled = true;
        }
        public void DisableChecking()
        {
            enabled = false;
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

