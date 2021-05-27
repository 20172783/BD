using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FirmServicePreview : MonoBehaviour
{
    public Image Img;
    public TMP_Text Title;
    public TMP_Text Desc;
    public TMP_Text Location;
    public TMP_Text Price;
    public TMP_Text Duration;
    public TMP_Text Workers;
    public TMP_Text Types;

    public void FillPreview(string title, string desc, string location, string price, string duration, string workers, string types, Image img)
    {
        Title.text = title;
        Desc.text = desc;
        Location.text = location;
        Price.text = price;
        Duration.text = duration;
        Workers.text = workers;
        Types.text = types;
        Img = img;
    }
}
