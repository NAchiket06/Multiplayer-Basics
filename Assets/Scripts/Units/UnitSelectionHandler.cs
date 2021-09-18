using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform SelectionArea = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Vector2 startPos;

    private Camera mainCamera;
    private RTSPlayer player;



    [SerializeField] public List<Unit> SelectedUnits { get; } = new List<Unit>();
    private void Start()
    {
        mainCamera = Camera.main;
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }

    private void Update()
    {

        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //start selection area
            StartSelectionArea();
           
        }

        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            //stop selection area
            ClearSelectionArea();
        }

        else if(Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        SelectionArea.gameObject.SetActive(true);

        startPos = Mouse.current.position.ReadValue();

    }
    void UpdateSelectionArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float areaWidth = mousePos.x - startPos.x;
        float areaHeight = mousePos.y - startPos.y;

        SelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs( areaHeight));
        SelectionArea.anchoredPosition = startPos + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        SelectionArea.gameObject.SetActive(false);

        if (SelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return;
            }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit))
            {
                return;
            }
            if (!unit.hasAuthority)
            {
                return;
            }

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }

            return;
        }

        Vector2 min = SelectionArea.anchoredPosition - (SelectionArea.sizeDelta / 2);
        Vector2 max = SelectionArea.anchoredPosition + (SelectionArea.sizeDelta / 2);

        foreach (Unit unit in player.GetPlayerUnits())
        {

            if(SelectedUnits.Contains(unit))
            {
                continue;
            }
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);
            if (screenPos.x > min.x &&
               screenPos.x < max.x &&
               screenPos.y > min.y &&
               screenPos.y < max.y)
            {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }

    }
}
