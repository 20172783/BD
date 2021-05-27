using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeSelect : MonoBehaviour
{
    public Transform ListContent;
    public TMP_InputField TypeInput;
    public GameObject TypePrefab;
    public List<string> types = new List<string>();
    public List<string> selectedTypes = new List<string>();
    public GameObject CreateNewServiceObject;
    public void LoadAllTypes()
    {
        foreach (Transform child in ListContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string type in types)
        {
            
            GameObject TypeListElement = Instantiate(TypePrefab, ListContent);
            TypeListElement.GetComponent<TypeElement>().NewElement(type);
        }

        SelectAllSelectedTypes();
    }

    public void AddAllTypes()
    {
        types.Clear();
        types.Add("Kirpimas");
        types.Add("Masažas");
    }
    public void SelectAllSelectedTypes()
    {
        foreach (string type in selectedTypes)
        {
            foreach (Transform child in ListContent.transform)
            {
                if (child.gameObject.GetComponent<TypeElement>().Name.text == type)
                {
                    child.gameObject.GetComponent<TypeElement>().toggle.SetIsOnWithoutNotify(true);
                }
            }
        }
    }
    public void OpenSelectTypeButton()
    {
        this.gameObject.SetActive(true);
        AddAllTypes();
        LoadAllTypes();
    }
    public void SaveTypeSelect()
    {
        TypeInput.text = "";
        CreateNewServiceObject.GetComponent<CreateNewService>().selectedTypes.Clear();
        foreach (string type in selectedTypes)
        {
            TypeInput.text = TypeInput.text + " " + type + ",";

            CreateNewServiceObject.GetComponent<CreateNewService>().selectedTypes.Add(type);
        }
        
        this.gameObject.SetActive(false);
    }
    public void EditSaveTypeSelect()
    {
        TypeInput.text = "";
        CreateNewServiceObject.GetComponent<EditService>().selectedTypes.Clear();
        foreach (string type in selectedTypes)
        {
            TypeInput.text = TypeInput.text + " " + type + ",";

            CreateNewServiceObject.GetComponent<EditService>().selectedTypes.Add(type);
        }

        this.gameObject.SetActive(false);
    }

    public void LoadOnInputField()
    {
        TypeInput.text = "";
        
        foreach (string type in selectedTypes)
        {
            TypeInput.text = TypeInput.text + " " + type + ",";
        }
       // this.gameObject.SetActive(false);
    }

    public List<string> GetSelectedTypes()
    {
        if (selectedTypes.Count < 1)
        {
            selectedTypes.Add("Nėra");
        }
        return selectedTypes;
    }
}
