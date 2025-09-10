using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager
{
    public Mode currentMode{get; private set;}
    
    public List<ModeParameter> modeParameters{ get; private set;}

    public void Initialize()
    {
        modeParameters = new List<ModeParameter>();
        modeParameters.Add(ConstVariables.easyModeParameter);
        modeParameters.Add(ConstVariables.normalModeParameter);
        modeParameters.Add(ConstVariables.hardModeParameter);
    }

    public void SetMode(Mode mode)
    {
        currentMode = mode;
        
        switch (mode)
        {
            case Mode.easy:
                
                break;
            case Mode.nomal:
                
                break;
            case Mode.hard:
                
                break;
        }
    }
}

public enum Mode
{
    easy,
    nomal,
    hard
}
