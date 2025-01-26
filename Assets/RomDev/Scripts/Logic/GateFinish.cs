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
        public string targetTag = "Player";
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag != targetTag) return;
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

