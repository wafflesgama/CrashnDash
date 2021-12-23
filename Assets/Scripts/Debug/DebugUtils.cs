using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugUtils : MonoBehaviour
{
    [Title("Time properties","Change at debug time")]
    [Button("1x")] private void ChangeTimeNormal() => Time.timeScale = 1;
    [HorizontalGroup("Group 1")]
    [Button(".25x")] private void ChangeTimeQuarter() => Time.timeScale = .25f;
    [HorizontalGroup("Group 1")]
    [Button(".5x")] private void ChangeTimeHalf() => Time.timeScale = .5f;
    [HorizontalGroup("Group 1")]
    [Button("2x")] private void ChangeTimeDouble() => Time.timeScale = 2;
    [HorizontalGroup("Group 1")]
    [Button("4x")] private void ChangeTimeFourth() => Time.timeScale = 4;
}
