#if UNITY_EDITOR

using UnityEditor;

namespace RomDev
{

	[CustomEditor(typeof(MenuActionList))]
	[System.Serializable]
	public class MenuActionListEditor : ActionListAssetEditor
	{ }

}

#endif