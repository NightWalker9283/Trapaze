using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
    private Camera _camera;
    private Matrix4x4 ortho,
                        perspective;
    public float fov = 2f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 50f;
    private float aspect;
    private MatrixBlender blender;
    private bool orthoOn;

    void Start()
    {
        _camera = GetComponent<Camera>();
        orthographicSize = _camera.orthographicSize;
        near = _camera.nearClipPlane;
        far = _camera.farClipPlane;
        fov = _camera.fieldOfView;


        aspect = _camera.aspect;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        _camera.projectionMatrix = ortho;
        orthoOn = true;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
    }

    public void SwitchToPerspectiveMode()
    {
        blender.BlendToMatrix(perspective, 2f);
        
    }
}