extends Path3D

@export var malemoniak: PackedScene

@onready var spawn_location : PathFollow3D = $PTF_Locations


func _on_timer_timeout():
	var entity = malemoniak.instantiate()
	
	spawn_location.progress_ratio = randf()
	
	entity.position = spawn_location.position
	add_child(entity)
