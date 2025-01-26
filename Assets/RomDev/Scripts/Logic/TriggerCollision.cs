using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomDev
{
    public class TriggerCollision : MonoBehaviour
    {
        public ActionList actionOnEnter;
        public ActionList actionOnContinuous;
        public ActionList actionOnExit;
        public string targetTag = "Player";
        private void OnCollisionEnter2D(Collision2D other) {
            if(actionOnEnter != null) {}
            {
                if( ! string.IsNullOrEmpty(targetTag))
                {
                    if(other.transform.tag != targetTag) return;
                    Player _player = other.transform.GetComponent<Player>();
                    if( _player != null ) return;
                }
                actionOnEnter.Interact();
            }   
        }
        private void OnCollisionStay2D(Collision2D other) {
            if(actionOnContinuous != null)
            {
                if( ! string.IsNullOrEmpty(targetTag))
                {
                    if(other.transform.tag != targetTag) return;
                    Player _player = other.transform.GetComponent<Player>();
                    if( _player != null ) return;
                }
                actionOnContinuous.Interact();
            }
        }
        private void OnCollisionExit2D(Collision2D other) {
            if(actionOnExit != null)
            {
                if( ! string.IsNullOrEmpty(targetTag))
                {
                    if(other.transform.tag != targetTag) return;
                    Player _player = other.transform.GetComponent<Player>();
                    if( _player != null ) return;
                }
                actionOnExit.Interact();
            }
        }
    }
}

