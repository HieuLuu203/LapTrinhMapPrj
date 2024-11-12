# Game Bomb Online

## Thành viên
1. Lê Trí Tâm - B21DCCN657
   
2. Lê Quốc Trung - B21DCCN730
   
3. Lưu Minh Hiếu - B21DCCN358

## Mô tả bài toán

Game multiplayer thiết kế theo client-server như sau:

Game bao gồm nhiều người chơi được phép di chuyển, đặt bom. Bản đồ của game là một lưới 2 chiều bao gồm các chướng ngại vật mà người chơi không thể đi qua được. Một người chơi có số lượng bom nhất định. Mỗi lần đặt bom sẽ tốn 1 giây để hồi lại 1 quả bom. Quả bom sau khi được đặt xuống bản đồ (tại vị trí người chơi có thể di chuyển được) thì sau 3 giây nó sẽ nổ. Khi nổ quả bom nổ theo 4 hướng trên dưới trái phải, độ dài các hướng tuỳ theo thuộc tính của người chơi. Nếu trên 1 đường nổ có chướng ngại vật, nó sẽ phá huỷ chướng ngại vật đó (lưu ý nếu trên đường nổ có nhiều chướng ngại vật thì chỉ phá huỷ chướng ngạt vật đầu tiên mà đường nổ đi qua, các chướng ngại vật còn lại ko bị huỷ). Nếu bom nổ trúng người chơi nào đó (bất kể là người chơi đang điều khiển hay người chơi khác) thì người đó sẽ bị xử thua và không thể chơi tiếp. Mỗi 15 giây, sẽ có một item xuất hiện mà khi người chơi nào đó đi qua thì sẽ nhận được trong đó có 70% tăng chiều dài của đường nổ lên 1 ô, 30% tăng số lượng của bom lên 1

## Công nghệ sử dụng:

- Server: UDP Socket, Multicast,.. (Java Core)
- Client: Game Engine (Unity)
