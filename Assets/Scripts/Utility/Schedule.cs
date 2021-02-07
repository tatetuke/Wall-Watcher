using System;
using System.Collections.Generic;
using UnityEngine;


namespace RPGM.Core
{
    /// <summary>
    /// The Schedule class implements the discrete event simulator pattern.
    /// Events are pooled.
    /// </summary>
    public static partial class Schedule
    {

        static HeapQueue<Event> eventQueue = new HeapQueue<Event>();
        static Dictionary<System.Type, Stack<Event>> eventPools = new Dictionary<System.Type, Stack<Event>>();

        /// <summary>
        /// Create a new event of type T and return it, but do not schedule it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T New<T>() where T : Event, new()
        {
            Stack<Event> pool;
            if (!eventPools.TryGetValue(typeof(T), out pool))
            {
                pool = new Stack<Event>(32);
                pool.Push(new T());
                eventPools[typeof(T)] = pool;
            }
            //もしpoolにobjectがあれば再利用
            if (pool.Count > 0)
                return (T)pool.Pop();
            else
                return new T();//新しく生成
        }

        /// <summary>
        /// Clear all pending events and reset the tick to 0.
        /// </summary>
        public static void Clear()
        {
            eventQueue.Clear();
        }

        /// <summary>
        /// Schedule an event for a future tick, and return it.
        /// </summary>
        /// <returns>The event.</returns>
        /// <param name="tick">Tick.</param>
        /// <typeparam name="T">The event type parameter.</typeparam>
        static public T Add<T>(System.Action function,float tick = 0) where T : Event, new()
        {
            var ev = New<T>();
            ev.tick = Time.time + tick;
            ev.function = function;
            eventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// Reschedule an existing event for a future tick, and return it.
        /// </summary>
        /// <returns>The event.</returns>
        /// <param name="tick">Tick.</param>
        /// <typeparam name="T">The event type parameter.</typeparam>
        static public T Add<T>(T ev, System.Action function, float tick) where T : Event, new()
        {
            ev.tick = Time.time + tick;
            ev.function = function;
            eventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// Tick the simulation. Returns the count of remaining events.
        /// If remaining events is zero, the simulation is finished unless events are
        /// injected from an external system via a Schedule() call.
        /// </summary>
        /// <returns></returns>
        static public int Tick()
        {
            var time = Time.time;
            var executedEventCount = 0;
            //ゲーム内時間がeventのtickを超えていたら
            while (eventQueue.Count > 0 && eventQueue.Peek().tick <= time)
            {
                var ev = eventQueue.Pop();
                var tick = ev.tick;
                ev.ExecuteEvent();
                if (ev.tick > tick)
                {
                    //event was rescheduled, so do not return it to the pool.
                    //tickが変化したのでまたQueueに登録する？
                    eventQueue.Push(ev);
                }
                else
                {
                    // Debug.Log($"<color=green>{ev.tick} {ev.GetType().Name}</color>");
                    ev.Cleanup();
                    try
                    {
                        //poolに流し、再利用されるまで待たせる
                        eventPools[ev.GetType()].Push(ev);
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.LogError($"No Pool for: {ev.GetType()}");
                    }
                }
                executedEventCount++;
            }
            return eventQueue.Count;
        }
    }
}


