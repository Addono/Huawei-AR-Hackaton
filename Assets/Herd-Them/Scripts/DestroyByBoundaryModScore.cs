using UnityEngine;

namespace Scripts
{
    public class DestroyByBoundaryModScore : MonoBehaviour
    {

        private HerdThemController _herdThemController;

        private void Start()
        {
            GameObject world = GameObject.FindWithTag("GameController");
            _herdThemController = world.GetComponent<HerdThemController>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("GoodPig"))
            {
                _herdThemController.DecreaseScore();
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("Enemy"))
            {
                _herdThemController.IncreaseScore();
                Destroy(other.gameObject);
            }
        }
    }
}
