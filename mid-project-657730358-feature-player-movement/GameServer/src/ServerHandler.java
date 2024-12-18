import model.Entity;
import model.Player;
import util.Constant;
import util.Preferences;
import util.Vector2;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.util.Map;

public class ServerHandler implements Runnable {
    private final DatagramSocket socket;
    private final Map<Long, Entity> entityMap;

    public ServerHandler(DatagramSocket socket, Map<Long, Entity> entityMap) {
        this.socket = socket;
        this.entityMap = entityMap;
    }

    @Override
    public void run() {
        while (true) {
            byte[] receiveBuffer = new byte[256];
            DatagramPacket receiveDatagramPacket = new DatagramPacket(receiveBuffer, receiveBuffer.length);
            try {
                socket.receive(receiveDatagramPacket);
                String received = new String(receiveDatagramPacket.getData(), 0, receiveDatagramPacket.getLength());
                System.out.println("received: " + received);
                if (received.equals("Connect to server")) {
                    connectToServer(receiveDatagramPacket.getAddress(), receiveDatagramPacket.getPort());
                } else if (received.startsWith("Disconnect")) {
                    String[] stat = received.split(";");
                    entityMap.get(Long.parseLong(stat[1])).setAlive(false);
                    Preferences.getInstance().setReceiveInput(true);
                } else {
                    String[] stat = received.split("\\|");
                    processInput(Long.parseLong(stat[0]), stat[1], Float.parseFloat(stat[2]));
                }
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private void connectToServer(InetAddress clientAddress, int clientPort) throws IOException {
        Vector2 hitBox = new Vector2(Constant.SCREEN_WIDTH / 32f, Constant.SCREEN_HEIGHT / 18f);
        Vector2 position = new Vector2(
                hitBox.getX() / 2 + (float) (Math.random() * (Constant.SCREEN_WIDTH - hitBox.getX())),
                hitBox.getY() / 2 + (float) (Math.random() * (Constant.SCREEN_HEIGHT - hitBox.getY()))
        );
        Entity player = new Player(position, hitBox);
        entityMap.put(player.getId(), player);
        for (Map.Entry<Long, Entity> entry : entityMap.entrySet()) entry.getValue().setChanged(true);
        String serverMessage = Long.toString(player.getId());
        System.out.println("connect send: " + serverMessage);
        byte[] serverMessageBuffer = serverMessage.getBytes();
        DatagramPacket sendDatagramPacket = new DatagramPacket(serverMessageBuffer, serverMessageBuffer.length, clientAddress, clientPort);
        socket.send(sendDatagramPacket);
        Preferences.getInstance().setReceiveInput(true);
    }

    private void processInput(long id, String input, float deltaTime) {
        String[] singleInput = input.split(";");
        Player player = (Player) entityMap.get(id);
        Vector2 direction = new Vector2(0.f, 0.f);
        for (String x : singleInput) {
            switch (x) {
                case "W" -> direction.add(0.f, 1.f);
                case "A" -> direction.add(-1.f, 0.f);
                case "S" -> direction.add(0.f, -1.f);
                case "D" -> direction.add(1.f, 0.f);
            }
        }
        direction.normalize();
        player.move(direction.multiply(player.getSpeed() * deltaTime));
        player.setChanged(true);
        Preferences.getInstance().setReceiveInput(true);
    }
}
