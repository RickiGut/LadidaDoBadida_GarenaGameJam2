using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class Trigger2D_L2 : MonoBehaviour
    {
        public ActionList actionOnEnter;
        public ActionList actionOnContinuous;
        public ActionList actionOnExit;
        private void OnTriggerEnter2D(Collider2D other) {
            if(actionOnEnter != null)
            {
                actionOnEnter.Interact();
            }
        }
        private void OnTriggerStay2D(Collider2D other) {
            if(actionOnContinuous != null)
            {
                actionOnContinuous.Interact();
            }
        }
        private void OnTriggerExit2D(Collider2D other) {
            if(actionOnExit != null)
            {
                actionOnExit.Interact();
            }
        }
    }
}

