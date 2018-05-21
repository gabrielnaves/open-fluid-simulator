﻿using UnityEngine;

public class DoubleActingCylinderSimulation : PneumaticComponentSimulation {

    CylinderEditing cylinderEditing;
    ComponentReferences componentReferences;

    Transform[] cylinderParts;
    Vector3[] originalPositions;

    float movementSpeed;
    float maxDisplacement;
    float displacement;
    float[] signals = new float[2];
    bool simulating;

    void Awake() {
        componentReferences = GetComponent<ComponentReferences>();
        cylinderEditing = GetComponent<CylinderEditing>();
    }

    public override void Setup() {
        cylinderParts = cylinderEditing.cylinderParts;
        originalPositions = cylinderEditing.originalPositions;
        maxDisplacement = cylinderEditing.GetMaxDisplacement();
        movementSpeed = maxDisplacement / cylinderEditing.movementTime;
        displacement = cylinderEditing.startingPercentage * maxDisplacement;
        simulating = true;
    }

    public override void Stop() {
        cylinderEditing.UpdateSprites();
        simulating = false;
    }

    public override void RespondToSignal(Connector sourceConnector, float signal) {
        var connectorList = componentReferences.connectorList;
        int sourceConnectorIndex = connectorList.IndexOf(sourceConnector);
        signals[sourceConnectorIndex] = signal;
    }

    void FixedUpdate() {
        if (simulating) {
            bool move = signals[0] != signals[1];
            movementSpeed = Mathf.Abs(movementSpeed) * (signals[0] > signals[1] ? 1f : -1f);
            if (move) {
                displacement += movementSpeed * Time.fixedDeltaTime;
                displacement = Mathf.Clamp(displacement, 0, maxDisplacement);
                for (int i = 0; i < cylinderParts.Length; ++i)
                    cylinderParts[i].position = originalPositions[i] + transform.right * displacement;
            }
        }
    }
}
