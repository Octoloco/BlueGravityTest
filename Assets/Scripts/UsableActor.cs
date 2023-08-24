using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsableActor
{
    public string usableName { get; }

    public void Action();
    public void ShowName();
    public void HideName();

}
