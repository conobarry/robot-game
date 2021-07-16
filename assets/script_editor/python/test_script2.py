import sys
import time
import robot2 as robot
    
robot.move_forward(1)
# time.sleep(2)
print("Hello there")
print >> sys.stderr, "This is an error"
print("another line")

robot.move_backward(5)