extends Node

signal city_health_changed(hit_points : int, max_hit_points : int)
func _on_city_health_changed(hit_points : int, max_hit_points : int):
	city_health_changed.emit(hit_points, max_hit_points)


signal city_fame_changed(player_fame : int)
func _on_city_fame_changed(player_fame : int):
	city_fame_changed.emit(player_fame)


signal malemoniak_damage_city(damage : int)
func _on_malemoniak_damage_city(damage : int):
	malemoniak_damage_city.emit(damage)
