using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Test : MonoBehaviour
{
    public GalleryManager gm;
    void Start()
    {
        var input = gameObject.GetComponent<TMP_InputField>();

        input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    public void SubmitName(string arg0)
    {
        Debug.Log(arg0);

        gm.path = arg0;

        gm.GetFileList();
    }
}


