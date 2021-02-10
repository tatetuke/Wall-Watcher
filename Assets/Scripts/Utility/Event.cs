using System.Collections.Generic;

namespace RPGM.Core
{
    /// <summary>
    /// An event allows execution of some logic to be deferred for a period of time.
    /// </summary>
    /// <typeparam name="Event"></typeparam>
    public class Event : System.IComparable<Event>
    {
        public System.Action function;
        public System.Func<bool> condition;

      //  protected GameModel model = Schedule.GetModel<GameModel>();

        public  float tick;

        public int CompareTo(Event other)
        {
            return tick.CompareTo(other.tick);
        }

        public  void ExecuteEvent()
        {
            if (condition==null||condition())
            {
                function();
            }
        }

        public  void Cleanup()
        {

        }
    }

}