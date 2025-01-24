#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor (typeof (RememberSceneItem), true)]
	public class RememberSceneItemEditor : ConstantIDEditor
	{

		public override void OnInspectorGUI ()
		{
			RememberSceneItem _target = (RememberSceneItem) target;
			_target.ShowGUI ();
			SharedGUI ();
		}

	}

}

#endif