extends Node3D

func _on_timer_timeout():
	if has_node("CHB_Malemoniak") :
		$CHB_Malemoniak.taking_damage(3)
	else:
		$Timer.stop()
