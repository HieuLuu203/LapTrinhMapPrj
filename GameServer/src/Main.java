import java.io.IOException;

public class Main {
    public static void main(String[] args) throws IOException, InterruptedException {
        GameServer gameServer = new GameServer();
        gameServer.startServer();
    }
}