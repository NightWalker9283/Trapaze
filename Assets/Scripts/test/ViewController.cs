using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour
{
    private RectTransform cacheRectTransform;

    public RectTransform CachedRectTransform
    {
        get
        {
            if (cacheRectTransform == null)
            { cacheRectTransform = GetComponent<RectTransform>(); }
            return cacheRectTransform;
        }
    }

    public virtual string Title { get { return ""; } set { } }

}
