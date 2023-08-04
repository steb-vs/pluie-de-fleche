extends Node3D

@onready var spawner : Path3D = $PTH_Spawner

func _ready():
	spawner.start_wave()
