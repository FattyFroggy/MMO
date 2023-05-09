using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Collider minimapBoundingbox;
    // Start is called before the first frame update
    void Start()
    {
        MinimapManager.Instance.UpdateMinimap(minimapBoundingbox);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
