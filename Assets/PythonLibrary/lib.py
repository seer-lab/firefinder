# Player Location
playerX = 0
playerY = 0
# Game board
board = [[]]
# Goal Location
goalX = 0
goalY = 0

# Check if obstacle in given direction


def check(dir):
    if(dir == "up"):
        return bool(board[playerX, playerY + 1] < 3)
    if(dir == "down"):
        return bool(board[playerX, playerY - 1] < 3)
    if(dir == "left"):
        return bool(board[playerX - 1, playerY] < 3)
    if(dir == "right"):
        return bool(board[playerX + 1, playerY] < 3)
    return bool(0)

# Move in given direction if there is nothing preventing player


def move(dir):
    global playerX
    global playerY
    if(dir == "up"):
        if(check(dir)):
            print dir
            playerY += 1
    if(dir == "down"):
        if(check(dir)):
            print dir
            playerY -= 1
    if(dir == "left"):
        if(check(dir)):
            print dir
            playerX -= 1
    if(dir == "right"):
        if(check(dir)):
            print dir
            playerX += 1


# while(playerX != goalX):
# 	if(playerX < goalX):
# 		move("right")
# 	else:
# 		move("left")

# while(playerY != goalY):
# 	if (playerY < goalY):
# 		move("up")
# 	else:
# 		move("down")
