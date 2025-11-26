using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string KeyName;
    public Sprite KeySprite;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.AddKey(KeyName,KeySprite);
        Destroy(gameObject);
    }
}