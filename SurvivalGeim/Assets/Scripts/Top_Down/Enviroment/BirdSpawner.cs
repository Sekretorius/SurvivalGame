using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class BirdSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 timeSpawnInterval = new Vector2(0, 10);
        [SerializeField]
        private int poolSize = 10;
        [SerializeField]
        private GameObject bird;

        [SerializeField]
        private float mapWidth = 0;
        [SerializeField]
        private float mapHeight = 0;

        [SerializeField]
        private Vector2 birdSizeRange = new Vector2(0.4f, 0.7f);

        private Queue<GameObject> pool = new Queue<GameObject>();
        private float nextBirdSpawnTime = 0;
        private float timmer = 0;
        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject birdObject = Instantiate(bird, transform);
                birdObject.SetActive(false);
                pool.Enqueue(birdObject);
            }
            nextBirdSpawnTime = Random.Range(timeSpawnInterval.x, timeSpawnInterval.y);
        }

        private void Update()
        {
            if (timmer >= nextBirdSpawnTime)
            {
                BirdController birdController = Take();
                if (birdController == null)
                {
                    return;
                }
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                float size = Random.Range(birdSizeRange.x, birdSizeRange.y);

                birdController.spawnerInstance = this;
                birdController.MoveDirection = new Vector2(direction, 0);
                birdController.FlipSprite(direction != -1);
                birdController.TargetDistance = mapWidth + mapWidth * 0.25f;
                birdController.transform.position = new Vector3(mapWidth * 0.75f * -direction, Random.Range(-mapHeight / 2, mapHeight / 2), 0);
                birdController.SetSize(new Vector3(size, size, size));
                birdController.gameObject.SetActive(true);

                timmer = 0;
                nextBirdSpawnTime = Random.Range(timeSpawnInterval.x, timeSpawnInterval.y);
            }
            timmer += Time.deltaTime;
        }

        public void Join(GameObject birdObject)
        {
            birdObject.SetActive(false);
            pool.Enqueue(birdObject);
        }
        public BirdController Take()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue().GetComponent<BirdController>();
            }
            return null;
        }
    }
}
