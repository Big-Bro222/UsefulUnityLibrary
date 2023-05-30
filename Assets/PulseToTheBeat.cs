using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseToTheBeat : MonoBehaviour
{
    [SerializeField] private bool useTestBeat;

    [SerializeField] private float pulseSize = 2f;

    [SerializeField] private float returnSpeed = 5f;

    private Vector3 _startSize;
    
    void Start()
    {
        _startSize = transform.localScale;
        // if (useTestBeat)
        // {
        //     StartCoroutine(TestBeat());
        // }
    }

    IEnumerator TestBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }

    public void Pulse()
    {
        transform.localScale = _startSize * pulseSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * returnSpeed);
    }
}
