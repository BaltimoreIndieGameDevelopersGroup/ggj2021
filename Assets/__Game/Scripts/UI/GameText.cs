namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu]
    public class GameText : ScriptableObject
    {
        [TextArea] [SerializeField] private string introText;

        public string IntroText { get { return introText; } }
    }
}
