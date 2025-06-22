using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour
{
    public GameObject guideShapeUI;       // 화면에 띄울 도형 UI
    public LineRenderer inputLine;        // 궤적 시각화
    public KeyCode triggerKey = KeyCode.F;
    public Transform target;              // ✅ 궤적을 그릴 기준 오브젝트

    private bool isCommandMode = false;
    private List<Vector3> drawnPoints = new List<Vector3>();

    void Start()
    {
        guideShapeUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            EnterCommandMode();
        }

        if (isCommandMode)
        {
            TrackMouseDrawing();

            // ✅ 도형이 target을 따라가게 설정
            if (target != null && guideShapeUI != null)
            {
                Vector3 targetPos = target.position + new Vector3(0f, 1f, 0f); // 머리 위
                guideShapeUI.transform.position = targetPos;
            }

            if (Input.GetKeyUp(triggerKey))
            {
                ExitCommandMode();
            }
        }
    }

    void EnterCommandMode()
    {
        isCommandMode = true;
        Debug.Log("✅ F 키 입력됨: EnterCommandMode 실행");
        guideShapeUI.SetActive(true);
        inputLine.positionCount = 0;
        drawnPoints.Clear();

        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        Debug.Log("guideShapeUI is " + (guideShapeUI == null ? "NULL ❌" : "CONNECTED ✅"));
        Debug.Log("target is " + (target == null ? "NULL ❌" : "CONNECTED ✅"));
    }

    void ExitCommandMode()
    {
        isCommandMode = false;
        guideShapeUI.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        RecognizePattern(drawnPoints);
    }

    void TrackMouseDrawing()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 point;

            // ✅ target이 지정되어 있으면 target 위치를 사용
            if (target != null)
            {
                point = target.position;
            }
            else
            {
                point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            point.z = 0f;
            drawnPoints.Add(point);

            inputLine.positionCount = drawnPoints.Count;
            inputLine.SetPositions(drawnPoints.ToArray());
        }
    }

    void RecognizePattern(List<Vector3> input)
    {
        Debug.Log("Recognizing pattern...");
    }
}
