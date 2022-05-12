using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Games.TicTacToe
{
    public class GameCameraShake : MonoBehaviour
    {
        private CinemachineVirtualCamera m_virtualCamera;

        private void Awake()
        {
            m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void Shake(float intensity, float durationSeconds) =>
            StartCoroutine(ShakeForSeconds(intensity, durationSeconds));

        private IEnumerator ShakeForSeconds(float intensity, float durationSeconds)
        {
            BeginShake(intensity);
            yield return new WaitForSeconds(durationSeconds);
            EndShake();
        }
        
        private void BeginShake(float intensity)
        {
            var perlin = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = intensity;
        }

        private void EndShake()
        {
            var perlin = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0.0f;
        }
    }
}
