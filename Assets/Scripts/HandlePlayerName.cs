using TMPro;
using UnityEngine;

public class HandlePlayerName : MonoBehaviour
{
    TMP_InputField playerNameField;
    
    private void Start()
    {
        playerNameField = gameObject.GetComponentInParent<TMP_InputField>();
    }


    //Once the player entered or modified the name in the input field then save it in the Game Manager
    public void HandlePlayerNameEdited()
    {
        GameManager.instance.SetPlayerName(playerNameField.text);
    }
}
