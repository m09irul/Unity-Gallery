using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PathInputTaker : MonoBehaviour
{
    public GalleryManager gm;
    void Start()
    {
        var input = gameObject.GetComponent<TMP_InputField>();

        input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    public void SubmitName(string p)
    {
        Debug.Log(p);

        gm.path = p;

        gm.GetFileList();
    }
}


