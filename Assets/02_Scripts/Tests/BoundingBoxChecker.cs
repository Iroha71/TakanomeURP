using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundingBoxChecker : MonoBehaviour
{
    public BoxCollider col;
    public Renderer mesh;
    private ImageMaker imageMaker;
    public GameObject colBox;
    public GameObject meshBox;
    // Start is called before the first frame update
    void Start()
    {
        imageMaker = GetComponent<ImageMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetBoundingBoxImage();
        }
    }
    public Transform potato;
    private void SetBoundingBoxImage()
    {
        colBox.transform.position = transform.TransformPoint(col.center);
        colBox.transform.localScale = col.size;
        meshBox.transform.position = mesh.bounds.center;
        meshBox.transform.localScale = mesh.bounds.size;
        print(potato.TransformPoint(col.center));
    }
}
