using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimation : MonoBehaviour
{
    private float WaitBetween = 0.15f;
    private List<Animator> _animators = new List<Animator>();
    private Bomb BombScript;

    void OnEnable()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(AnimateScore());
        BombScript = GameObject.Find("Engine").GetComponent<Bomb>();
        BombScript.AddScore(100);
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * 0.01f;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator AnimateScore()
    {
        foreach (Animator eachAnimator in _animators)
        {
            eachAnimator.SetTrigger("Animate");
            yield return new WaitForSeconds(WaitBetween);
        }
        yield return new WaitForSeconds(1.1f);
        this.gameObject.SetActive(false);
    }
}
