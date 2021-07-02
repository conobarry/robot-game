import sys
import robot
import time

def test(): 
    
    robot.move_forward(5)
    # time.sleep(2)
    print("Hello there")
    print >> sys.stderr, "This is an error"
    print("another line")
    
    robot.move_backward(5)
    # die()
    return 42