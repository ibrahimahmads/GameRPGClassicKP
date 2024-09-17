using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Idle
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Animator animator;
    private Vector2 roamPosition; // Menyimpan posisi roaming

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (true)
        {
            if (state == State.Roaming)
            {
                // Musuh bergerak ke posisi yang ditentukan
                animator.SetBool("IsMoving", true);
                roamPosition = GetRoamingPosition();
                enemyPathFinding.MoveTo(roamPosition);
                
                // Tunggu beberapa detik saat musuh bergerak
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                
                // Setelah selesai bergerak, ubah state menjadi idle
                state = State.Idle;
            }

            if (state == State.Idle)
            {
                // Set animasi idle (IsMoving = false)
                animator.SetBool("IsMoving", false);

                enemyPathFinding.StopMovement();
                
                // Berhenti selama beberapa detik sebelum mulai bergerak lagi
                yield return new WaitForSeconds(Random.Range(2f, 3f));

                // Setelah idle, ubah state kembali ke roaming
                state = State.Roaming;
            }
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Dapatkan posisi baru yang acak untuk bergerak
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
