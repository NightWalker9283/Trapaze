using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiDistance : MonoBehaviour
{
    public static UiDistance uiDistance;
    TextMeshProUGUI txtDistance;
    Material mtTxtDistance;
    [SerializeField] Rigidbody rbPlayerControllPoint;

    // Start is called before the first frame update
    void Start()
    {
        uiDistance = this;
        txtDistance = GetComponent<TextMeshProUGUI>();
        mtTxtDistance = txtDistance.fontSharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMessDistance()
    {
        StartCoroutine(MessDistanceProc());
    }

    IEnumerator MessDistanceProc()
    {
        var baseFontSize = txtDistance.fontSize;
        var canvasSize = PlayingManager.playingManager.cvsPublic.GetComponent<RectTransform>().sizeDelta;
        txtDistance.fontSizeMax = 120f;
        while (PlayingManager.playingManager.Stat != PlayingManager.Stat_global.result)
        {
            txtDistance.text = rbPlayerControllPoint.position.z.ToString("F2") + "m";
            mtTxtDistance.EnableKeyword("_Glow");
            mtTxtDistance.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Clamp01(Mathf.Abs(rbPlayerControllPoint.position.z) / 100f));
            txtDistance.fontSize = baseFontSize + Mathf.Clamp(Mathf.Abs(rbPlayerControllPoint.position.z) / 2, 0f, canvasSize.y * 0.25f);
            yield return null;
        }
    }
    public void Finish()
    {
        if (!PlayingManager.playingManager.isTraining) StartCoroutine(MoveToFinishPos());

    }

    IEnumerator MoveToFinishPos()
    {
        var fromSize = txtDistance.fontSize;
        var fromPosX = txtDistance.rectTransform.localPosition.x;
        var sizeCvsPublic = PlayingManager.playingManager.cvsPublic.GetComponent<RectTransform>().sizeDelta;
        var rect = GetComponent<RectTransform>();
        var toPosX = fromPosX - sizeCvsPublic.x / 2f + rect.sizeDelta.x / 2f + 15f;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            txtDistance.fontSize = Mathf.Lerp(fromSize, 36f, Mathf.Clamp01(t));
            toPosX = fromPosX - sizeCvsPublic.x / 2f + rect.sizeDelta.x / 2f + 15f;
            txtDistance.rectTransform.localPosition = new Vector3(
                Mathf.Lerp(fromPosX, toPosX, Mathf.Clamp01(t)),
                rect.localPosition.y
            );

            yield return null;
        }


    }
}
