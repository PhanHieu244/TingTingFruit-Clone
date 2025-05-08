using ChuongCustom;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.ChuongScripts
{
    public class TargetFill : Singleton<TargetFill>
    {
        [SerializeField] private Image fill;

        private float target;
        
        public Tween SetProgress(float value, float duration = 0.2f)
        {
            var fillAmount = (target - value) / target;
            return fill.DOFillAmount(fillAmount, duration).SetEase(Ease.OutQuad);
        }

        public void SetTarget(float value)
        {
            fill.fillAmount = 0;
            target = value;
        }
    }
}