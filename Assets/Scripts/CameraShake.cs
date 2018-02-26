using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public IEnumerator Shake(float duration, float magniture)
    {
        Vector3 originalPos = transform.localPosition;

            float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.RandomRange(-1f, 1f) * magniture;
            float y = Random.RandomRange(-1f, 1f) * magniture;

            transform.localPosition = new Vector3(x+originalPos.x, y+originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
