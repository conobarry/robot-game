import sys, builtins

# Override the python print function and add @print to the front of any output.
def print(*args, sep=' ', end='\n', file=sys.stdout, flush=False):
    builtins.print("@print:", end='')
    builtins.print(*args, sep=sep, end=end, file=file, flush=flush)

# Prepends @command to *args and prints to STDOUT using the builtin print function.
def _print_command(command, *args):
    builtins.print("@command:", end='')
    builtins.print(command, *args)

# Defines methods and properties of the robot that the player can call.
#   Calling these methods prints that command to STDOUT.
class robot:
    def command(command, *args):
        _print_command(command, *args)
    def move_forward(distance):
        robot.command("move-forward", distance)    
    def move_backward(distance):
        robot.command("move-backward", distance)
    def turn_right(angle):
        robot.command("turn-right", angle)
    def turn_left(angle):
        robot.command("turn-left", angle)

# Remember to end the file on a blank line