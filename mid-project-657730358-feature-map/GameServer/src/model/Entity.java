package model;

import util.Vector2;

public abstract class Entity {
    private final long id;
    private final Vector2 position;
    private final Vector2 hitBox;
    private boolean isAlive;
    private boolean isChanged;

    public Entity(Vector2 position, Vector2 hitBox) {
        this.id = System.currentTimeMillis();
        this.position = position;
        this.hitBox = hitBox;
        this.isAlive = true;
        this.isChanged = true;
    }

    abstract public String getType();

    public long getId() {
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

    public boolean isChanged() {
        return isChanged;
    }

    public void setChanged(boolean changed) {
        isChanged = changed;
    }
}
