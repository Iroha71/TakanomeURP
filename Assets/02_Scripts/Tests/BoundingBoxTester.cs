using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundingBoxTester : MonoBehaviour
{
    private ActorManager actorManager;
    private Transform testerCanvas;
    private Image pointImage;
    [SerializeField] private GameObject pointImagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        actorManager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<ActorManager>();
        testerCanvas = GameObject.Find("TesterCanvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Rect rect = GetBoundingBox(actorManager.DisplayActor.Mesh);
            DrawDetectResult(rect);
        }
    }

    private Rect GetBoundingBox(Renderer mesh)
    {
        Bounds bounds = mesh.bounds;
        
        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, bounds.center);
        Vector2 extentPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, bounds.extents);
        Vector2 min = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(bounds.min.x, bounds.min.y, bounds.center.z));
        Vector2 max = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(bounds.max.x, bounds.max.y, bounds.center.z));

        return new Rect(position.x, position.y, 10f, 10f);
    }

    private void DrawDetectResult(Rect rect)
    {
        if (!pointImage)
        {
            GameObject __pointImage = Instantiate(pointImagePrefab, testerCanvas, false);
            pointImage = __pointImage.GetComponent<Image>();
        }
        Debug.Log(rect);
        pointImage.rectTransform.anchoredPosition = new Vector2(rect.x, rect.y - Screen.height);
    }

    //private void CalcBoundingBoxSizeOnScreen()
    //{
    //    modelBox = GameObject.FindGameObjectWithTag(BOUNDING_BOX_TAGNAME).GetComponent<Renderer>();
    //    bounds = modelBox.bounds;
    //    Bounds b = modelBox.bounds;
    //    BoundingBox.transform.position = modelBox.transform.TransformPoint(b.center);
    //    //Vector3 center = Camera.main.WorldToScreenPoint(modelBox.transform.TransformPoint(new Vector3(b.min.x, b.max.y, b.center.z)));
    //    //Vector3 extent = Camera.main.WorldToScreenPoint(modelBox.transform.TransformPoint(new Vector3(b.max.x, b.min.y, b.center.z)));
    //    Vector3 center = Camera.main.WorldToScreenPoint(new Vector3(b.min.x, b.max.y, 0f));
    //    Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(b.max.x, b.min.y, b.center.z));



    //    Rect rect = new Rect(center.x, Screen.height - center.y, extent.x - center.x, center.y - extent.y);

    //    Debug.Log(rect);
    //}
}
