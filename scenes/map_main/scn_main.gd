extends Node3D

func _ready():
	$PTH_Spawner.start()

func _input(event):
	if event.is_action_pressed("exit"):
		get_tree().quit()
