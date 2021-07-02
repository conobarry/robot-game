import sys, builtins

def print(*args, sep=' ', end='\n', file=sys.stdout, flush=False):
    builtins.print("#output ", end='')
    builtins.print(*args, sep=sep, end=end, file=file, flush=flush)

# Remember to end the file on a blank line