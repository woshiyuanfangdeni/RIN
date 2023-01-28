using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomType : MonoBehaviour
{
    public int type;

    public void roomDestroy()
    {
        Destroy(gameObject);
    }
}
