package model.action;

import util.Vector2;

public interface Moveable {
    float getSpeed();

    void move(Vector2 deltaPosition);

    void moveTo(float x, float y);
}
