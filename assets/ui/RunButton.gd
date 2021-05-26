extends Button


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
#	var button = Button.new()
#	button.text = "Click me"
#	button.connect("pressed", self, "_button_pressed")
#	add_child(button)
	pass
	

func _button_pressed():
	if Engine.has_singleton("JavaScript"):
		print("yes")
		var js = Engine.get_singleton("JavaScript")
		var js_return = js.eval("var myNumber = 1; return myNumber;")
		print(js_return)
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
