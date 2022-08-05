using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingManager ring_manager;
    public float speed;
    public Transform player;

    private float _ring_size;
    private bool has_not_passed = true;
    public float ring_size
    {
        get { return _ring_size; }
        set { 
            _ring_size = value;
            this.transform.localScale = new Vector3(ring_size, this.transform.localScale.y, ring_size);
        }
    }

    void Update()
    {
        this.transform.position += new Vector3(0, 0, -speed);
        if (player.position.z >= this.transform.position.z )
        {
            if (has_not_passed)
            {
                ring_manager.RingScore(isThroughRing(player.position), this);
                has_not_passed = false;
            }
        }
        else
        {
            has_not_passed = true;
        }
    }

    bool isThroughRing(Vector3 pos)
    {
        return new Vector2((pos.y - this.transform.position.y) , (pos.x - this.transform.position.x)).magnitude < ring_size;
    }


}
