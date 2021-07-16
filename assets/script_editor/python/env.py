import sys, builtins

# Override the python print function and add @print to the front of any output. 
#   This shows the script manager that this data was output by this script or the user
def print(*args, sep=' ', end='\n', file=sys.stdout, flush=False):
    operations = ["@command"]
    operation = args[0]
    if (not operation in operations):
        builtins.print("@print ", end='')    
    builtins.print(*args, sep=sep, end=end, file=file, flush=flush)

class robot:
    def command(cmd):
        print("@command", cmd)
    def move_forward(distance):
        robot.command(f"move-forward {distance}")    
    def move_backward(distance):
        robot.command(f"move-backward {distance}")

# Remember to end the file on a blank line