﻿using UnityEngine;

public abstract class PneumaticComponentSimulation : MonoBehaviour {

    public virtual void Setup() { }
    public virtual void Stop() { }
    public abstract void RespondToSignal(Connector sourceConnector, float signal);
}
