using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class InteractButton : MonoBehaviour
    {
        public ActionList actionList;
        public Animator animator;
        public GameObject instructObj;
        public string lockParam = "IsLocked";
        public bool isLocked;
        private void Awake() {
            enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if(isLocked) return;
            EnableChecking();
        }
        private void OnTriggerExit2D(Collider2D other) {   
            if(isLocked) return;
            DisableChecking();
        }
        private void Update() {
            PerformChecking();
        }
        public void LockButton()
        {
            isLocked = true;
            animator.SetBool(lockParam, true);
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
                DisableChecking();
            }
        }
    }
}

