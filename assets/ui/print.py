import sys
import robot

def test(): 
    robot.move_forward()
    print("Hello there")
    print >> sys.stderr, "This is an error"
    return 42