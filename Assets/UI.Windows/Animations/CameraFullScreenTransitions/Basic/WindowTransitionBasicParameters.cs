﻿using UnityEngine;
using UnityEngine.UI.Windows;

public class WindowTransitionBasicParameters : TransitionInputParameters {
	
	public WindowTransitionBasic.Parameters parameters;
	
	public override void SetDefaultParameters(TransitionBase.ParametersBase parameters) {
		
		this.parameters = new WindowTransitionBasic.Parameters(parameters);
		
	}
	
	public override TransitionBase.ParametersBase GetParameters() {
		
		return this.parameters;
		
	}
	
}
