extends Area3D


func _on_body_entered(body):
	if body is Malemoniak:
		var malemoniak : Malemoniak = body
		malemoniak.attack_city()
