extends Label


# Called when the node enters the scene tree for the first time.
func _ready():
	EventBus.city_fame_changed.connect(_on_city_fame_changed)


func _on_city_fame_changed(player_fame : int):
	text = "Fame : " + str(player_fame)
