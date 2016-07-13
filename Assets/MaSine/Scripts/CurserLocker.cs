using UnityEngine;
using System.Collections;

public class CurserLocker : MonoBehaviour
{
    CursorLockMode wantedMode;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    Destroy(this.gameObject);
#endif

        Cursor.visible = false;
    }

    // Apply requested cursor state
    void SetCursorState()
    {
        //Cursor.lockState = wantedMode;
        //// Hide cursor when locking
        //Cursor.visible = (CursorLockMode.Locked != wantedMode);
        //Cursor.visible = false;
    }
    int cnt = 0;
    void OnGUI()
    {
        //if (Input.GetMouseButtonDown(0)) {
        //    print("ss");
        //    Application.CaptureScreenshot("Screenshot" + cnt++ + "_" + System.DateTime.Now.Hour.ToString()
        //        + System.DateTime.Now.Minute.ToString() +
        //         System.DateTime.Now.Second.ToString() + ".png", 2);
        //}
        //GUILayout.BeginVertical();
        //// Release cursor on escape keypress
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Cursor.lockState = wantedMode = CursorLockMode.None;

        //switch (Cursor.lockState)
        //{
        //    case CursorLockMode.None:
        //        GUILayout.Label("Cursor is normal");
        //        if (GUILayout.Button("Lock cursor"))
        //            wantedMode = CursorLockMode.Locked;
        //        if (GUILayout.Button("Confine cursor"))
        //            wantedMode = CursorLockMode.Confined;
        //        break;
        //    case CursorLockMode.Confined:
        //        GUILayout.Label("Cursor is confined");
        //        if (GUILayout.Button("Lock cursor"))
        //            wantedMode = CursorLockMode.Locked;
        //        if (GUILayout.Button("Release cursor"))
        //            wantedMode = CursorLockMode.None;
        //        break;
        //    case CursorLockMode.Locked:
        //        GUILayout.Label("Cursor is locked");
        //        if (GUILayout.Button("Unlock cursor"))
        //            wantedMode = CursorLockMode.None;
        //        if (GUILayout.Button("Confine cursor"))
        //            wantedMode = CursorLockMode.Confined;
        //        break;
        //}

        //GUILayout.EndVertical();

        //SetCursorState();
    }
}