#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor (typeof (BasicObjectDataSaver), true)]
	public class BasicObjectDataSaverEditor : ConstantIDEditor
	{
		BasicObjectDataSaver _target;
        private void OnEnable() {
            _target = (BasicObjectDataSaver)target;
        }
		public override void OnInspectorGUI()
		{
            _target.ShowGUI();
			SharedGUI ();
		}

	}

}

#endif