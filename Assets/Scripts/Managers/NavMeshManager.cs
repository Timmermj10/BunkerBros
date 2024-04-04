using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    // Reference to the NavMeshSurface
    private NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to AirDropLanded Events
        EventBus.Subscribe<AirdropLandedEvent>(_updateSurfaceCreate);

        // Subscribe to ObjectDestroyedEvents
        EventBus.Subscribe<ObjectDestroyedEvent>(_updateSurfaceDestroy);

        // Grab reference to the surface
        surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
    }

   public void _updateSurfaceCreate(AirdropLandedEvent e)
   {
        //surface.BuildNavMesh();
   }

   public void _updateSurfaceDestroy(ObjectDestroyedEvent e)
   {
        if (e.name != "Objective")
        {
            surface.BuildNavMesh();
        }
   }
}
