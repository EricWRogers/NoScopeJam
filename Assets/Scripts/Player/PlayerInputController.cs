using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInputController : MonoBehaviour
{
    public struct PlayerInput
    {
        public Vector3 Move;
        public bool Sprint;
        public bool Jump;
        public bool JumpStart;
        public bool Slide;
        public bool SlideStart;
    }

    private PlayerInput _playerInput = new PlayerInput();

    public void Update()
    {
        UpdatePlayerInput();
    }

    public void UpdatePlayerInput()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        _playerInput.Move = new Vector3(horizontal, 0f, vertical);
        _playerInput.Sprint = CrossPlatformInputManager.GetButton("Sprint");
        _playerInput.Jump = CrossPlatformInputManager.GetButton("Jump");
        _playerInput.JumpStart = CrossPlatformInputManager.GetButtonDown("Jump");
        _playerInput.Slide = CrossPlatformInputManager.GetButton("Slide");
        _playerInput.SlideStart = CrossPlatformInputManager.GetButtonDown("Slide");
    }

    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }
}