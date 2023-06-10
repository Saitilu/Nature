using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCam : MonoBehaviour
{
    const int MAX_FPS = 60;

    [SerializeField] Transform leader;
    [SerializeField] float lagSeconds;

    Vector3[] _positionBuffer;
    Quaternion[] _rotationBuffer;
    float[] _timeBuffer;
    int _oldestIndex;
    int _newestIndex;

    // Use this for initialization
    void Start()
    {
        int bufferLength = Mathf.CeilToInt(lagSeconds * MAX_FPS);
        _positionBuffer = new Vector3[bufferLength];
        _rotationBuffer = new Quaternion[bufferLength];
        _timeBuffer = new float[bufferLength];

        _positionBuffer[0] = _positionBuffer[1] = leader.position;
        _rotationBuffer[0] = _rotationBuffer[1] = leader.rotation;
        _timeBuffer[0] = _timeBuffer[1] = Time.time;

        _oldestIndex = 0;
        _newestIndex = 1;
    }


    void FixedUpdate()
    {
        // Insert newest position into our cache.
        // If the cache is full, overwrite the latest sample.
        int newIndex = (_newestIndex + 1) % _positionBuffer.Length;
        if (newIndex != _oldestIndex)
            _newestIndex = newIndex;

        _positionBuffer[_newestIndex] = leader.position;
        _rotationBuffer[_newestIndex] = leader.rotation;
        _timeBuffer[_newestIndex] = Time.time;

        // Skip ahead in the buffer to the segment containing our target time.
        float targetTime = Time.time - lagSeconds;
        int nextIndex;
        while (_timeBuffer[nextIndex = (_oldestIndex + 1) % _timeBuffer.Length] < targetTime)
            _oldestIndex = nextIndex;

        // Interpolate between the two samples on either side of our target time.
        float span = _timeBuffer[nextIndex] - _timeBuffer[_oldestIndex];
        float progress = 0f;
        if (span > 0f)
        {
            progress = (targetTime - _timeBuffer[_oldestIndex]) / span;
        }

        transform.position = Vector3.Lerp(_positionBuffer[_oldestIndex], _positionBuffer[nextIndex], progress);
        transform.rotation = Quaternion.Lerp(_rotationBuffer[_oldestIndex], _rotationBuffer[nextIndex], progress);
    }

    void OnDrawGizmos()
    {
        if (_positionBuffer == null || _positionBuffer.Length == 0)
            return;

        Gizmos.color = Color.grey;

        Vector3 oldPosition = _positionBuffer[_oldestIndex];
        int next;
        for (int i = _oldestIndex; i != _newestIndex; i = next)
        {
            next = (i + 1) % _positionBuffer.Length;
            Vector3 newPosition = _positionBuffer[next];
            Gizmos.DrawLine(oldPosition, newPosition);
            oldPosition = newPosition;
        }
    }
}
