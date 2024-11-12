import model.Entity;
import util.Constant;
import util.Preferences;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.util.Map;

public class MulticastHandler implements Runnable {
    private final MulticastSocket multicastSocket;
    private final InetAddress multicastGroup;
    private final Map<Long, Entity> entityMap;

    public MulticastHandler(MulticastSocket multicastSocket, InetAddress multicastGroup, Map<Long, Entity> entityMap) {
        this.multicastSocket = multicastSocket;
        this.multicastGroup = multicastGroup;
        this.entityMap = entityMap;
    }

    @Override
    public void run() {
        while (true) {
            try {
                sendPlayersPosition();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private void sendPlayersPosition() throws IOException {
        if (!Preferences.getInstance().isReceiveInput()) {
            return;
        }
        if (entityMap.isEmpty()) {
            return;
        }
        StringBuilder stringBuilder = new StringBuilder();
        for (Map.Entry<Long, Entity> entry : entityMap.entrySet()) {
//            System.out.println(entry.getValue());
            if (!entry.getValue().isAlive()) {
                stringBuilder.append(entry.getKey()).append(";").append("Disconnect").append("|");
                entityMap.remove(entry.getKey());
                continue;
            }
            if (!entry.getValue().isChanged()) {
                continue;
            }
            stringBuilder
                    .append(entry.getKey()).append(";")
                    .append(entry.getValue().getType()).append(";")
                    .append(entry.getValue().getPosition().getX() / Constant.SCREEN_WIDTH).append(";")
                    .append(entry.getValue().getPosition().getY() / Constant.SCREEN_HEIGHT).append(";")
                    .append(entry.getValue().getHitBox().getX() / Constant.SCREEN_WIDTH).append(";")
                    .append(entry.getValue().getHitBox().getY() / Constant.SCREEN_HEIGHT).append("|");
            entry.getValue().setChanged(false);
        }
        if (stringBuilder.isEmpty()) {
            return;
        }
//        System.out.println(stringBuilder);
//        System.out.println(entityMap.size());
        byte[] serverMessageBuffer = stringBuilder.toString().getBytes();
        DatagramPacket sendDatagramPacket = new DatagramPacket(serverMessageBuffer, serverMessageBuffer.length, multicastGroup, Constant.MULTICAST_PORT);
        multicastSocket.send(sendDatagramPacket);
        Preferences.getInstance().setReceiveInput(false);
    }
}
