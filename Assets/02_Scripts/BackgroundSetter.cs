using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// îwåiâÊëúÇïœçXÇ∑ÇÈ
/// </summary>
public class BackgroundSetter : MonoBehaviour
{
    private Transform canvas;
    private Image backgroundImage;
    private Sprite[] backgroundSprites;
    private int currentIndex = 0;
    private ImageMaker imageMaker;
    // Start is called before the first frame update
    void Start()
    {
        canvas = transform;
        backgroundImage = GetComponentInChildren<Image>();
        backgroundSprites = Resources.LoadAll<Sprite>("Backgrounds");
        ReplaceBackground();
        imageMaker = Camera.main.gameObject.GetComponent<ImageMaker>();
        imageMaker.OnBeginCapture.AddListener(ReplaceBackground);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ReplaceBackground()
    {
        if (backgroundSprites.Length <= 0)
            return;
        int randIndex = Random.Range(0, backgroundSprites.Length);
        backgroundImage.sprite = backgroundSprites[randIndex];
    }
}
