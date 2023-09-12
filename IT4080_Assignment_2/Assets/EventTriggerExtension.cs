using UnityEngine.UI;
using UnityEngine.EventSystems;


public static class EventTriggerExtension
{
    public static void AddEventTrigger(this Button button, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => { action((BaseEventData)data); });
        trigger.triggers.Add(entry);
    }
}
