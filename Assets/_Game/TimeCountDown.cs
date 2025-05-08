using System;
using ChuongCustom;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Game
{
    public class TimeCountDown : Singleton<TimeCountDown>
    {
        [SerializeField] private Image fill;
        [SerializeField] private float duration = 1f;

        private void OnEnable()
        {
            CountDown(duration);
        }
        
        
#if UNITY_EDITOR
        [Button]
        private void OnCompleted()
        {
            transform.DOKill();
            CountDown(1f);
        }
#endif

        public Tween CountDown(float time)
        {
            fill.fillAmount = 1f;
            return fill.DOFillAmount(0.004f, time).OnComplete(() =>
            {
                InGameManager.Instance.Lose();
            }).SetTarget(transform);
        }
    }
}