import model.Entity;
import model.MulticastHandler;
import util.Constant;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class GameServer {
    private DatagramSocket serversocket;
    private MulticastSocket multicastSocket;
    private InetAddress multicastGroup;
    private final Map<String, Entity> entityMap = new ConcurrentHashMap<>();
    private ServerHandler serverHandler;
    private MulticastHandler multicastHandler;

    public void startServer() throws IOException {
        serversocket = new DatagramSocket(Constant.SERVER_PORT);
        multicastSocket = new MulticastSocket();
        multicastGroup = InetAddress.getByName(Constant.MULTICAST_GROUP);
        serverHandler = new ServerHandler(serversocket, entityMap);
        multicastHandler = new MulticastHandler(multicastSocket, multicastGroup, entityMap);
        System.out.println("Server started");

        Thread serverThread = new Thread(serverHandler);
        serverThread.start();
        Thread multicastThread = new Thread(multicastHandler);
        multicastThread.start();
    }
}
