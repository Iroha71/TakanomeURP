using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�m�e�[�V�������s���A�����L�^����
/// </summary>
public class AnnotateRecorder : MonoBehaviour
{
    private Camera mainCamera;
    private ActorManager actorManager;
    private AppConfigManager appConfig;
    private ImageMaker imageMaker;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        actorManager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<ActorManager>();
        appConfig = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppConfigManager>();
        imageMaker = Camera.main.gameObject.GetComponent<ImageMaker>();
        imageMaker.OnFinishedCreateImage.AddListener(WriteAnnotateInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// �A�m�e�[�V�����t�@�C�����쐬����
    /// </summary>
    /// <param name="classId">�N���XID</param>
    /// <param name="annotateInfos">�摜���̃A�m�e�[�V�����t�@�C����񃊃X�g</param>
    private void WriteAnnotateInfo(int classId, List<AnnotateInfo> annotateInfos, bool isExportImage)
    {
        if (!isExportImage)
            return;
        foreach (AnnotateInfo annotateInfo in annotateInfos)
        {
            string annotateText = classId.ToString();
            float normalizedX = CalcNormalizeSize(annotateInfo.boundsRect.x, Screen.width);
            float normalizedY = CalcNormalizeSize(annotateInfo.boundsRect.y, Screen.height);
            float normalizedWidth = CalcNormalizeSize(annotateInfo.boundsRect.width, Screen.width);
            float normalizedHeight = CalcNormalizeSize(annotateInfo.boundsRect.height, Screen.height);
            annotateText += $" { normalizedX } { normalizedY } { normalizedWidth } { normalizedHeight }";
            appConfig.WriteTextFile(annotateInfo.imageName + ".txt", annotateText);
        }
    }

    /// <summary>
    /// ���l�𑊑ΐ��ɕϊ�
    /// </summary>
    /// <param name="originSize">���X�̃T�C�Y</param>
    /// <param name="maxSize">�ő�l</param>
    /// <returns>���ΐ�</returns>
    private float CalcNormalizeSize(float originSize, float maxSize)
    {
        float normalizedSize = originSize / maxSize;

        return normalizedSize;
    }
}
