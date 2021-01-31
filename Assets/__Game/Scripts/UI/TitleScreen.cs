namespace Game
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class TitleScreen : MonoBehaviour
    {
        private void Awake()
        {
            var terrainGenerator = FindObjectOfType<TerrainGenerator>();
            if (terrainGenerator != null) Destroy(terrainGenerator.gameObject);
        }

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetButtonDown("Fire1"))
            {
                GetComponent<Animator>().Play("FadeToBlack");
                Invoke(nameof(LoadGameplayScene), 0.6f);
            }
        }

        private void LoadGameplayScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
