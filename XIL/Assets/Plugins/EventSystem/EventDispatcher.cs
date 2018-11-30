
namespace  XIL.EventSystem
{
    public class EventDispatcher
    {
        public static EventDispatcher instance = new EventDispatcher();

        private EventManager eventCommonManager = new EventManager();

        public EventManager MainEventManager
        {
            get { return this.eventCommonManager; }
            private set { }
        }

    }
}