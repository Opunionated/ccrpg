using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour { //TODO Incomplete Class

    private List<EventAnimation> queue = new List<EventAnimation>();

    public void QueueAnimation(EventAnimation animation)
    {
        queue.Add(animation);
    }
}
