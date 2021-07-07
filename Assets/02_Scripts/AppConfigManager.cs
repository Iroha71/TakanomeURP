using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

/// <summary>
/// �c�[���S�̂̐ݒ���Ǘ�����
/// </summary>
public class AppConfigManager : MonoBehaviour
{
    [SerializeField] private float screenSize = 416;
    [SerializeField] private int createImageNum = 1;
    [SerializeField] private string saveFilePath = "";
    public int CreateImageNum
    {
        get { return createImageNum; }
    }
    public string SaveFilePath { 
        get 
        { 
            return this.saveFilePath != "" ? this.saveFilePath : Path.Combine(Application.dataPath, "Results"); ; 
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (Screen.width != screenSize || Screen.height != screenSize)
        {
            EditorApplication.isPlaying = false;
            Debug.LogAssertion($"��ʃT�C�Y��{ screenSize } x { screenSize }�ɂ��Ă�������");
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteTextFile(string fileName, string writeText)
    {
        string saveFilePath = this.saveFilePath != "" ? this.saveFilePath : Path.Combine(Application.dataPath, "Results");

        File.WriteAllText(Path.Combine(saveFilePath, fileName), writeText);
    }
}
