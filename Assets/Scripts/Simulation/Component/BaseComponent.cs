﻿using UnityEngine;

/// <summary>
/// Provides selection, movement, rotation and deletion for electric/pneumatic components
/// </summary>
/// This script reunites the implementation for each editing action on
/// a single MonoBehaviour. This method results in a cleaner game object
/// for electric and pneumatic components.
public class BaseComponent : MonoBehaviour, ISelectable, IDraggable {

    new Collider2D collider;
    SpriteRenderer spriteRenderer;
    ComponentMove componentMove;

    public bool RequestedSelect() {
        return SimulationInput.instance.singleClick && collider.OverlapPoint(SimulationInput.instance.mousePosition);
    }

    public bool IsInsideSelectionBox(Collider2D selectionBox) {
        return selectionBox.IsTouching(collider);
    }

    public void OnSelect() {
        spriteRenderer.color = new Color32(191, 186, 255, 255);
    }

    public void OnDeselect() {
        spriteRenderer.color = Color.white;
    }

    public bool RequestedDrag() {
        return SimulationInput.instance.mouseDragStart && collider.OverlapPoint(SimulationInput.instance.startingDragPoint);
    }

    public void StartDragging() {
        if (!SelectedObjects.instance.IsSelected(this)) {
            SelectedObjects.instance.ClearSelection();
            SelectedObjects.instance.SelectComponent(this);
        }
        componentMove.StartMoving();
    }

    public void StopDragging() {
        componentMove.StopMoving();
    }

    void Awake() {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        componentMove = new ComponentMove();
    }
    
    void Update() {
        if (componentMove.moving)
            componentMove.Update();
    }
}
