using System;

public class EasingFunctions
{
    static public float EaseOutQuint(float currentTime, float startValue, float change, int duration)
    {
        currentTime /= duration;
        currentTime--;
        return change * ((float)Math.Pow(currentTime, 5) + 1) + startValue;
    }
}
