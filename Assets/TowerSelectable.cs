using UnityEngine;

public class TowerSelectable : MonoBehaviour
{
    public void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tower Base"));
        
        if (hit.collider) {
            LevelManager.main.setSelectedTower(hit.collider.GetComponentInParent<Tower>().gameObject);
        }
        
    }

}
