namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class WinScreen : EndGameScreen
    {
        [SerializeField] private GameText gameText;
        [SerializeField] private Slider successSlider;

        private int score;
        private int currentTextIndex;

        protected override void Start()
        {
            score = Mathf.Min(ServiceLocator.Get<IInventory>().Score, gameText.VictoryText.Length);
            successSlider.value = score;
            currentTextIndex = 0;
            base.Start();
        }

        protected override void StartTextScroll()
        {
            if (currentTextIndex < score)
            {
                StartCoroutine(ScrollText(gameText.VictoryText[currentTextIndex]));
                currentTextIndex++;
            }
            else
            {
                base.DoneScrolling();
            }
        }

        protected override void DoneScrolling()
        {
            StartTextScroll();
        }

    }
}
