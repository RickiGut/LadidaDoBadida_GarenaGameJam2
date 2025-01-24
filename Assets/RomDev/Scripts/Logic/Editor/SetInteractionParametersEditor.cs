#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor (typeof (SetInteractionParameters))]
	public class SetInteractionParametersEditor : Editor
	{

		private SetInteractionParameters _target;


		public override void OnInspectorGUI ()
		{
			_target = (SetInteractionParameters) target;

			_target.ShowGUI ();

			UnityVersionHandler.CustomSetDirty (_target);
		}
		
	}

}

#endif