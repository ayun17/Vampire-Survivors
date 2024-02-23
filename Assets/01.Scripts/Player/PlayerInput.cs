using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> onMovementKeyPressed = null;
    //public UnityEvent<Vector2> onFirePressed = null;

    private Camera _mainCam;
    void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        GetMovementInput();
        //FireInput();
    }

    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y);
        onMovementKeyPressed?.Invoke(direction.normalized);
    }

    //private void FireInput() // 테스트 함수
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        mousePos.z = 0;
    //        Vector2 mousePosInWorld = (Vector2)_mainCam.ScreenToWorldPoint(mousePos);

    //        onFirePressed?.Invoke(mousePosInWorld);
    //    }
    //}
}
