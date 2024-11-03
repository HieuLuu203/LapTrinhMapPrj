package model;

import util.Vector2;

public class Wall extends Entity {
    public Wall(Vector2 position, Vector2 hitBox) {
        super(position, hitBox);
    }

    @Override
    public String getType() {
        return "Wall";
    }

    @Override
    public String toString() {
        return "Wall{" +
                "id=" + getId() +
                ", position=" + getPosition() +
                ", hitBox=" + getHitBox() +
                '}';
    }
}
