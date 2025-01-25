using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RomDev
{
    public class HealthBar : MonoBehaviour
    {
        public Image fillImage;
        public Image fillReducing;
        public float reducingTime;//The full time to reduce all health
        public float reducingWait;//time to start reducing
        private BarState barState;
        private float timeElapsed;
        private float timePercentage;
        private Coroutine savedCoroutine;
        private enum BarState:byte
        {
            Idle,
            WaitReducing,
            Reducing
        }
        public virtual void Awake() {
            EarlyConfigProps();
        }
        public virtual void Start() {
            barState=BarState.Idle;
        }
        public virtual void Update() {
            if(barState==BarState.Reducing)
            {
                timeElapsed+=Time.deltaTime;
                timePercentage=timeElapsed/reducingTime;
                fillReducing.fillAmount=Mathf.Lerp(1f,0f,timePercentage);
                if(fillReducing.fillAmount<=fillImage.fillAmount)
                {
                    barState=BarState.Idle;
                    if(fillReducing.fillAmount<=0f)
                    {
                        HealthBarAbility(false);
                    }
                }
            }
        }
        private void EarlyConfigProps()
        {
        }
        private IEnumerator WaitReducing()
        {
            yield return new WaitForSeconds(reducingWait);
            barState=BarState.Reducing;
        }
        public void SetHealthFill(float fillAmount)
        {
            if(barState==BarState.Idle)
            {
                barState=BarState.WaitReducing;
                savedCoroutine=StartCoroutine(WaitReducing());
            }
            fillImage.fillAmount=fillAmount;
        }
        public void HealthBarAbility(bool ability)
        {
            gameObject.SetActive(ability);
        }
    }
}

