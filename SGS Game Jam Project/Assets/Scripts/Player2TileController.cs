using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class Player2TileController : MonoBehaviour
{
    public GameObject SelectedTile;
    public float moveSpeed = 5f;

    private Vector3 targetScale;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private float cooldownDuration = 2f; // Default cooldown time
    private float countdown = 2f;        // Countdown tracker

    public float raycastDistance;
    public Material HoverMaterial;
    public Material StandardMaterial;

    private Renderer HitTile;
    private Renderer PrevHitObject;

    public GameObject Camera;
    public GameObject SmokeVfx;
    public PlayerInput PlayerInput;

    private Color originalColor;
    private bool isHovering = false;

    public GameObject ActiveTile;

    public Animator PlayerAnimator;

    public Slider CooldownSlider;

    private void Start()
    {
        // Subscribe to input actions
        PlayerInput.actions["TileUp"].performed += ctx => MoveTile(true);
        PlayerInput.actions["TileDown"].performed += ctx => MoveTile(false);
    }

    void Update()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        if (!isMoving)
        {
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    HitTile = hit.collider.gameObject.GetComponent<MeshRenderer>();

                    SelectedTile = HitTile.gameObject;

                    if (HitTile != PrevHitObject)
                    {
                        HitTile.material = HoverMaterial;
                        if (PrevHitObject != null) PrevHitObject.material = StandardMaterial;
                        PrevHitObject = HitTile;
                    }
                }
                else if (PrevHitObject != null)
                {
                    PrevHitObject.material = StandardMaterial;
                    PrevHitObject = null;
                }
            }
        }

        if (isMoving)
        {
            CooldownSlider.gameObject.SetActive(true);
            CooldownSlider.value = countdown;

            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                SelectedTile.transform.localScale = Vector3.Lerp(SelectedTile.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
                SelectedTile.transform.position = Vector3.Lerp(SelectedTile.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                isMoving = false;
                countdown = cooldownDuration; // Use the adjustable cooldown
                CooldownSlider.gameObject.SetActive(false);
            }
        }
    }

    private void MoveTile(bool moveUp)
    {
        if (ActiveTile != SelectedTile)
        {
            if (isMoving || HitTile == null) return;

            isMoving = true;
            SelectedTile = HitTile.gameObject;

            targetScale = SelectedTile.transform.localScale;
            targetPosition = SelectedTile.transform.position;

            if (moveUp)
            {
                targetScale.z += 15;
                PlayerAnimator.SetBool("IsLifting", true);
                StartCoroutine(AnimationStop(0.5f));
                AudioManager.Instance.PlaySound("ROCK RISING", 1, 0.7f, 0f, 1.5f);
            }
            else
            {
                targetScale.z -= 15;
                PlayerAnimator.SetBool("IsLowering", true);
                StartCoroutine(AnimationStop(0.5f));
                AudioManager.Instance.PlaySound("ROCK LOWERING", 1, 0.7f, 0f, 1.2f);
            }

            targetPosition = new Vector3(SelectedTile.transform.position.x, targetScale.z * 0.0397325f, SelectedTile.transform.position.z);
            SpawnSmoke(new Vector3(SelectedTile.transform.position.x, 0, SelectedTile.transform.position.z));
        }
        else
        {
            Debug.Log("You cannot move the tile you're on");
        }
    }

    IEnumerator AnimationStop(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerAnimator.SetBool("IsLifting", false);
        PlayerAnimator.SetBool("IsLowering", false);
    }

    private void SpawnSmoke(Vector3 position)
    {
        Instantiate(SmokeVfx, position, Quaternion.identity);
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            ActiveTile = col.gameObject;
        }
    }

    void OnCollisionLeave(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            ActiveTile = null;
        }
    }

    // Get/Set for external scripts like powerups
    public float GetCooldown()
    {
        return cooldownDuration;
    }

    public void SetCooldown(float newCooldown)
    {
        cooldownDuration = newCooldown;

        // If not in the middle of a cooldown, update the countdown immediately
        if (!isMoving)
        {
            countdown = newCooldown;
        }
    }

}
