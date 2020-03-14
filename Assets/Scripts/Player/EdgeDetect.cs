using UnityEngine;

public class EdgeDetect : MonoBehaviour {
    
    float radius = 0.1f;
    PlayerControls player;

    private void Awake() {
        player = GetComponentInParent(typeof(PlayerControls)) as PlayerControls;
    }

    private void Update() {
        LayerMask mask = 1 << gameObject.layer;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius,mask);
        if(hitColliders.Length == 0){
            player.fullStop();
        }
        else{
            player.notOnEdge();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}