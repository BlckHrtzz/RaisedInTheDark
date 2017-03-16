using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { set; get; }

    public Image fadeImage;

    private bool isInTransition;
    private float duration;
    private float transition;
    private bool isFading;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fade(true, 1.25f);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Fade(false, 3f);
        }

        if (!isInTransition)
            return;

        transition += (isFading) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.black, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    void Fade(bool _isFading, float _duration)
    {
        isFading = _isFading;
        isInTransition = true;
        duration = _duration;
        transition = (isFading) ? 0 : 1;
    }
}
