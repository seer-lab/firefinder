import sys
sys.path.append(r"c:\python27\lib")

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
            return bool(board[playerX, playerY + 1] != 'x')
    if(direction == "down"):
        if(playerY > 0):
            return bool(board[playerX, playerY - 1] != 'x')
    if(direction == "left"):
        if(playerX > 0):
            return bool(board[playerX - 1, playerY] != 'x')
    if(direction == "right"):
        if(playerX < boardX - 1):
            return bool(board[playerX + 1, playerY] != 'x')
    return bool(0)

# Move in given direction if there is nothing preventing player


def move(direction):
    global playerX
    global playerY
    if(direction == "up"):
        if(check(direction)):
            playerY += 1
            print direction
    if(direction == "down"):
        if(check(direction)):
            playerY -= 1
            print direction
    if(direction == "left"):
        if(check(direction)):
            playerX -= 1
            print direction
    if(direction == "right"):
        if(check(direction)):
            playerX += 1
            print direction


# for i in range(0,100):
# 	if(playerX < goalX):
# 		move("right")
# 	elif(playerX > goalX):
# 		move("left")
# 	if (playerY < goalY):
# 		move("up")
# 	elif(playerY > goalY):
# 		move("down")

# for i in range(0,100):
# 	if(playerX < goalX):
# 		if(check("right")):
# 			move("right")
# 		elif(check("down")):
# 			move("down")
# 	if(playerY > goalY):
# 		if(check("down")):
# 			move("down")
# 		elif(check("right")):
# 			move("right")

# for i in range(0,100):
# 	if(playerX < goalX):
# 		if(check("right")):
# 			move("right")
# 		elif(check("down")):
# 			move("down")
# 	else:
# 		if(check("left")):
# 			move("left")
# 		elif(check("up")):
# 			move("up")
# 	if(playerY > goalY):
# 		if(check("down")):
# 			move("down")
# 		elif(check("right")):
# 			move("right")
# 	else:
# 		if(check("up")):
# 			move("up")
# 		elif(check("left")):
# 			move("left")
	
	