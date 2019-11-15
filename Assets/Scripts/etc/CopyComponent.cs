using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CopyComponent : MonoBehaviour
{
    [SerializeField] bool Run=false;
    [SerializeField] Transform Origin, Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Run)
        {
            




            Run = false;
        }
    }
}
