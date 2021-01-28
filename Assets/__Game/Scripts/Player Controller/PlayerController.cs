namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private void Awake()
        {
            ServiceLocator.Register<IPlayerController>(this);
        }

        private void Update()
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }
}
