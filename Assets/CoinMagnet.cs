using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    PlayerStatus playerStatus;
    public float coinFlySpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (!playerStatus.coinMagnetIsOn) return;
            other.GetComponent<Coin>().MagnetEffect(transform, coinFlySpeed);
        }
    }
}
