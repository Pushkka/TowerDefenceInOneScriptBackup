using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomYrot : MonoBehaviour
{
    // Start is called before the first frame update
    public float minHeight = 1 ;
    public float maxHeight = 1;
    void Start()
    {

    }
    private void Update()
    {
        Vector3 rot = new Vector3(0, Random.Range(0, 360));
        transform.rotation = Quaternion.Euler(rot);
        float height = Random.Range(minHeight, maxHeight);
        transform.localScale = new Vector3(height, height, height);
    }
}
