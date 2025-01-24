#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor(typeof(InvActionList))]

	[System.Serializable]
	public class InvActionListEditor : ActionListAssetEditor
	{ }

}

#endif