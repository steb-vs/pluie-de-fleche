extends Node

signal health_changed(hit_points : int, max_hitpoints : int)
signal fame_changed(player_fame : int)

var max_hit_points : int = 1000
var hit_points : int = max_hit_points:
	get:
		return hit_points
	set(value):
		hit_points = clamp(value, 0, max_hit_points)
		health_changed.emit(hit_points, max_hit_points)

var regen_per_sec : int = 1

var player_fame : int = 0:
	get:
		return player_fame
	set(value):
		player_fame = value
		fame_changed.emit(player_fame)


func _ready():
	health_changed.connect(EventBus._on_city_health_changed)
	fame_changed.connect(EventBus._on_city_fame_changed)
	EventBus.malemoniak_damage_city.connect(_on_malemoniak_damage_city)
	EventBus.malemoniak_killed.connect(_on_malemoniak_killed)
	hit_points = 950




func regen() -> void:
	hit_points += regen_per_sec


func _on_tmr_regen_timeout() -> void:
	regen()


func _on_malemoniak_damage_city(damage : int) -> void:
	take_damage(damage)


func take_damage(amount : int) -> void:
	hit_points -= amount


func _on_malemoniak_killed(bounty_fame : int) -> void:
	player_fame += bounty_fame
