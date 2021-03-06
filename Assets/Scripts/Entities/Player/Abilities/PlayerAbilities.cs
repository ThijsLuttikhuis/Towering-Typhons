using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAbilities : MonoBehaviour {
    [SerializeField] private Player player;

    [SerializeField] private Ability attack;
    [SerializeField] private Ability ability1;
    [SerializeField] private Ability ability2;
    [SerializeField] private Ability ability3;
    [SerializeField] private Ability ability4;

    private Transform rayTarget;
    private Vector3 rayPosition;

    private Transform lastMoveRayTarget;
    private Vector3 lastMoveRayPosition;
    
    enum LastButtonPressed {
        none = -1,
        stop = 0,
        ability1 = 1,
        ability2 = 2,
        ability3 = 3,
        ability4 = 4,
        move = 5
    }

    private LastButtonPressed lastButtonPressed = LastButtonPressed.none;

    private bool buttonStopMove = false;
    private bool buttonAbility1 = false;
    private bool buttonAbility2 = false;
    private bool buttonAbility3 = false;
    private bool buttonAbility4 = false;
    private bool buttonMove = false;


    private void OnMove() {
        buttonMove = !buttonMove;
    }

    private void OnAbility1() {
        buttonAbility1 = !buttonAbility1;
    }

    private void OnAbility2() {
        buttonAbility2 = !buttonAbility2;
    }

    private void OnAbility3() {
        buttonAbility3 = !buttonAbility3;
    }

    private void OnAbility4() {
        buttonAbility4 = !buttonAbility4;
    }

    private void OnStopMove() {
        buttonStopMove = !buttonStopMove;
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;
        
        // Update mouse target if any button is pressed
        if (buttonAbility1 || buttonAbility2 || buttonAbility3 || buttonAbility4 || buttonMove) {
            Transform targetTransform;
            Vector3 targetPosition;

            (targetTransform, targetPosition) = player.GetRayPosition();
            rayTarget = targetTransform;
            rayPosition = targetPosition;
        }

        // Update last button pressed
        lastButtonPressed =
            buttonAbility1 ? LastButtonPressed.ability1 :
            buttonAbility2 ? LastButtonPressed.ability2 :
            buttonAbility3 ? LastButtonPressed.ability3 :
            buttonAbility4 ? LastButtonPressed.ability4 :
            buttonMove ? LastButtonPressed.move :
            buttonStopMove ? LastButtonPressed.stop : lastButtonPressed;

        Debug.Log(lastButtonPressed);

        // Update the ability corresponding to the last button pressed

        if (!rayTarget) return;

        if (rayTarget.CompareTag("Enemy")) {
            switch (lastButtonPressed) {
                case LastButtonPressed.ability1:
                    ability1.UpdateCast(dt, rayTarget, rayPosition);
                    break;
                case LastButtonPressed.ability2:
                    break;
                case LastButtonPressed.ability3:
                    break;
                case LastButtonPressed.ability4:
                    break;
                case LastButtonPressed.move:
                    lastMoveRayTarget = rayTarget;
                    lastMoveRayPosition = rayPosition;
                    
                    attack.UpdateCast(dt, rayTarget, rayPosition);
                    break;
            }
        }
        else if (rayTarget.CompareTag("Terrain")) {
            if (lastButtonPressed == LastButtonPressed.move || buttonMove) {
                Vector3 eulerAngles;
                Vector3 position;
                (eulerAngles, position) =
                    attack.UpdatePlayerPosition(dt, rayPosition, transform.position, player.getMoveSpeed);

                player.Move(eulerAngles, position);
            }
        }
        else if (lastMoveRayTarget) {
            if (lastMoveRayTarget.CompareTag("Enemy")) {
                Vector3 eulerAngles;
                Vector3 position;
                (eulerAngles, position) =
                    attack.UpdatePlayerPosition(dt, lastMoveRayPosition, transform.position, player.getMoveSpeed);
                
                player.Move(eulerAngles, position);
            }
            else if (lastMoveRayTarget.CompareTag("Terrain")) {
                Vector3 eulerAngles;
                Vector3 position;
                (eulerAngles, position) =
                    attack.UpdatePlayerPosition(dt, lastMoveRayTarget.transform.position, transform.position, player.getMoveSpeed);
                
                player.Move(eulerAngles, position);
            }
        }
    }
}
