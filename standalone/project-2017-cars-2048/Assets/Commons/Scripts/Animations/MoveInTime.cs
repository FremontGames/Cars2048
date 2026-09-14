using UnityEngine;
using System.Collections;

namespace Commons.Animations
{
    public class MoveInTime : MonoBehaviour
    {
        public Vector3 begin;
        public Vector3 end;
        public float time_for_move = 0.7f;

        protected float timer;

        protected void Start()
        {
            transform.position = begin;
            timer = time_for_move;
        }

        protected void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                Vector3 distance = end - begin;
                float degree_of_movement = (time_for_move - timer) / time_for_move;
                transform.position = new Vector3(
                    begin.x + (distance.x * degree_of_movement),
                    begin.y + (distance.y * degree_of_movement),
                    begin.z + (distance.z * degree_of_movement)
                    );
            }
        }
    }

    public class MoveInTimeThenDestroy : MoveInTime
    {
        new void Update()
        {
            base.Update();
            if (timer <= 0)
                Destroy(gameObject);
        }
    }

    public class MoveInTimeDirectionThenDestroy : MoveInTimeDirection
    {
        new void Update()
        {
            base.Update();
            if (timer <= 0)
                Destroy(gameObject);
        }
    }

    public class MoveInTimeDirection : MoveInTime
    {
        public Vector3 move = new Vector3(0, 100, 0);

        new void Start()
        {
            begin = transform.position;
            end = new Vector3(
                begin.x + move.x,
                begin.y + move.y,
                begin.z + move.z);
            base.Start();
        }
    }
}