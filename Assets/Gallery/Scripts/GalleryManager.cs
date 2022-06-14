using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class GalleryManager : MonoBehaviour
{
	
    public GameObject pictureContainer;

    public GameObject fullScreenPanel;

	string album ; // initalized on start
	string path; // initalized on start

	public TextMeshProUGUI ContainerText;

	FileInfo[] fileinfo;
	List<string> imageList = new List<string>();

	Texture2D texture;

	// Start is called before the first frame update
	void Start()
    {
		album = "Test folder";
		
		path = "/storage/emulated/0/DCIM/" + album + "/";

		ContainerText.text = "";

		fullScreenPanel.SetActive(false);

		GetFileList();

	}

	public void GetFileList()
	{
		//clear all previous data
		CleanPictureContainer();

		if (path != "")
		{
			DirectoryInfo dataDir = new DirectoryInfo(path);

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
		{
			ContainerText.gameObject.SetActive(true);

			ContainerText.text = "Path null";
		}
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

				/// convert texture to a sprite
				/// add this to image UI component
				/// preserve aspect

				//We need a parent rect which contains child rect in order to preserve ratio of the texture.
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
		//get the texture of the clicked image
		//show on full screen

		Texture my_texture = EventSystem.current.currentSelectedGameObject.GetComponent<RawImage>().texture;

		fullScreenPanel.SetActive(true);

		RawImage img = fullScreenPanel.transform.GetChild(0).GetComponent<RawImage>();

		img.texture = my_texture;

		PreserveRawImageApect.SizeToParent(img);
	}

	void CleanPictureContainer()
	{
		foreach (Transform child in pictureContainer.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

	 public void Capture()
    {
        StartCoroutine(TakeImage());
    }
    
    IEnumerator TakeImage()
    {  
        yield return new WaitForEndOfFrame();

        ShowImageAfterCapture();
    }

    private void ShowImageAfterCapture()
    {
        //get the texture
		//show on full screen

		texture = ScreenCapture.CaptureScreenshotAsTexture();

		fullScreenPanel.SetActive(true);

		RawImage img = fullScreenPanel.transform.GetChild(0).GetComponent<RawImage>();

		img.texture = texture;

		PreserveRawImageApect.SizeToParent(img);

    }

    public void SaveImage()
    {
        StartCoroutine(ProcessSave());
    }
    IEnumerator ProcessSave()
    {
        //temp save

        byte[] mediaBytes = texture.EncodeToPNG();

        string filename = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        NativeGallery.SaveImageToGallery(mediaBytes, album, filename, null);

        yield return new WaitForEndOfFrame();

		fullScreenPanel.SetActive(false);
		
		GetFileList();
    }

    public void DeleteImage()
    {
        fullScreenPanel.SetActive(false);
    }


}
