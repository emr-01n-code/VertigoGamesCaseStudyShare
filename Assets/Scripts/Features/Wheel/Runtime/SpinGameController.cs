using UnityEngine;
using UnityEngine.SceneManagement;
using VertigoCase.Shared.Events;

namespace Project.Features.Wheel.Runtime.Controllers
{
    public class SpinGameController : MonoBehaviour
    {
        private void OnEnable()
        {
            //EventManager.Subscribe<RunExitRequestedEvent>(OnExit);
            EventManager.Subscribe<RunRestartRequestedEvent>(OnRestart);
        }

        private void OnDisable()
        {
            //EventManager.Unsubscribe<RunExitRequestedEvent>(OnExit);
            EventManager.Unsubscribe<RunRestartRequestedEvent>(OnRestart);
        }

        private void OnRestart(RunRestartRequestedEvent _)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            EventManager.ClearAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
