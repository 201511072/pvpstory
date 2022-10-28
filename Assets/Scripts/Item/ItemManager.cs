using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ItemManager : MonoBehaviourPunCallbacks
{
    public RectTransform RT;
    public int Slot1;
    public int Slot2;
    public int Slot3;

    public Image Slot1Image;
    public Image Slot2Image;
    public Image Slot3Image;
    public Image SelectedImage;
    public Text Explain;

    public Sprite EmptyItemSlot;

    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;

    public GameObject Inventory;
    public InventoryScript inventoryScript;
    public PhotonView PV;
    public GameObject ExplainItem;
    public Text ExplainItemOfTeam;

    int CurrentSlot;
    int tempItem;


    private void Start()
    {
        if (PV.IsMine)
        {
            NetworkManager.instance.ItemManager = this;
            if (NetworkManager.instance.myPlayerNumber == 1)
            {
                PV.RPC("ItemSlotRTChangeRPC", RpcTarget.All, 320f, 50f, 1);
            }
            else if (NetworkManager.instance.myPlayerNumber == 2)
            {
                PV.RPC("ItemSlotRTChangeRPC", RpcTarget.All, -425f, 50f, 2);
            }
            else if (NetworkManager.instance.myPlayerNumber == 3)
            {
                PV.RPC("ItemSlotRTChangeRPC", RpcTarget.All, -425f, -406f, 3);
            }
            else if (NetworkManager.instance.myPlayerNumber == 4)
            {
                PV.RPC("ItemSlotRTChangeRPC", RpcTarget.All, 320f, -406f, 4);
            }
            Inventory = NetworkManager.instance.Inventory;
            inventoryScript = Inventory.GetComponent<InventoryScript>();
            SelectedImage = inventoryScript.SelectedImage;
            Explain = inventoryScript.Explain;
            inventoryScript.init(this);
        }
    }

    [PunRPC]
    public void ItemSlotRTChangeRPC(float x, float y, int playerNumber)
    {
        bool tempBool = true;
        if (NetworkManager.instance.myPlayerNumber == 1 && (playerNumber == 2 || playerNumber == 3))
        {
            tempBool = false;
        }
        else if (NetworkManager.instance.myPlayerNumber == 2 && (playerNumber == 1 || playerNumber == 4))
        {
            tempBool = false;
        }
        else if (NetworkManager.instance.myPlayerNumber == 3 && (playerNumber == 1 || playerNumber == 4))
        {
            tempBool = false;
        }
        else if (NetworkManager.instance.myPlayerNumber == 4 && (playerNumber == 2 || playerNumber == 3))
        {
            tempBool = false;
        }

        if (tempBool)
        {
            if (playerNumber == 1)
            {
                RT.anchorMin = new Vector2(0f, 0.5f);
                RT.anchorMax = RT.anchorMin;
            }
            else if (playerNumber == 2)
            {
                RT.anchorMin = new Vector2(1f, 0.5f);
                RT.anchorMax = RT.anchorMin;
            }
            else if (playerNumber == 3)
            {
                RT.anchorMin = new Vector2(1f, 0.5f);
                RT.anchorMax = RT.anchorMin;
            }
            else if (playerNumber == 4)
            {
                RT.anchorMin = new Vector2(0f, 0.5f);
                RT.anchorMax = RT.anchorMin;
            }

            gameObject.transform.SetParent(NetworkManager.instance.CharacterSelectCanvas.transform);
            RT.localScale = Vector3.one;
            RT.anchoredPosition = new Vector2(x, y);
        }
    }


    public void CurrentSlotChange(int whatSlot)
    {
        if (PV.IsMine)
        {
            CurrentSlot = whatSlot;
            Inventory.SetActive(true);
        }
        else
        {
            if (whatSlot == 1 && Slot1 != 0)
            {
                ExplainItem.SetActive(true);
                ExplainItemOfTeam.text = ExplainItemTexts(Slot1);
            }
            else if (whatSlot == 2 && Slot2 != 0)
            {
                ExplainItem.SetActive(true);
                ExplainItemOfTeam.text = ExplainItemTexts(Slot2);
            }
            else if (whatSlot == 3 && Slot3 != 0)
            {
                ExplainItem.SetActive(true);
                ExplainItemOfTeam.text = ExplainItemTexts(Slot3);
            }
        }
    }

    public string ExplainItemTexts(int whatItem)
    {
        if (whatItem == 1)
        {
            if (!NetworkManager.instance.englishMode)
            {
                return "사용시 2.5초간 무적이 됩니다. 그동안 조작 불가능합니다\n쿨타임 : 30초";
            }
            else
            {
                return "If use press button, invincible 2.5seconds. You can't move.";
            }
        }
        else if (whatItem == 2)
        {
            if (!NetworkManager.instance.englishMode)
            {
                return "일정거리에 적이 있을시 2초마다 30고정피해를 입힙니다";
            }
            else
            {
                return "If enemy is near at you, enemy will get damage on every 2seconds. 30 fixdamage";
            }
        }
        else if (whatItem == 3)
        {
            if (!NetworkManager.instance.englishMode)
            {
                return "이동속도가 0.1증가합니다. 공격력이 20증가합니다";
            }
            else
            {
                return "Speed up 0.1. Atk up 20";
            }
        }
        return null;
    }

    public void ExplainExit()
    {
        ExplainItem.SetActive(false);
        ExplainItemOfTeam.text = null;
    }


    public void tempItemChange(int whatItem)
    {
        tempItem = whatItem;

        if (whatItem == 1)
        {
            SelectedImage.sprite = Item1;
            Explain.text = ExplainItemTexts(whatItem);
        }
        else if (whatItem == 2)
        {
            SelectedImage.sprite = Item2;
            Explain.text = ExplainItemTexts(whatItem);
        }
        else if (whatItem == 3)
        {
            SelectedImage.sprite = Item3;
            Explain.text = ExplainItemTexts(whatItem);
        }
    }

    public void Complete()
    {
        if (CurrentSlot == 1)
        {
            Slot1 = tempItem;
            Slot1Image.sprite = ChangeSlotImage(tempItem);
            OverlapItemCheck();
            SelectedImage.sprite = null;
            Explain.text = null;
            Inventory.SetActive(false);
        }
        else if (CurrentSlot == 2)
        {
            Slot2 = tempItem;
            Slot2Image.sprite = ChangeSlotImage(tempItem);
            OverlapItemCheck();
            SelectedImage.sprite = null;
            Explain.text = null;
            Inventory.SetActive(false);
        }
        else if (CurrentSlot == 3)
        {
            Slot3 = tempItem;
            Slot3Image.sprite = ChangeSlotImage(tempItem);
            OverlapItemCheck();
            SelectedImage.sprite = null;
            Explain.text = null;
            Inventory.SetActive(false);
        }
        PV.RPC("SynchronizeSlot", RpcTarget.All, Slot1, Slot2, Slot3);
    }

    public Sprite ChangeSlotImage(int tempItem)
    {
        if (tempItem == 0)
        {
            return EmptyItemSlot;
        }
        else if (tempItem == 1)
        {
            return Item1;
        }
        else if (tempItem == 2)
        {
            return Item2;
        }
        else if (tempItem == 3)
        {
            return Item3;
        }

        return EmptyItemSlot;
    }


    void OverlapItemCheck()
    {
        if (CurrentSlot == 1)
        {
            if (tempItem == Slot2)
            {
                Slot2 = 0;
                Slot2Image.sprite = EmptyItemSlot;
            }
            if (tempItem == Slot3)
            {
                Slot3 = 0;
                Slot3Image.sprite = EmptyItemSlot;
            }
        }
        else if (CurrentSlot == 2)
        {
            if (tempItem == Slot1)
            {
                Slot1 = 0;
                Slot1Image.sprite = EmptyItemSlot;
            }
            if (tempItem == Slot3)
            {
                Slot3 = 0;
                Slot3Image.sprite = EmptyItemSlot;
            }
        }
        else if (CurrentSlot == 3)
        {
            if (tempItem == Slot1)
            {
                Slot1 = 0;
                Slot1Image.sprite = EmptyItemSlot;
            }
            if (tempItem == Slot2)
            {
                Slot2 = 0;
                Slot2Image.sprite = EmptyItemSlot;
            }
        }
    }



    public void Cancle()
    {
        SelectedImage.sprite = null;
        Explain.text = null;
        Inventory.SetActive(false);
    }


    [PunRPC]
    void SynchronizeSlot(int Slot1, int Slot2, int Slot3)
    {
        this.Slot1 = Slot1;
        this.Slot2 = Slot2;
        this.Slot3 = Slot3;
        SlotImageChange(1);
        SlotImageChange(2);
        SlotImageChange(3);
    }

    public void SlotImageChange(int whatSlot)
    {
        if (whatSlot == 1)
        {
            Slot1Image.sprite = ChangeSlotImage(Slot1);
        }
        else if (whatSlot == 2)
        {
            Slot2Image.sprite = ChangeSlotImage(Slot2);
        }
        else if (whatSlot == 3)
        {
            Slot3Image.sprite = ChangeSlotImage(Slot3);
        }
    }
}