using System.Collections;
using Builds;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private CastItem castItem;
    [SerializeField] [Range(0, 10)] private float _attackSpeed;

    private GameObject _target;
    private bool _canCast;

    public void SetTarget(GameObject target)
    {
        if (_target == null)
        {
            _target = target;
            _canCast = true;

            StartCoroutine(Casting());
        }
    }

    public void ResetTarget()
    {
        _target = null;
        _canCast = false;
    }

    public void StartCasting() => _canCast = true;

    public void PauseCasting() => _canCast = false;

    private IEnumerator Casting()
    {
        while (_canCast == true && _target != null)
        {
            // TODO you need to use the object pool and add checks
            var missile = Instantiate(castItem, transform.position, Quaternion.identity);
            missile.Init(_target.transform);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }
}