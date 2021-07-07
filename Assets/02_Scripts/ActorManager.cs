using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �B�e�I�u�W�F�N�g�̕ύX�����E�N���X�t�@�C���쐬���̊Ǘ����s��
/// </summary>
[RequireComponent(typeof(AppConfigManager))]
public class ActorManager : MonoBehaviour
{
    private List<ActorInfo> actors = new List<ActorInfo>();
    [SerializeField] public ActorInfo displayingActor = null;
    public string BOUNDING_BOX_TAGNAME { get { return "BoundingBox"; } }
    public ActorInfo DisplayActor { get { return displayingActor; } }
    private AppConfigManager appConfig;
    public UnityEvent<ActorInfo> OnSwitchedDisplayingActor = new UnityEvent<ActorInfo>();
    private List<Vector3> moveLimitCorners = new List<Vector3>();
    private Transform actorPutArea;
    // Start is called before the first frame update
    void Start()
    {
        appConfig = GetComponent<AppConfigManager>();
        actorPutArea = appConfig.transform;
        foreach (GameObject moveLimitCorner in GameObject.FindGameObjectsWithTag("MoveLimitCorner"))
        {
            moveLimitCorners.Add(moveLimitCorner.transform.position);
        }
        SetActorInfos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// �A�N�^�[��ύX����
    /// </summary>
    /// <param name="targetIndex">�ύX�������A�N�^�[�̃C���f�b�N�X</param>
    private void SwitchActor(int targetIndex)
    {
        if (targetIndex >= actors.Count)
            return;
        if (displayingActor != null)
            Destroy(displayingActor.Model);
        displayingActor = actors[targetIndex];
        GameObject _actor = Instantiate(displayingActor.Model, Vector3.zero, actorPutArea.rotation);
        _actor.transform.SetParent(actorPutArea);
        displayingActor.Model = _actor;
        displayingActor.Mesh = GameObject.FindGameObjectWithTag(BOUNDING_BOX_TAGNAME).GetComponent<Renderer>();
        OnSwitchedDisplayingActor.Invoke(displayingActor);
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// "Resources/Actors"�Ɋi�[���ꂽ�A�N�^�[�����X�g�Ƃ��Ď擾����
    /// </summary>
    private void SetActorInfos()
    {
        GameObject[] actorResources = Resources.LoadAll<GameObject>("Actors");
        for (int i = 0; i < actorResources.Length; i++)
        {
            actors.Add(new ActorInfo(actorResources[i].name, actorResources[i], i));
        }
        SwitchActor(0);
        CreateClassFile();
    }

    /// <summary>
    /// �A�N�^�[����]������
    /// </summary>
    /// <param name="minRotate">�ŏ��̊p�x</param>
    /// <param name="maxRotate">�ő�̊p�x</param>
    /// <param name="anglePerFrame">�t���[�����̉�]�p</param>
    public void RotateActor(float minRotate=0f, float maxRotate=360f, float moveAnglePerFrame=5f)
    {
        float nextAngle = displayingActor.Model.transform.localEulerAngles.y;
        
        if (nextAngle == maxRotate)
        {
            nextAngle = minRotate;
            Vector3 newAngle = new Vector3(0f, nextAngle, 0f);
            displayingActor.Model.transform.localEulerAngles = newAngle;
            return;
        }
        nextAngle += moveAnglePerFrame;
        if (nextAngle > maxRotate)
        {
            nextAngle = maxRotate;
        }
            
        else if (nextAngle < minRotate)
        {
            nextAngle = minRotate;
        }
        Vector3 nAngle = new Vector3(0f, nextAngle, 0f);
        displayingActor.Model.transform.localEulerAngles = nAngle;
    }
    
    public void MoveActorRandom()
    {
        int MIN = 0, MAX = 1;
        float x = Random.Range(moveLimitCorners[MIN].x, moveLimitCorners[MAX].x);
        float y = Random.Range(moveLimitCorners[MIN].y, moveLimitCorners[MAX].y);
        float z = Random.Range(moveLimitCorners[MIN].z, moveLimitCorners[MAX].z);

        displayingActor.Model.transform.position = new Vector3(x, y, z);
    }

    /// <summary>
    /// �N���X�t�@�C�����쐬����
    /// </summary>
    private void CreateClassFile()
    {
        string classText = "";
        foreach (ActorInfo actor in actors)
        {
            classText +=  actor.ClassId == actors.Count - 1 ? $"{ actor.Name }" : $"{ actor.Name }\n";
        }
        appConfig.WriteTextFile("class.txt", classText);
    }
}

[System.Serializable]
public class ActorInfo
{
    public string Name { get; set; }
    public GameObject Model { get; set; }
    public int ClassId { get; set; }
    public Renderer Mesh { get; set; }
    public ActorInfo(string name, GameObject model, int classId)
    {
        Name = name;
        Model = model;
        ClassId = classId;
    }
}