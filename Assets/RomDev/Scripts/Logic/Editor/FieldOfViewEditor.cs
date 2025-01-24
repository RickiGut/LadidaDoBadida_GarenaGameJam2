using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		// FieldOfView fow = (FieldOfView)target;
		// Handles.color = Color.white;
		// Handles.DrawWireArc (fow.transform.position, Vector2.zero, Vector2.right, 360, fow.viewRadius);
        // Handles.DrawWireArc();
		// Vector2 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
		// Vector2 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

		// Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		// Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		// Handles.color = Color.red;
		// foreach (Transform visibleTarget in fow.visibleTargets) {
		// 	Handles.DrawLine (fow.transform.position, visibleTarget.position);
		// }
	}

}