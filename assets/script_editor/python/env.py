import sys, builtins

# Override the python print function and add @print to the front of any output. 
#   This shows the script manager that this data was output by this script or the user
def print(*args, sep=' ', end='\n', file=sys.stdout, flush=False):
    builtins.print("@print:", end='')
    builtins.print(*args, sep=sep, end=end, file=file, flush=flush)

def _print_command(command, *args):
    builtins.print("@command:", end='')
    builtins.print(command, *args)

class robot:
    def command(command, *args):
        _print_command(command, *args)
    def move_forward(distance):
        robot.command("move-forward", distance)    
    def move_backward(distance):
        robot.command("move-backward", distance)

# Remember to end the file on a blank line