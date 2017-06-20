using UnityEngine;
using System.Collections;

public class AnimationUtil 
{
    public static IEnumerator TranslateWithScale(Transform fromTransform, Transform toTransform)
    {        
        while (true)
        {
            Vector3 fromScale = fromTransform.localScale;
            fromScale.x -= 0.1f;
            fromScale.y -= 0.1f;
            if (fromScale.x <= 0.0f)
            {
                fromScale.x = 0.01f;
                fromScale.y = 0.01f;
            }
            fromTransform.localScale = fromScale;

            if (fromScale.x <= 0.05f)
            {
                fromScale.x = 0.00f;
                fromScale.y = 0.00f;           
           
                fromTransform.localScale = fromScale;

                break;
            }

            yield return null;
        }


        toTransform.gameObject.SetActive (true);

        Vector3 toScale = toTransform.localScale;
        toScale.x = 0.02f;
        toScale.y = 0.02f;
        toTransform.localScale = toScale;

        while (true)
        {

            toScale = toTransform.localScale;
            toScale.x += 0.1f;
            toScale.y += 0.1f;
            toTransform.localScale = toScale;

            if (toScale.x >= 0.95f)
            {
                toScale.x = 1.00f;
                toScale.y = 1.00f;
                toTransform.localScale = toScale;
                break;
            }

            yield return null;
        }
        fromTransform.gameObject.SetActive (false);

        yield return null;
    }

}
