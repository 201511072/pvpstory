using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public Image SelectedImage;
    public Text Explain;
    public ItemManager itemManager;
    public Button Cancle;
    public Button Complete;
    public Button Item1;
    public Button Item2;
    public Button Item3;

    public void init(ItemManager itemManager)
    {
        this.itemManager = itemManager;
        Cancle.onClick.AddListener(itemManager.Cancle);
        Complete.onClick.AddListener(itemManager.Complete);
        Item1.onClick.AddListener(()=>itemManager.tempItemChange(1));
        Item2.onClick.AddListener(()=>itemManager.tempItemChange(2));
        Item3.onClick.AddListener(()=>itemManager.tempItemChange(3));
    }
}
