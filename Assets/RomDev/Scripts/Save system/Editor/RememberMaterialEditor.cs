#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor (typeof (RememberMaterial), true)]
	public class RememberMaterialEditor : ConstantIDEditor
	{

		public override void OnInspectorGUI ()
		{
			RememberMaterial _target = (RememberMaterial) target;
			_target.ShowGUI ();
			SharedGUI ();
		}

	}

}

#endif