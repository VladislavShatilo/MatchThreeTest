using System;

public class SwipeDetector
{
    private ScreenPoint startPos;
    private bool dragging;

    private const float Threshold = 30f;

    public bool TryProcess(bool down, bool up, ScreenPoint position, out SwipeDirection direction)
    {
        direction = default;

        if (down)
        {
            startPos = position;
            dragging = true;
            return false;
        }

        if (!dragging)
            return false;

        if (up)
        {
            dragging = false;

            var deltaX = position.X - startPos.X;
            var deltaY = position.Y - startPos.Y;

            if (GetMagnitude(deltaX, deltaY) < Threshold)
                return false;

            direction = GetDirection(deltaX, deltaY);
            return true;
        }

        return false;
    }

    private static float GetMagnitude(float deltaX, float deltaY)
    {
        return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    private static SwipeDirection GetDirection(float deltaX, float deltaY)
    {
        if (Math.Abs(deltaX) > Math.Abs(deltaY))
            return deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left;

        return deltaY > 0 ? SwipeDirection.Up : SwipeDirection.Down;
    }
}
