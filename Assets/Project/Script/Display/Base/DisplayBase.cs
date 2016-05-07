using UnityEngine;
using System.Collections;

public class DisplayBase : MonoBehaviour 
{
    protected IEnumerator translateCoroutine = null;

    private void OnDisable()
    {
        this.translateCoroutine = null;
    }

}
