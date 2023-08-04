extends Node

signal health_changed(hit_points : int, max_hitpoints : int)

var max_hit_points : int = 1000
var hit_points : int = max_hit_points:
	get:
		return hit_points
	set(value):
		hit_points = clamp(value, 0, max_hit_points)
		health_changed.emit(hit_points, max_hit_points)

var regen_per_sec : int = 1

func _ready():
	health_changed.connect(EventBus._on_city_health_changed)
	hit_points = 300
	regen_per_sec = 50




func regen():
	hit_points += regen_per_sec


func _on_tmr_regen_timeout():
	regen()
