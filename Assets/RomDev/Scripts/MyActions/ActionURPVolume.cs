/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2023
 *	
 *	"ActionTemplate.cs"
 * 
 *	This is a blank action template.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RomDev
{

	[System.Serializable]
	public class ActionURPVolume : Action
	{
		
		// Declare properties here
		public override ActionCategory Category { get { return ActionCategory.Engine; }}
		public override string Title { get { return "URP Volume"; }}
		public override string Description { get { return "Control URP Volume"; }}


		// Declare variables here
        public UnityEngine.Rendering.Volume volume;
        public ActionMode actionMode;
        public TargetOverride targetOverride;
        public OverrideControlMode overrideControlMode;
        public enum ActionMode 
        {
            ControlWeight,
            ControlOverride
        }
        public enum TargetOverride
        {
            Vignette,
            DepthOfField
        }
        public enum OverrideControlMode
        {
            EnableDisable,
            ControlValue
        }
        public bool isEnable;
		public override float Run ()
		{
			/* 
			 * This function is called when the action is performed.
			 * 
			 * The float to return is the time that the game
			 * should wait before moving on to the next action.
			 * Return 0f to make the action instantenous.
			 * 
			 * For actions that take longer than one frame,
			 * you can return "defaultPauseTime" to make the game
			 * re-run this function a short time later. You can
			 * use the isRunning boolean to check if the action is
			 * being run for the first time, eg: 
			 if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			else
			{
				isRunning = false;
				return 0f;
			}
			*/
            switch(actionMode)
            {
                case ActionMode.ControlWeight:
                break;
                case ActionMode.ControlOverride:
                RunControlOverride();
                break;
            }
			return 0f;
		}


		public override void Skip ()
		{
			/*
			 * This function is called when the Action is skipped, as a
			 * result of the player invoking the "EndCutscene" input.
			 * 
			 * It should perform the instructions of the Action instantly -
			 * regardless of whether or not the Action itself has been run
			 * normally yet.  If this method is left blank, then skipping
			 * the Action will have no effect.  If this method is removed,
			 * or if the Run() method call is left below, then skipping the
			 * Action will cause it to run itself as normal.
			 */

			 Run();
		}

		
		#if UNITY_EDITOR

		public override void ShowGUI ()
		{
			// Action-specific Inspector GUI code here
            volume = (UnityEngine.Rendering.Volume)EditorGUILayout.ObjectField("Volume: ",volume, typeof(UnityEngine.Rendering.Volume), true);
            actionMode = (ActionMode) EditorGUILayout.EnumPopup("Action Mode: ", actionMode);
            switch(actionMode)
            {
                case ActionMode.ControlWeight:
                break;
                case ActionMode.ControlOverride:
                DrawControlOverride();
                break;
            }
		}
		

		public override string SetLabel ()
		{
			// (Optional) Return a string used to describe the specific action's job.
			
			return string.Empty;
		}
        private void DrawControlOverride()
        {
            targetOverride = (TargetOverride)EditorGUILayout.EnumPopup("Override :", targetOverride);
            overrideControlMode = (OverrideControlMode)EditorGUILayout.EnumPopup("Control Mode :", overrideControlMode);
            switch (overrideControlMode)
            {
                case OverrideControlMode.EnableDisable:
                isEnable = EditorGUILayout.Toggle("Is Enabled ? ", isEnable);
                break;
            }
        }

		#endif
        private void RunControlOverride()
        {
            switch (overrideControlMode)
            {
                case OverrideControlMode.EnableDisable:
                EnableDisableOverride(isEnable);
                break;
            }
        }
        private void EnableDisableOverride(bool _isEnable)
        {
            UnityEngine.Rendering.VolumeComponent volumeComp = null;
            switch(targetOverride)
            {
                case TargetOverride.Vignette:
                if( ! volume.profile.TryGet( out UnityEngine.Rendering.Universal.Vignette vignette))
                {
                    return;
                }
                volumeComp = vignette;
                break;
                case TargetOverride.DepthOfField:
                if( ! volume.profile.TryGet( out UnityEngine.Rendering.Universal.DepthOfField depthOfField))
                {
                    return;
                }
                volumeComp = depthOfField;
                break;
            }
            if (volumeComp == null) return;
            volumeComp.active = _isEnable;
        }
		
	}

}