﻿using UnityEngine;

/// <summary>
/// Action for creating a new component on the simulation pane
/// </summary>
/// The newly-instantiated game object is then added to the SimulationPane.
public class NewComponentAction : IAction {

    GameObject componentPrefab;
    Vector3 componentPosition;

    BaseComponent createdComponent;

    public NewComponentAction(GameObject componentPrefab, Vector3 componentPosition) {
        this.componentPrefab = componentPrefab;
        this.componentPosition = componentPosition;
    }

    public void DoAction() {
        createdComponent = Object.Instantiate(componentPrefab).GetComponent<BaseComponent>();
        createdComponent.transform.position = componentPosition;
        createdComponent.name = createdComponent.name.Replace("(Clone)", "");
        SimulationPanel.instance.AddComponent(createdComponent);
    }

    public void UndoAction() {
        createdComponent.gameObject.SetActive(false);
        SimulationPanel.instance.RemoveComponent(createdComponent);
        if (SelectedObjects.instance.IsSelected(createdComponent))
            SelectedObjects.instance.DeselectObject(createdComponent);
    }

    public void RedoAction() {
        createdComponent.gameObject.SetActive(true);
        SimulationPanel.instance.AddComponent(createdComponent);
    }

    public void OnDestroy() {
        Object.Destroy(createdComponent.gameObject);
    }

    public string Name() {
        return "New component: " + createdComponent.name;
    }
}
