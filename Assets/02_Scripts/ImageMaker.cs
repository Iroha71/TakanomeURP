using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System.IO;
using System;
using UnityEngine.UI;

/// <summary>
/// �摜�𐶐�����
/// </summary>
public class ImageMaker : MonoBehaviour
{
    private Camera mainCamera;
    private ActorManager actorManager;
    private List<AnnotateInfo> annotateInfos = new List<AnnotateInfo>();
    public UnityEvent<int, List<AnnotateInfo>, bool> OnFinishedCreateImage = new UnityEvent<int, List<AnnotateInfo>, bool>();
    private AppConfigManager appConfig;
    private int capturedCount = 0;
    private bool isCapturing = false;
    [SerializeField] private bool isExportImage = true;
    [Header("�B�e�ݒ�(�A�N�^�[)")]
    [SerializeField, Range(-360f, 360f)] private float[] angleRange = { 0f, 360f };
    [SerializeField, Tooltip("1�t���[��������̉�]�p")] private float rotateAnglePerFrame = 5f;
    public UnityEvent OnBeginCapture = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        actorManager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<ActorManager>();
        appConfig = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppConfigManager>();
        actorManager.OnSwitchedDisplayingActor.AddListener(ResetAnnotateInfo);
    }

    private void ResetAnnotateInfo(ActorInfo displayActor)
    {
        capturedCount = 0;
        annotateInfos = new List<AnnotateInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCapturing = true;
            CreateImage();
        }

        if (isCapturing)
        {
            CreateImage();
        }

    }

    private void CreateImage()
    {
        if (capturedCount >= appConfig.CreateImageNum)
        {
            isCapturing = false;
            Debug.Log($"�B�e�I��: { appConfig.CreateImageNum }���̉摜�𐶐����܂���");
            OnFinishedCreateImage.Invoke(actorManager.DisplayActor.ClassId, annotateInfos, isExportImage);
            capturedCount = 0;
            return;
        }
        
        OnBeginCapture.Invoke();
        actorManager.RotateActor(angleRange[0], angleRange[1], rotateAnglePerFrame);
        actorManager.MoveActorRandom();
        string imageName = actorManager.DisplayActor.ClassId.ToString() + Time.frameCount.ToString();
        AnnotateInfo annotateInfo = new AnnotateInfo();
        annotateInfo.imageName = imageName;
        Rect annotateRect = GetBoundingBoxFromMesh(actorManager.DisplayActor.Mesh);
        annotateInfo.boundsRect = annotateRect;
        annotateInfos.Add(annotateInfo);
        StartCoroutine(ShotImageScreen(Path.Combine(appConfig.SaveFilePath, imageName + ".jpg")));
        
        capturedCount++;
    }

    private IEnumerator ShotImageScreen(string fileSavePath)
    {
        if (!isExportImage)
            yield return null;

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        texture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        File.WriteAllBytes(fileSavePath, texture.EncodeToJPG());
    }

    /// <summary>
    /// �o�E���f�B���O�{�b�N�X�̃T�C�Y�ƍ��W���擾����
    /// </summary>
    /// <param name="bounds">�Ώۂ̃o�E���f�B���O�{�b�N�X</param>
    /// <returns>��ʏ�̃o�E���f�B���O�{�b�N�X���W</returns>
    public Rect GetBoundingBoxFromMesh(Renderer mesh)
    {
        Bounds bounds = mesh.bounds;
        Vector2 centerPositionOnScreen = RectTransformUtility.WorldToScreenPoint(mainCamera, bounds.center);
        Vector2 min = RectTransformUtility.WorldToScreenPoint(mainCamera, new Vector3(bounds.min.x, bounds.min.y, bounds.center.z));
        Vector2 max = RectTransformUtility.WorldToScreenPoint(mainCamera, new Vector3(bounds.max.x, bounds.max.y, bounds.center.z));

        return new Rect(centerPositionOnScreen.x, Screen.height - centerPositionOnScreen.y, max.x - min.x, max.y - min.y);
    }
}
