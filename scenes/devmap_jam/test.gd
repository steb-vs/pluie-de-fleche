extends Node3D

@onready var spawner : Path3D = $PTH_Spawner

func _ready():
	spawner.start_wave()


func _on_timer_timeout():
	if $CHB_Malemoniak:
		$CHB_Malemoniak.take_damage(3)
