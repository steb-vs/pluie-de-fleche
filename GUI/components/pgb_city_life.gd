extends ProgressBar

func _ready() -> void:
	EventBus.city_health_changed.connect(_on_city_health_changed)

func update_display(hit_points : int, max_hitpoints : int):
	max_value = max_hitpoints
	value = hit_points


func _on_city_health_changed(hit_points : int, max_hitpoints : int):
	update_display(hit_points, max_hitpoints)
