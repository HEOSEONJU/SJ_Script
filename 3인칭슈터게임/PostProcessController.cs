using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessController : MonoBehaviour
{
    public AnimationCurve animationCurve;
    private DepthOfField depthOfField;
    private PostProcessVolume processVolume;
    // Start is called before the first frame update
    void Start()
    {
        processVolume = GetComponent<PostProcessVolume>();
        if( processVolume != null )
        {
            // processVolume.sharedProfile.TryGetSettings<DepthOfField>(out depthOfField );
            processVolume.profile.TryGetSettings<DepthOfField>(out depthOfField);
        }
    }

    private IEnumerator IEDepthOfField( float speed, float start, float end )
    {
        float elapsedTime = 0;
        //depthOfField.enabled.Override(true);
        depthOfField.enabled.value = false;
        depthOfField.focusDistance.value = start;
        depthOfField.active = true;
        while ( true )
        {
            elapsedTime +=Time.deltaTime /speed;
            float delta = Mathf.Lerp(start, end, elapsedTime );
            depthOfField.focusDistance.value = delta;
            if ( elapsedTime >= 1.0f )
                break;
            yield return null;
        }
        //depthOfField.active = false;
    }
    public void ExecuteDepthOfField( float speed, float start, float end )
    {
        // PostProcessController 계층에서 실행되고 있는 모든 코루틴을 종료합니다.
        StopAllCoroutines();
        // DepthOfField효과를 실행합니다.
        StartCoroutine(IEDepthOfField(speed, start, end));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ExecuteDepthOfField(1, 50, 0.1f);


    }
}
