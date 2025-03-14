using UnityEngine;

public class TheItem : MonoBehaviour
{
    public ItemConfig itemConfig;  
    private Rigidbody myRigidbody;

    private void Start()
    {
        if (itemConfig != null)
        {
            if (myRigidbody == null)
            {
                myRigidbody = GetComponent<Rigidbody>();
            }

            myRigidbody.mass = itemConfig.weight;
            myRigidbody.useGravity = true;
            GetComponent<DraggableItem>().itemType = itemConfig.itemType;
        }
    }
}
