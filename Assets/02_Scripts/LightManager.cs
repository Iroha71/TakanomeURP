using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���C�g�̓�������̐�����s��
/// </summary>
public class LightManager : MonoBehaviour
{
    private ImageMaker imageMaker;
    private Transform directionalLight;
    [Header("���z���̊p�x")]
    [SerializeField, Range(-180f, 270f)] private float minRotationDirectionalLight = -180f;
    [SerializeField, Range(-180f, 270f)] private float maxRotationDirectionalLight = 180f;
    // Start is called before the first frame update
    void Start()
    {
        imageMaker = GetComponent<ImageMaker>();
        directionalLight = GameObject.Find("Directional Light").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RotateDirectionalLight()
    {
        //float randRotate = Random.Range(minRotationDirectionalLight, maxRotationDirectionalLight);
        //directionalLight.eulerAngles = new 
    }
}
