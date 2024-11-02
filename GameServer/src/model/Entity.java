package model;

import util.Vector2;

import java.util.UUID;

public abstract class Entity {
    private final String id = UUID.randomUUID().toString();
    private final Vector2 position;
    private final Vector2 hitBox;
    private boolean isAlive;

    public Entity(Vector2 position, Vector2 hitBox) {
        this.position = position;
        this.hitBox = hitBox;
        this.isAlive = true;
    }

    public String getId() {
        return id;
    }

    public Vector2 getPosition() {
        return position;
    }

    public Vector2 getHitBox() {
        return hitBox;
    }

    public boolean isAlive() {
        return isAlive;
    }

    public void setAlive(boolean alive) {
        isAlive = alive;
    }
}
