#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor (typeof (RememberAnimator), true)]
	public class RememberAnimatorEditor : ConstantIDEditor
	{
		
		public override void OnInspectorGUI ()
		{
			RememberAnimator _target = (RememberAnimator) target;
			_target.ShowGUI ();
			SharedGUI ();
		}
		
	}
	
}

#endif