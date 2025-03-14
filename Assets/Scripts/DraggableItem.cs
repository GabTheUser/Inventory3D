using UnityEngine;
using System.Collections;

public class DraggableItem : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isBeingSnapped = false; 

    private Rigidbody myRigidbody;
    private Transform targetPosition; 
    private bool isInBackpackTrigger = false; 

    [HideInInspector] public ItemType itemType; 
    private Backpack backPack;
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        backPack = FindAnyObjectByType<Backpack>();
    }

    private void OnMouseDown()
    {
        if (isDragging) return;

        myRigidbody.isKinematic = true; 
        GetComponent<Collider>().isTrigger = true;
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        transform.position = GetMouseWorldPosition() + offset; 
    }

    private void OnMouseUp()
    {
        myRigidbody.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
        isDragging = false;

        if (isInBackpackTrigger && targetPosition != null && !isDragging)
        {
            StartCoroutine(MoveItemToBackpack());
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 8f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SetTargetPosition(Transform slotTransform)
    {
        targetPosition = slotTransform;
    }

    public IEnumerator MoveItemToBackpack()
    {
        if (targetPosition == null) yield break;

        isBeingSnapped = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosition.position; 
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, endPosition) > 0.05f)
        {
            float distanceCovered = (Time.time - startTime) * 2f; 
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;
        }
        backPack.AddToInventory(GetComponent<TheItem>());
        myRigidbody.isKinematic = true; 
        GetComponent<Collider>().isTrigger = true;
        transform.SetLocalPositionAndRotation(endPosition, targetPosition.rotation);
        isBeingSnapped = false;
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            isInBackpackTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            isInBackpackTrigger = false; 
        }
    }

    public void MakeAvailable()
    {
        enabled = true;
        GetComponent<Collider>().isTrigger = false; 
        GetComponent<Collider>().enabled = true;
        myRigidbody.isKinematic = false;  

        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;

        float randomX = Random.Range(-1f, 1f);  
        float randomZ = Random.Range(-1f, 1f);  

        Vector3 randomDirection = new Vector3(randomX, 0f, randomZ).normalized;

        myRigidbody.AddForce(randomDirection * 50f, ForceMode.Impulse); 

    }
}
