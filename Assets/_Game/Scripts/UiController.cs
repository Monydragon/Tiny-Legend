using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Image[] heartContainers;
    public Sprite fullHeartContainer;
    public Sprite emptyHeartContainer;

    private void OnEnable()
    {
        EventManager.onActorDamaged += EventManager_onActorDamaged;
    }

    private void EventManager_onActorDamaged(GameObject obj, int dmg)
    {
        if(obj.tag == "Player")
        {
            var player = obj.GetComponent<PlayerController>();
            for (int i = 0; i < heartContainers.Length; i++)
            {
                if (player.health - dmg > i)
                {
                    heartContainers[i].sprite = fullHeartContainer;
                }
                else
                {
                    heartContainers[i].sprite = emptyHeartContainer;

                }
            }
        }
    }

    private void OnDisable()
    {
        EventManager.onActorDamaged -= EventManager_onActorDamaged;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
