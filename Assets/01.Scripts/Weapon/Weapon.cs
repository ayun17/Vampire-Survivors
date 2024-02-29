using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent onAttack;

    [SerializeField] private float weaponDelay = 1f, attackTime = 0.2f;
    [SerializeField] private Vector2 originPosition;

    public int weaponDamage = 1;

    public bool isAttacking = false;
    private bool _delayCoroutine = false;

    public void UseWeapon(Vector2 targetPos)
    {
        if (!isAttacking && !_delayCoroutine)
        {
            isAttacking = true;
            StabbingAttack(targetPos);
        }
    }

    private void StabbingAttack(Vector2 targetPos) // 찌르기 공격
    {
        transform.DOMove(targetPos, attackTime).OnComplete(() =>
        {
            onAttack?.Invoke();
            transform.DOLocalMove(originPosition, attackTime).OnComplete(() => FinishAttack());
        });
    }

    private void FinishAttack()
    {
        StartCoroutine(DelayNextCoroutine());
    }

    private IEnumerator DelayNextCoroutine()
    {
        _delayCoroutine = true;
        yield return new WaitForSeconds(weaponDelay);
        _delayCoroutine = false;
        isAttacking = false;
    }

    public void FinishShakeWeapon()
    {
        transform.DOComplete(); // 모든 트윈 종료
    }
}
