package util;

public class Preferences {
    private static volatile Preferences instance;
    private volatile boolean isReceiveInput;

    private Preferences() {
    }

    public static Preferences getInstance() {
        if (instance == null) {
            synchronized (Preferences.class) {
                if (instance == null) {
                    instance = new Preferences();
                }
            }
        }
        return instance;
    }

    public boolean isReceiveInput() {
        return isReceiveInput;
    }

    public void setReceiveInput(boolean receiveInput) {
        isReceiveInput = receiveInput;
    }
}
