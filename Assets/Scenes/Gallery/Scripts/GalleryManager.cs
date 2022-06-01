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

    public GameObject fullScreenPanelImage;
    public GameObject fullScreenPanelRawImage;

	public string path;

	public TextMeshProUGUI ContainerText;

	FileInfo[] fileinfo;
	List<string> imageList = new List<string>();

	public bool isRawImage;

	// Start is called before the first frame update
	void Start()
    {
		ContainerText.text = "";

		fullScreenPanelImage.SetActive(false);

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

				Debug.Log(e);
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
				if (!isRawImage)
				{
					GameObject obj = new GameObject("Image", typeof(Image));

					RectTransform rt = obj.GetComponent<RectTransform>();
					rt.SetParent(pictureContainer.transform);
					ResetRectTransform(rt);

					s = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), Vector2.zero);

					Image img = obj.GetComponent<Image>();
					img.sprite = s;
					img.preserveAspect = true;

					obj.AddComponent<Button>().onClick.AddListener(ButtonAction);
				}
				else
				{
					GameObject pObj = new GameObject("Image holder", typeof(RectTransform));

					RectTransform pRT = pObj.GetComponent<RectTransform>();
					pRT.SetParent(pictureContainer.transform);
					ResetRectTransform(pRT);

					GameObject obj = new GameObject("Image", typeof(RawImage));

					RectTransform rt = obj.GetComponent<RectTransform>();
					rt.SetParent(pObj.transform);
					ResetRectTransform(rt);

					RawImage img = obj.GetComponent<RawImage>();
					img.texture = m_texture;
					PreserveRawImageApect.SizeToParent(img);

					obj.AddComponent<Button>().onClick.AddListener(ButtonAction);
				}
			}
			
		}
	}

	//Reset Rect transform position and scale
	void ResetRectTransform(RectTransform rt)
	{
		rt.position = Vector3.zero;
		rt.localScale = Vector3.one;
	}

	//Handles click action when image is pressed.
	//show the image full screen
	void ButtonAction()
	{
		if (!isRawImage)
		{
			Sprite my_sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;

			fullScreenPanelImage.SetActive(true);

			Image img = fullScreenPanelImage.transform.GetChild(0).GetComponent<Image>();

			img.sprite = my_sprite;
			img.preserveAspect = true;
		}
		else
		{
			Texture my_texture = EventSystem.current.currentSelectedGameObject.GetComponent<RawImage>().texture;

			fullScreenPanelRawImage.SetActive(true);

			RawImage img = fullScreenPanelRawImage.transform.GetChild(0).GetComponent<RawImage>();

			img.texture = my_texture;
			PreserveRawImageApect.SizeToParent(img);
		}
	}

	void CleanPictureContainer()
	{
		foreach (Transform child in pictureContainer.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

}
