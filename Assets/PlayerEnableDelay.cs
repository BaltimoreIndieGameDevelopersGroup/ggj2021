using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerEnableDelay : MonoBehaviour
{
    public int enableDelay = 5;
    public GameObject player;
    private Vector3 initLoc;

    async void Start()
    {
        await Task.Delay(enableDelay);
        player.transform.position = initLoc;
        //player.SetActive(true);
    }
    public void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");//.GetComponent<CharacterController>();
        initLoc = player.transform.position;

        //player.SetActive(false);


    }
}
