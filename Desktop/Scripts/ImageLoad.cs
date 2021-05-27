using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System.IO;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
	public Sprite sprite = null;
	public GameObject CreateNewServiceComponent;
	public void onPress()
    {
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
		FileBrowser.SetDefaultFilter("Images");
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
		FileBrowser.AddQuickLink("Users", "C:\\Desktop", null);
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
		Debug.Log(FileBrowser.Success);

		if (FileBrowser.Success)
		{
			string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
			FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
			this.gameObject.GetComponent<Image>().sprite = LoadSelectedImage(destinationPath);

			string[] NameArray = FileBrowser.Result[0].Split(char.Parse("."));
			string format = "."+ NameArray[1];

			CreateNewServiceComponent.GetComponent<CreateNewService>().ImgUrl = destinationPath;
			CreateNewServiceComponent.GetComponent<CreateNewService>().ImgFormat = format;
		}
	}
	public Sprite LoadSelectedImage(string filePath)
	{
		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))
		{
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); // autoresize img
			Rect rect = new Rect(0, 0, tex.width, tex.height);
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			sprite = Sprite.Create(tex, rect, pivot);
		}
		return sprite;
	}
}
