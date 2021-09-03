using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColourRenderer = null;

    //hooks call the function whenever the value of var is updated.
    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColourUpdated))]
    [SerializeField]
    private Color displayColour = Color.black;


    #region Server

    //only executed in servers and not in clients
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }



    [Server]
    public void SetDisplayColour(Color newDisplayColour)
    {
        displayColour = newDisplayColour;
    }



    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        //server Validation here
        if(newDisplayName.Length<4 || newDisplayName.Length > 12)
        {
            return;
        }

        RpcLogNewName(newDisplayName); 

        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client


    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }



    private void HandleDisplayColourUpdated(Color oldColour, Color newColour)
    {
        displayColourRenderer.material.SetColor("_Color", newColour);
    }
    


    [ContextMenu("Set Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("New Name");
    }



    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }



    #endregion
}

