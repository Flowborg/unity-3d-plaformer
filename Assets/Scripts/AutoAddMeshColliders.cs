using UnityEngine;

public class AutoAddMeshColliders : MonoBehaviour
{
    [ContextMenu("Add Mesh Colliders to All Children")]
    void AddColliders()
    {
        int added = 0;

        // Get all MeshFilter components under this object
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);

        foreach (MeshFilter mf in meshFilters)
        {
            GameObject go = mf.gameObject;

            // Skip if a collider already exists
            if (go.GetComponent<Collider>() == null)
            {
                MeshCollider mc = go.AddComponent<MeshCollider>();
                mc.convex = false; // Don't use convex for large static meshes
                added++;
            }
        }

        Debug.Log($"✅ Added {added} MeshColliders to children of {gameObject.name}");
    }
}