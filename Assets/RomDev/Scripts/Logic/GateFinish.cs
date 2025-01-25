using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class GateFinish : MonoBehaviour
    {
        public Animator animator;
        public string getButtonParam;
        public string finishParam;
        public int getButton;
        public ActionList actionList;
        private void OnTriggerEnter2D(Collider2D other) {
            CheckTotalButton();
        }
        public void ButtonPressed()
        {
            getButton ++;
            animator.SetInteger(getButtonParam, getButton);
        }
        private void CheckTotalButton()
        {
            if(getButton < 2) return;
            getButton = 0;
            animator.SetBool(finishParam, true);
            actionList.Interact();
        }
    }
}

