using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public SpawnableManager spawnableManager;
    public int index;

    private bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.tag == "Spawnable" && collision.gameObject.GetComponent<CollisionDetector>().index == 0 && spawnableManager.collisionsCount < spawnableManager.maxCollisions)
        {
            if (index == 1)
            {
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Random.ColorHSV();
                }
            }
            spawnableManager.collisionsCount++;
            if (spawnableManager.collisionsCount >= spawnableManager.maxCollisions)
            {
                spawnableManager.text.text = "Game Complete";
            }
            else
            {
                spawnableManager.text.text = "Collisions: " + spawnableManager.collisionsCount.ToString() + "/" + spawnableManager.maxCollisions.ToString();
            }
            hasCollided = true;
        }
    }
}
