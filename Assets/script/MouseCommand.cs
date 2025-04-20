using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour
{
    public GameObject guideShapeUI;       // 화면에 띄울 도형 UI (ex. Sprite 또는 LineRenderer)
    public LineRenderer inputLine;        // 마우스로 그리는 궤적 시각화
    public KeyCode triggerKey = KeyCode.F;

    private bool isCommandMode = false;
    private List<Vector3> drawnPoints = new List<Vector3>();

    void Start()
    {
        guideShapeUI.SetActive(false); // 시작 시 꺼줌
    }

    void Update()
    {
        Debug.Log("🔄 Update 도는 중");

        if (Input.GetKeyDown(triggerKey))
        {
            EnterCommandMode();   // guideShapeUI.SetActive(true)
        }

        if (isCommandMode)
        {
            TrackMouseDrawing();

            if (Input.GetKeyUp(triggerKey))
            {
                ExitCommandMode();  // guideShapeUI.SetActive(false)
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
    }

    void ExitCommandMode()
    {
        isCommandMode = false;
        guideShapeUI.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // 궤적 인식 시도
        RecognizePattern(drawnPoints);
    }

    void TrackMouseDrawing()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            drawnPoints.Add(mousePos);

            inputLine.positionCount = drawnPoints.Count;
            inputLine.SetPositions(drawnPoints.ToArray());
        }
    }

    void RecognizePattern(List<Vector3> input)
    {
        // 👉 여기서 궤적을 미리 저장된 도형과 비교 (샘플 예시)
        // 유사하면 스킬 발동
        Debug.Log("Recognizing pattern...");
    }
}
