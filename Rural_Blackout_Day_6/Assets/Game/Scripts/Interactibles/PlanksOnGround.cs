using UnityEngine;

public class PlanksOnGround : Interactable {

    public GameObject pickupEffect;

    [SerializeField]
    private PlayerInteraction _playerInteraction;

    public override void OnPress() {
        if(_playerInteraction._planksCollectedCount < _playerInteraction._maxPlanks) {
            Instantiate(pickupEffect, transform.position, transform.rotation);
            _playerInteraction._planksCollectedCount++;
            Destroy(gameObject);
        }
    }
}