package model;

import util.Constant;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.util.Map;

public class MulticastHandler implements Runnable {
    private final MulticastSocket multicastSocket;
    private final InetAddress multicastGroup;
    private final Map<String, Entity> entityMap;

    public MulticastHandler(MulticastSocket multicastSocket, InetAddress multicastGroup, Map<String, Entity> entityMap) {
        this.multicastSocket = multicastSocket;
        this.multicastGroup = multicastGroup;
        this.entityMap = entityMap;
    }

    @Override
    public void run() {
        while (true) {
            try {
                sendPlayersPosition();
                Thread.sleep(50);
            } catch (IOException e) {
                e.printStackTrace();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }
    }

    private void sendPlayersPosition() throws IOException {
        if (entityMap.isEmpty()) {
            return;
        }
        StringBuilder stringBuilder = new StringBuilder();
        for (Map.Entry<String, Entity> entry : entityMap.entrySet()) {
            if (!entry.getValue().isAlive()) {
                stringBuilder.append(entry.getKey()).append(";").append("Disconnect").append("|");
                entityMap.remove(entry.getKey());
                continue;
            }
            stringBuilder
                    .append(entry.getKey()).append(";")
                    .append(entry.getValue().getPosition().getX() / Constant.SCREEN_WIDTH).append(";")
                    .append(entry.getValue().getPosition().getY() / Constant.SCREEN_HEIGHT).append(";")
                    .append(entry.getValue().getHitBox().getX() / Constant.SCREEN_WIDTH).append(";")
                    .append(entry.getValue().getHitBox().getY() / Constant.SCREEN_HEIGHT).append("|");
        }
        //System.out.println(stringBuilder);
        byte[] serverMessageBuffer = stringBuilder.toString().getBytes();
        DatagramPacket sendDatagramPacket = new DatagramPacket(serverMessageBuffer, serverMessageBuffer.length, multicastGroup, Constant.MULTICAST_PORT);
        multicastSocket.send(sendDatagramPacket);
    }
}
