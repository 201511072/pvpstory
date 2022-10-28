using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{
    string log;
    public GameObject startPanel;
    bool canLogin=true;


    //void OnGUI()
    //{
    //    GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 3);
    //
    //
    //    if (GUILayout.Button("ClearLog"))
    //        log = "";
    //
    //    if (GUILayout.Button("Login"))
    //        GPGSBinder.Inst.Login((success, localUser) =>
    //        log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");
    //
    //    if (GUILayout.Button("Logout"))
    //        GPGSBinder.Inst.Logout();        
    //
    //    GUILayout.Label(log);
    //}
    
    public void BtnClick_Login()
    {
        if (canLogin)
        {
            canLogin = false;
            GPGSBinder.Inst.Login((success, localUser) =>
            log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");

            StartCoroutine(StartPanelOffCRT());
        }
    }

    public IEnumerator StartPanelOffCRT()
    {
        yield return new WaitForSeconds(2f);
        if (log.IndexOf("T") == 0)
        {
            StartPanelOff();
        }
        yield return new WaitForSeconds(0.1f);
        canLogin = true;
    }



    public void StartPanelOff()
    {
        NetworkManager.instance.isTempAccount = true;
        startPanel.SetActive(false);
        NetworkManager.instance.soundManagerScript.AS_Setting(Sound_Background.Main);
    }

}