using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    public TextMeshProUGUI pickupCountText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void peekGameplayUI()
    {

    }

    public void SetPickupCount(int pickupCount)
    {
        pickupCountText.SetText(pickupCount.ToString());
    }
}
