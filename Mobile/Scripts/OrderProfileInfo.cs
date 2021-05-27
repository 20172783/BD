using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrderProfileInfo : MonoBehaviour
{
    public string name;
    public string phone;

    public TMP_InputField nameText;
    public TMP_InputField phoneText;

    public void getInfo()
    {
        GameObject.Find("FirebaseManager").GetComponent<FirebaseManager>().GetClientDataForOrder();
    }
    public void fillFields()
    {
        nameText.text = name;
        phoneText.text = phone;
    }

    public void ContinueOrderButton()
    {
        GameObject FinalOrderPanel = GameObject.Find("ListPanel").GetComponent<OrderListPanel>().FinalOrderPanel;
        FinalOrderPanel.GetComponent<OrderComplete>().name = name;
        FinalOrderPanel.GetComponent<OrderComplete>().clientPhone = phone;
    }
}
