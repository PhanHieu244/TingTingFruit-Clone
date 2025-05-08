using ChuongCustom;
using UnityEngine;

namespace _Game
{
    public class ButtonHint : ButtonClick
    {
        private void ShowHint()
        {
            InGameManager.Instance.ShowHint();
        }

        protected override void OnClick()
        {
            if (Data.Player.Gem <= 0) return;
            Data.Player.Gem--;
            ShowHint();
        }
    }
}