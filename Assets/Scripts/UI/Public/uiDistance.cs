using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiDistance : MonoBehaviour
{
    TextMeshProUGUI txtDistance;
    Material mtTxtDistance;
    [SerializeField] Rigidbody rbPlayerControllPoint;

    // Start is called before the first frame update
    void Start()
    {
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
        txtDistance.fontSizeMax = 120f;
        while (true)
        {
            txtDistance.text = rbPlayerControllPoint.position.z.ToString("F2") + "m";
            mtTxtDistance.EnableKeyword("_Glow");
            mtTxtDistance.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Clamp01(rbPlayerControllPoint.position.z / 100f));
            txtDistance.fontSize = baseFontSize + Mathf.Clamp(Mathf.Abs(rbPlayerControllPoint.position.z)/2,-20f,100f);
            yield return null;
        }
    }

}
