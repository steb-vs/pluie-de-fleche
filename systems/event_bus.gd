extends Node

signal city_health_changed(hit_points : int, max_hit_points : int)

func _on_city_health_changed(hit_points : int, max_hit_points : int):
	city_health_changed.emit(hit_points, max_hit_points)
