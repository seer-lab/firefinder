# Player Location
playerX = 0
playerY = 0
# Game board
board = [[]]
boardX = 0
boardY = 0
# Goal Location
goalX = 0
goalY = 0

# Check if obstacle in given direction


def check(direction):
    if(direction == "up"):
        if(playerY < boardY - 1):
            return bool(board[playerX, playerY + 1] <= 0)
    if(direction == "down"):
        if(playerY > 0):
            return bool(board[playerX, playerY - 1] <= 0)
    if(direction == "left"):
        if(playerX > 0):
            return bool(board[playerX - 1, playerY] <= 0)
    if(direction == "right"):
        if(playerX < boardX - 1):
            return bool(board[playerX + 1, playerY] <= 0)
    return bool(0)

# Move in given direction if there is nothing preventing player


def move(direction):
    global playerX
    global playerY
    if(direction == "up"):
        if(check(direction)):
            print direction
            playerY += 1
    if(direction == "down"):
        if(check(direction)):
            print direction
            playerY -= 1
    if(direction == "left"):
        if(check(direction)):
            print direction
            playerX -= 1
    if(direction == "right"):
        if(check(direction)):
            print direction
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
