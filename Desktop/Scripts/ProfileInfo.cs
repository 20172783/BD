using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfileInfo : MonoBehaviour
{
    public string name;
    public string phone;
    public string rating;
    public string username;
    public string accepted;
    public string verified;
    public string status;
    public string asm_kodas;
    public string email;

    public TMP_Text ProgramTitle;
    public Text UserName;
    public Image image;
    public void fillData(string _name, string _phone, string _rating, string _username, string _accepted, string _vedified, string _status, string _asm_kodas, string _email)
    {
        name = _name;
        phone = _phone;
        rating = _rating;
        username = _username;
        accepted = _accepted;
        verified = _vedified;
        status = _status;
        asm_kodas = _asm_kodas;
        email = _email;

        ProgramTitle.text = name;
        UserName.text = username;

    }
}
