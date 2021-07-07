using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アノテーションを行い、情報を記録する
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
    /// アノテーションファイルを作成する
    /// </summary>
    /// <param name="classId">クラスID</param>
    /// <param name="annotateInfos">画像毎のアノテーションファイル情報リスト</param>
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
    /// 数値を相対数に変換
    /// </summary>
    /// <param name="originSize">元々のサイズ</param>
    /// <param name="maxSize">最大値</param>
    /// <returns>相対数</returns>
    private float CalcNormalizeSize(float originSize, float maxSize)
    {
        float normalizedSize = originSize / maxSize;

        return normalizedSize;
    }
}
