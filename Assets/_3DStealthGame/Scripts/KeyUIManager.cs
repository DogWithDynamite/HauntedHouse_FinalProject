using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    public Transform keysContainer; 
    public GameObject keyIconPrefab; 

    private Dictionary<string, GameObject> keyIcons = new Dictionary<string, GameObject>();

 
    public void AddKeyUI(string keyName, Sprite keySprite)
    {
        if (keyIcons.ContainsKey(keyName)) return;

        GameObject newIcon = Instantiate(keyIconPrefab, keysContainer);
        newIcon.GetComponent<Image>().sprite = keySprite;
        keyIcons[keyName] = newIcon;
    }
}
