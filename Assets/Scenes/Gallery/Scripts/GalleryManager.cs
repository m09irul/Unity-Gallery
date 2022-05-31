using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class GalleryManager : MonoBehaviour
{
    public GameObject pictureContainer;

    public GameObject fullScreenPanel;

	public string path;

	public TextMeshProUGUI ContainerText;

	FileInfo[] fileinfo;
	List<string> imageList = new List<string>();

	// Start is called before the first frame update
	void Start()
    {
		ContainerText.text = "";

		fullScreenPanel.SetActive(false);

		GetFileList();

	}

	public void GetFileList()
	{
		if (path != "")
		{
			DirectoryInfo dataDir = new DirectoryInfo(path);

			//clear all previous data
			CleanPictureContainer();

			try
			{
				ContainerText.gameObject.SetActive(false);

				string[] ss = { "*png", "*jpg", "*jpeg" }; // search for images with these extensions

				imageList.Clear();

				foreach (string s in ss)
				{
					fileinfo = dataDir.GetFiles(s);

					for (int i = 0; i < fileinfo.Length; i++)
					{
						imageList.Add(fileinfo[i].FullName);
					}

				}

				//if no files found show empty msg
				if (imageList.Count <= 0)
				{
					ContainerText.gameObject.SetActive(true);

					ContainerText.text = "No image found";

				}
				else
				{
					// Pick a PNG image from Gallery/Photos
					// If the selected image's width and/or height is greater than 1024px, down-scale the image
					PickImage(1024);
				}
			}
			catch (System.Exception e)
			{
				ContainerText.gameObject.SetActive(true);

				ContainerText.text = "Directory Invalid";
			}
		}
		else
			ContainerText.text = "Path null";
	}

	private void PickImage(int maxSize)
	{
		if (path != null)
		{
			Texture2D m_texture;
			Sprite s;

            for (int i = 0; i < imageList.Count; i++)
            {
				// Create Texture from selected image
				m_texture = NativeGallery.LoadImageAtPath(imageList[i], maxSize);

				if (m_texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				// convert texture to a sprite
				// add this to image UI component
				// preserve aspect
				GameObject img = new GameObject();

				s = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), Vector2.zero);
				
				img.AddComponent<Image>().sprite = s;

				img.AddComponent<Button>().onClick.AddListener(ButtonAction);

				RectTransform rt = img.GetComponent<RectTransform>();
				rt.SetParent(pictureContainer.transform);
				rt.localScale = Vector3.one;

				img.GetComponent<Image>().preserveAspect = true;
			}
			
		}
	}


	//Handles click action when image is pressed.
	//show the image full screen
	void ButtonAction()
	{
		Sprite my_sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;

		fullScreenPanel.SetActive(true);

		Image img = fullScreenPanel.transform.GetChild(0).GetComponent<Image>();

		img.sprite = my_sprite;
		img.preserveAspect = true;
	}

	void CleanPictureContainer()
	{
		foreach (Transform child in pictureContainer.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

}
