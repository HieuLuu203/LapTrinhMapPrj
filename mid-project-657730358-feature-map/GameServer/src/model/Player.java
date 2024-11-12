package model;

import model.action.Moveable;
import util.Constant;
import util.Vector2;

public class Player extends Entity implements Moveable {
    public Player(Vector2 position, Vector2 hitBox) {
        super(position, hitBox);
    }

    @Override
    public String getType() {
        return "Player";
    }

    @Override
    public float getSpeed() {
        return Constant.PLAYER_SPEED;
    }

    @Override
    public void move(Vector2 deltaPosition) {
        getPosition().add(deltaPosition);
    }

    @Override
    public void moveTo(float x, float y) {
        getPosition().set(x, y);
    }

    @Override
    public String toString() {
        return "Player{" +
                "id=" + getId() +
                ", position=" + getPosition() +
                '}';
    }
}
