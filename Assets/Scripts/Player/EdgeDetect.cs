using UnityEngine;

public class EdgeDetect : MonoBehaviour {
    
   
    PlayerControls player;

    private void Awake() {
        player = GetComponentInParent(typeof(PlayerControls)) as PlayerControls;
    }

    private void Update() {
        LayerMask mask = 1 << player.gameObject.layer;
        RaycastHit raycastHit;
        
        if (!Physics.Raycast(transform.position, -transform.up, out raycastHit, 5.5f, mask) && player != null)
        {
            player.fullStop();
            Debug.DrawRay(transform.position, -transform.up * 5.5f, Color.white);
            
        }
        else
        {
            
            player.notOnEdge();
           Debug.DrawRay(transform.position, -transform.up * raycastHit.distance, Color.yellow);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
       // Gizmos.DrawSphere(transform.position, radius);
    }
}