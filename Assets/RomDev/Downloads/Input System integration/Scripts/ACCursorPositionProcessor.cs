#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.InputSystem;
using UnityEngine;

namespace RomDev
{

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class ACCursorPositionProcessor : InputProcessor<Vector2>
	{

#if UNITY_EDITOR
		static ACCursorPositionProcessor ()
		{
			Initialize ();
		}
#endif

		public override Vector2 Process (Vector2 value, InputControl control)
		{
			return KickStarter.playerInput.GetMousePosition ();
		}

		[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Initialize ()
		{
			InputSystem.RegisterProcessor<ACCursorPositionProcessor> ();
		}

	}

}