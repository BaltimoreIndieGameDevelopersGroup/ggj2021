namespace Game
{
    using UnityEngine;

    [CreateAssetMenu]
    public class GameText : ScriptableObject
    {
        [TextArea] [SerializeField] private string introText;

        [TextArea] [SerializeField] private string[] victoryText;

        public string IntroText { get { return introText; } }
        public string[] VictoryText { get { return victoryText; } }
    }
}

